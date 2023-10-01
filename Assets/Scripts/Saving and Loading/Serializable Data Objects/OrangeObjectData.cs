using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// This is what is being serialized
[System.Serializable]
public class SerializableOrangeObjectData
{
    public Vector3 savedPosition;
    public RigidbodyType2D bodyType;
    public string ID;
}

// PlayerData has two responsibilities:
// 1. Upon scene load, it attempts to load the player data from the GameManager
// 2. It captures the player data for the GameManager to serialize
[RequireComponent(typeof(OrangeObject))]
public class OrangeObjectData : MonoBehaviour
{
    // Configuration parameters
    // Associates checkpoints with their save data
    [SerializeField] private string myID;
    public string ID { get { return myID; } }
    // Cached component references
    private OrangeObject orangeObject;
    private GameManager gameManager;
    private Rigidbody2D rb2d;

    void Start()
    {
        orangeObject = GetComponent<OrangeObject>();
        rb2d = GetComponent<Rigidbody2D>();

        gameManager = FindObjectOfType<GameManager>();
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
        List<SerializableOrangeObjectData> allOrangeObjectData = gameData?.orangeObjectData;
        SerializableOrangeObjectData orangeObjectData = allOrangeObjectData?.Find(x => x.ID == ID);
        if(orangeObjectData == null)
            Debug.LogWarning($"[OrangeObjectData] OrangeObject data not found for cutscene {ID}", gameObject);
        else
        {
            orangeObject.SetBodyType(orangeObjectData.bodyType);
            orangeObject.transform.position = orangeObjectData.savedPosition; 
        }
    }

    // Returns a serializable version of the player's data
    public SerializableOrangeObjectData Capture()
    {
        Vector3 pos = transform.position;
        RigidbodyType2D type = rb2d.bodyType;
        return new SerializableOrangeObjectData()
        {
            savedPosition = pos,
            bodyType = type,
            ID = ID
        };
    }
    [ContextMenu("Regenerate GUID")]
    private void RegenerateGUID()
    {
        myID = System.Guid.NewGuid().ToString();
    }
}
