using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

// This is what is being serialized
[System.Serializable]
public class SerializableCutsceneData
{
    public string ID;
    public bool hasPlayed;
}

// CutsceneData has two responsibilities:
// 1. Upon scene load, it attempts to load the cutscene's "Has Played" state from the GameManager
// 2. It captures the cutscene's "Has Played" state for the GameManager to serialize
[RequireComponent(typeof(CutsceneTrigger))]
public class CutsceneData : MonoBehaviour
{
    private static List<string> _guids = new List<string>();

    // Associates checkpoints with their save data
    [SerializeField] private string myID;
    public string ID { get { return myID; } }

    // Cached component references
    private CutsceneTrigger cutsceneTrigger;

    // Cached GameObject references 
    private GameManager gameManager;

    void Reset()
    {
        RegenerateGUID();
    }

    void Awake()
    {
        // Show warning if no GUID
        if (myID == "")
            Debug.LogError($"[CutsceneData] Cutscene data is missing a GUID!", gameObject);
        // Check if GUID is unique
        else if (_guids.Contains(myID))
            Debug.LogError($"[CutsceneData] Duplicated GUID detected: {myID}", gameObject);
        else
            _guids.Add(myID);
    }

    private void OnDestroy()
    {
        _guids.Remove(myID);
    }

    void Start()
    {
        cutsceneTrigger = GetComponent<CutsceneTrigger>();

        // Register the player with the GameManager
        gameManager = FindObjectOfType<GameManager>();
        gameManager.cutsceneData.Add(this);

        Load();
    }

    // Attempt to load previous game data from the Game Manager
    void Load()
    {
        GameData gameData = gameManager?.gameData;
        if (gameData == null) return;

        // Get the build index from the saved level
        bool buildIndexFound = Global.LevelToBuildIndexMap.TryGetValue(gameData.level, out int savedBuildIndex);
        if (!buildIndexFound) return;

        // Ensure build index matches before using the loaded gameData
        bool buildIndexMatch = savedBuildIndex == SceneManager.GetActiveScene().buildIndex;
        if (!buildIndexMatch) return;

        // Ensure cutscene data exists
        List<SerializableCutsceneData> allCutsceneData = gameData?.cutsceneData;
        SerializableCutsceneData cutsceneData = allCutsceneData?.Find(x => x.ID == ID);

        if (cutsceneData == null)
            Debug.LogWarning($"[CutsceneData] Cutscene data not found for cutscene {ID}", gameObject);
        else
        {
            Debug.Log($"[CutsceneData] Loading cutscene data for cutscene {ID}", gameObject);
            if (cutsceneData.hasPlayed)
            {
                cutsceneTrigger.hasPlayed = true;
                StartCoroutine(GoToEndStateWhenReady());
            }
        }
    }

    // Wait until the cutscenes that this cutscene depends on have been played. 
    // Then evalue the last frame of the cutscene (fast forward to end).
    IEnumerator GoToEndStateWhenReady()
    {
        yield return new WaitUntil(() => cutsceneTrigger.AllDependenciesMet());
        cutsceneTrigger.GoToEndState();
    }

    // Returns a serializable version of the player's data
    public SerializableCutsceneData Capture()
    {
        return new SerializableCutsceneData()
        {
            hasPlayed = cutsceneTrigger.hasPlayed,
            ID = ID
        };
    }

    [ContextMenu("Regenerate GUID")]
    private void RegenerateGUID()
    {
        myID = System.Guid.NewGuid().ToString();
    }
}
