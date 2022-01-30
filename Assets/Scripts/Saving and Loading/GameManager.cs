using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Singleton))]
public class GameManager : MonoBehaviour
{
    // Configuration parameters
    [Header("File saving")]

    // Object references with attributes to save
    [Header("Object References")]
    public PlayerData player;
    public List<Checkpoint> checkpointData;

    // State variables
    string saveFilePath;
    GameSerializer serializer;
    public GameData gameData;

    void Awake()
    {
        // Set up the game manager
        saveFilePath = Path.Combine(
            Application.persistentDataPath,
            "save.dat"
        );
        serializer = new GameSerializer();
        gameData = null;
    }

    // Debugging purposes
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
            Save();

        if (Input.GetKeyDown(KeyCode.L))
            Load();
    }

    // Save the game to the savefile
    public void Save()
    {
        // Create a game data object
        gameData = new GameData();

        // Capture the game data
        gameData.sceneIndex = SceneManager.GetActiveScene().buildIndex;
        gameData.playerData = player.Capture();

        // Write to file
        serializer.Save(gameData, saveFilePath);

        Debug.Log("Game saved");
    }

    // Load the game data from the savefile
    public void Load()
    {
        if (!CanLoad())
        {
            Debug.Log("No save file found");
            return;
        }

        // Read from file
        gameData = serializer.Load(saveFilePath);
        Debug.Log("Game loaded. Reloading scene.");

        // Reload the scene
        int sceneIndex = gameData.sceneIndex;
        FindObjectOfType<LevelLoader>().LoadScene(sceneIndex);

        // It is now the responsibility of the saved GameObjects in the reloaded scene
        // to reference the GameManager and retrieve the loaded game data.
    }

    // Returns true if a savefile exists, false otherwise
    public bool CanLoad()
    {
        return File.Exists(saveFilePath);
    }
}
