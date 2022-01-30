using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This is what is being serialized
[System.Serializable]
public class SerializablePlayerData
{
    public float[] position = new float[3];
}

// PlayerData has two responsibilities:
// 1. Upon scene load, it attempts to load the player data from the GameManager
// 2. It captures the player data for the GameManager to serialize
public class PlayerData : MonoBehaviour
{
    public SerializablePlayerData playerData;
    public GameManager gameManager;

    void Start()
    {
        // Register the player with the GameManager
        gameManager = FindObjectOfType<GameManager>();
        gameManager.player = this;

        // Attempt to load previous game data from the Game Manager
        GameData gameData = gameManager.gameData;
        if (gameData != null)
        {
            playerData = gameData.playerData;
            transform.position = new Vector3(
                playerData.position[0],
                playerData.position[1],
                playerData.position[2]
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
            }
        };
    }
}
