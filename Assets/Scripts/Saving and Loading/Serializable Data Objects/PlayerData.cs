using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// This is what is being serialized
[System.Serializable]
public class SerializablePlayerData
{
    public float[] position = new float[3];
    public float[] respawnPos = new float[3];
}

// PlayerData has two responsibilities:
// 1. Upon scene load, it attempts to load the player data from the GameManager
// 2. It captures the player data for the GameManager to serialize
[RequireComponent(typeof(PlayerMovement))]
public class PlayerData : MonoBehaviour
{
    // Cached component references
    private PlayerMovement playerMovement;

    // Cached GameObject references
    private GameManager gameManager;

    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();

        // Register the player with the GameManager
        gameManager = FindObjectOfType<GameManager>();
        gameManager.player = this;

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
        
        // Ensure player data exists 
        SerializablePlayerData playerData = gameData?.playerData;
        if (playerData != null)
        {
            transform.position = new Vector3(
                playerData.position[0],
                playerData.position[1],
                playerData.position[2]
            );
            playerMovement.respawnPos = new Vector3(
                playerData.respawnPos[0],
                playerData.respawnPos[1],
                playerData.respawnPos[2]
            );
        }
    }

    // Returns a serializable version of the player's data
    public SerializablePlayerData Capture()
    {
        return new SerializablePlayerData()
        {
            position = new float[] {
                transform.position.x,
                transform.position.y,
                transform.position.z
            },
            respawnPos = new float[] {
                playerMovement.respawnPos.x,
                playerMovement.respawnPos.y,
                playerMovement.respawnPos.z,
            }
        };
    }
}
