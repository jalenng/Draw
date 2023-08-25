using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Singleton))]
public class GameManager : MonoBehaviour
{
    // Objects with attributes to save
    [Header("Save Data Objects")]
    public PlayerData player;
    public List<CheckpointData> checkpointData;
    public List<CutsceneData> cutsceneData;

    // Object for other saveData objects to retrieve from
    public GameData gameData = null;

    // State variables
    string saveFilePath;
    ObjectSerializer<GameData> serializer;

    void Awake()
    {
        // Set up the game manager
        saveFilePath = Path.Combine(
            Application.persistentDataPath,
            "save.dat"
        );
        serializer = new ObjectSerializer<GameData>();
        gameData = null;

        // Call the Save function when the game quits
        Application.quitting += Save;
    }

    void Update()
    {
        // For dev and debugging convenience
        if (Debug.isDebugBuild && Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (Input.GetKeyDown(KeyCode.S))
                Save();

            if (Input.GetKeyDown(KeyCode.L))
                Load();
        }
    }

    // Save the game to the savefile
    public void Save()
    {
        // Create a game data object
        GameData newGameData = new GameData();

        // Capture the game data
        newGameData.sceneIndex = SceneManager.GetActiveScene().buildIndex;
        newGameData.playerData = player?.Capture();
        
        newGameData.checkpointData = new List<SerializableCheckpointData>();
        foreach (CheckpointData checkpoint in checkpointData)
        {
            if (checkpoint != null) {
                newGameData.checkpointData.Add(checkpoint.Capture());
            }
        }
        
        newGameData.cutsceneData = new List<SerializableCutsceneData>();
        foreach (CutsceneData cutscene in cutsceneData)
        {
            if (cutscene != null) {
                newGameData.cutsceneData.Add(cutscene.Capture());
            }
        }

        // Write to file
        serializer.Write(newGameData, saveFilePath);

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
        gameData = serializer.Read(saveFilePath);
        Debug.Log("Game loaded. Reloading scene.");

        // Reload the scene
        int sceneIndex = gameData.sceneIndex;
        FindObjectOfType<SceneLoader>().LoadScene(sceneIndex);

        // It is now the responsibility of the saved GameObjects in the reloaded scene
        // to reference the GameManager and retrieve the loaded game data.
    }

    // Returns true if a game save exists, false otherwise
    public bool CanLoad()
    {
        return serializer.CanRead(saveFilePath);
    }
}
