using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

// This is what is being serialized
[System.Serializable]
public class SerializableCheckpointData
{
    public string ID;
    public bool isActivated;
}

// CheckpointData has two responsibilities:
// 1. Upon scene load, it attempts to load the checkpoint data from the GameManager
// 2. It captures the checkpoint data for the GameManager to serialize
[RequireComponent(typeof(Checkpoint))]
public class CheckpointData : MonoBehaviour
{
    // Configuration parameters

    // Associates checkpoints with their save data
    [SerializeField] private string myID;
    public string ID { get { return myID; } }

    // Cached component references
    private Checkpoint checkpoint;

    // Cached GameObject references 
    private GameManager gameManager;

    void Reset()
    {
        RegenerateGUID();
    }

    void Start()
    {
        checkpoint = GetComponent<Checkpoint>();

        // Register the player with the GameManager
        gameManager = FindObjectOfType<GameManager>();
        gameManager.checkpointData.Add(this);

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
        if (buildIndexMatch)
        {
            List<SerializableCheckpointData> allCheckpointData = gameManager?.gameData?.checkpointData;
            SerializableCheckpointData checkpointData = allCheckpointData?.Find(x => x.ID == ID);

            // Ensure checkpoint data exists
            if (checkpointData == null)
                Debug.LogWarning("[CheckpointData] Checkpoint data not found for checkpoint " + ID);
            else if (checkpointData.isActivated)
                checkpoint.Activate();
        }
    }

    // Returns a serializable version of the player's data
    public SerializableCheckpointData Capture()
    {
        return new SerializableCheckpointData()
        {
            isActivated = checkpoint.IsActivated,
            ID = ID
        };
    }

    [ContextMenu("Regenerate GUID")]
    private void RegenerateGUID()
    {
        myID = System.Guid.NewGuid().ToString();
    }
}
