using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

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
    // Configuration parameters
    
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

    void Start()
    {
        cutsceneTrigger = GetComponent<CutsceneTrigger>();

        // Register the player with the GameManager
        gameManager = FindObjectOfType<GameManager>();
        gameManager.cutsceneData.Add(this);

        // Attempt to load previous game data from the Game Manager
        List<SerializableCutsceneData> allCutsceneData = gameManager?.gameData?.cutsceneData;
        SerializableCutsceneData cutsceneData = allCutsceneData?.Find(x => x.ID == ID);
        if (cutsceneData == null)
            Debug.Log("Cutscene data not found for cutscene " + ID);
        else if (cutsceneData.hasPlayed)
        {
            cutsceneTrigger.hasPlayed = true;
            // Get all siblings of cutscene trigger and disable them
            foreach (Transform child in transform.parent)
                child.gameObject.SetActive(false);
        }
            
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
    private void RegenerateGUID () {
        myID = System.Guid.NewGuid().ToString();
    }
}
