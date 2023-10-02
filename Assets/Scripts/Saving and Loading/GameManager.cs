using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Singleton))]
public class GameManager : MonoBehaviour
{
    [SerializeField] private string saveFileName = "save.dat";

    // Objects with attributes to save
    [Header("Save Data Objects")]
    public PlayerData player;
    public List<CheckpointData> checkpointData;
    public List<CutsceneData> cutsceneData;
    public List<OrangeObjectData> orangeObjectData;

    // Object for other saveData objects to retrieve from
    public GameData gameData = null;

    // State variables
    private string saveFilePath;
    private ObjectSerializer<GameData> serializer;

    void Awake()
    {
        Debug.unityLogger.logEnabled = Debug.isDebugBuild;

        // Set up the game manager
        saveFilePath = Path.Combine(
            Application.persistentDataPath,
            saveFileName
        );
        serializer = new ObjectSerializer<GameData>();
        gameData = null;

        SceneManager.sceneLoaded += OnSceneLoaded;

        // Call the Save function when the game quits
        Application.quitting += Save;
    }

    void Update()
    {
        // For dev and debugging convenience
        if (Debug.isDebugBuild && Input.GetKey(KeyCode.LeftShift))
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
        int currentBuildIndex = SceneManager.GetActiveScene().buildIndex;
        Global.Level currentLevel = Global.GetLevelFromBuildIndex(currentBuildIndex);

        // Ensure this is a scene that we should be saving progress for
        if (currentLevel == Global.Level.NONE)
        {
            Debug.Log("[GameManager] Tried to save but current scene is not a game level");
            return;
        }

        // Capture level state
        newGameData.level = currentLevel;

        // Capture player state
        if (player != null)
        {
            newGameData.playerData = player.Capture();
        }

        // Capture checkpoint state
        newGameData.checkpointData = new List<SerializableCheckpointData>();
        foreach (CheckpointData checkpoint in checkpointData)
        {
            if (checkpoint != null)
            {
                newGameData.checkpointData.Add(checkpoint.Capture());
            }
        }

        // Capture cutscene state
        newGameData.cutsceneData = new List<SerializableCutsceneData>();
        foreach (CutsceneData cutscene in cutsceneData)
        {
            if (cutscene != null)
            {
                newGameData.cutsceneData.Add(cutscene.Capture());
            }
        }
        newGameData.orangeObjectData = new List<SerializableOrangeObjectData>();
        foreach (OrangeObjectData orangeObject in orangeObjectData)
        {
            if (orangeObject != null)
            {
                newGameData.orangeObjectData.Add(orangeObject.Capture());
            }
        }

        // Write to file
        serializer.Write(newGameData, saveFilePath);

        Debug.Log("[GameManager] Game saved");
    }

    // Load the game data from the savefile
    public void Load()
    {
        if (!CanLoad())
        {
            Debug.Log("[GameManager] No save file found");
            return;
        }

        // Read from file
        gameData = serializer.Read(saveFilePath);
        Debug.Log("[GameManager] Game loaded. Reloading scene.");

        // Load the scene        
        bool buildIndexFound = Global.LevelToBuildIndexMap.TryGetValue(gameData.level, out int savedBuildIndex);
        if (buildIndexFound)
        {
            FindObjectOfType<SceneLoader>().LoadScene(savedBuildIndex);
        }

        // It is now the responsibility of the saved GameObjects in the reloaded scene
        // to reference the GameManager and retrieve the loaded game data.
    }

    // Returns true if a game save exists, false otherwise
    public bool CanLoad()
    {
        return serializer.CanRead(saveFilePath);
    }

    // Action for when a new scene is loaded.
    // If the new scene doesn't match the scene for the save file, clear the cached save.
    public void OnSceneLoaded(Scene scene, LoadSceneMode _) {
        // If no cached save file, there's nothing to clear
        if (gameData == null) return;

        // Check if the build indices match
        int loadedSceneIndex = scene.buildIndex;
        bool loadedSaveBuildIndexFound = Global.LevelToBuildIndexMap.TryGetValue(gameData.level, out int loadedSaveBuildIndex);
        bool currentGameDataIsValid = loadedSaveBuildIndexFound && (loadedSceneIndex == loadedSaveBuildIndex);
        if (!currentGameDataIsValid) {
            gameData = null;
            Debug.Log($"[GameManager] The loaded scene ({loadedSceneIndex}) didn't match the saved scene ({loadedSaveBuildIndex}). Cleared the cached game save.");
        }
    }
}
