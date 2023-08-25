using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class PersistentStore
{
    public HashSet<Global.Level> reachedLevels = new HashSet<Global.Level>();
    // public bool gameCompleted = false;
}

[RequireComponent(typeof(Singleton))]
public class PersistentStoreManager : MonoBehaviour
{
    [SerializeField] private string saveFileName = "store.dat";
    private PersistentStore store = null;

    // State variables
    private ObjectSerializer<PersistentStore> serializer;
    private string saveFilePath;

    void Awake()
    {
        saveFilePath = Path.Combine(
            Application.persistentDataPath,
            saveFileName
        );
        serializer = new ObjectSerializer<PersistentStore>();

        // Load the game save
        Load();

        // Call the Save function when the game quits
        Application.quitting += Save;
    }

    // Load the store from disk to memory
    public void Load()
    {
        if (File.Exists(saveFilePath))
        {
            store = serializer.Read(saveFilePath);
            Debug.Log("Persistent data loaded from disk");
        }
        // If the file doesn't exist, create one
        else
        {
            store = new PersistentStore();
            Save();
            Debug.Log($"No persistent save file found. Created one on disk in {saveFilePath}.");
        }
    }

    // Write the store from memory to disk
    public void Save()
    {
        serializer.Write(store, saveFilePath);
        Debug.Log("Persistent data written to disk");
    }

    // Below are mutators for the persistent store
    public void AddLevelReached(Global.Level level)
    {
        store.reachedLevels.Add(level);
        Save();
    }

    // public void SetGameCompleted(bool value)
    // {
    //     store.gameCompleted = value;
    //     Save();
    // }

    // Below are getters for the persistent store
    public bool QueryLevelReached(Global.Level level)
    {
        return store.reachedLevels.Contains(level);
    }

    // public bool GetGameCompleted()
    // {
    //     return store.gameCompleted;
    // }
}
