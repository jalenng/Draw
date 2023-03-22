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
    public List<CheckpointData> checkpointData;
    public List<CutsceneData> cutsceneData;

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

        // SetupSaveOnQuit();
    }

    void Start()
    {
        SetRatio(3, 2);
    }

    void SetRatio(float w, float h)
    {
        if ((((float)Screen.width) / ((float)Screen.height)) > w / h)
        {
            Screen.SetResolution((int)(((float)Screen.height) * (w / h)), Screen.height, true);
        }
        else
        {
            Screen.SetResolution(Screen.width, (int)(((float)Screen.width) * (h / w)), true);
        }
    }

    void SetupSaveOnQuit()
    {
        Application.quitting += Save;
    }

    void Update()
    {
        // For dev and debugging convenience
        if (Debug.isDebugBuild)
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
        gameData = new GameData();

        // Capture the game data
        gameData.sceneIndex = SceneManager.GetActiveScene().buildIndex;
        gameData.playerData = player?.Capture();
        
        gameData.checkpointData = new List<SerializableCheckpointData>();
        foreach (CheckpointData checkpoint in checkpointData)
        {
            //TODO: improve this
            if (checkpoint == null)
                continue;
            gameData.checkpointData.Add(checkpoint.Capture());
        }
        
        gameData.cutsceneData = new List<SerializableCutsceneData>();
        foreach (CutsceneData cutscene in cutsceneData)
        {
            //TODO: Improve this
            if (cutscene == null)
                continue;
            gameData.cutsceneData.Add(cutscene.Capture());
        }

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
        FindObjectOfType<SceneLoader>().LoadScene(sceneIndex);

        // It is now the responsibility of the saved GameObjects in the reloaded scene
        // to reference the GameManager and retrieve the loaded game data.
    }

    // Returns true if a savefile exists, false otherwise
    public bool CanLoad()
    {
        return File.Exists(saveFilePath);
    }
}
