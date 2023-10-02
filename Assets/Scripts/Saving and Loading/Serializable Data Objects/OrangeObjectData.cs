using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// This is what is being serialized
[System.Serializable]
public class SerializableOrangeObjectData
{
    public float[] position = new float[3];
    public float rotation;
    public float[] respawnPos = new float[3];
    public float respawnRot;
    public bool isDynamic;
    public string ID;
}

// OrangeObjectData has two responsibilities:
// 1. Upon scene load, it attempts to load the orange object data from the GameManager
// 2. It captures the orange object data for the GameManager to serialize
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
        gameManager.orangeObjectData.Add(this);
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

        // Ensure orange object data exists 
        List<SerializableOrangeObjectData> allOrangeObjectData = gameData?.orangeObjectData;
        SerializableOrangeObjectData orangeObjectData = allOrangeObjectData?.Find(x => x.ID == ID);
        if (orangeObjectData == null)
            Debug.LogWarning($"[OrangeObjectData] OrangeObject data not found for object {ID}", gameObject);
        else
        {
            // Set current pos and rot
            transform.position = new Vector3(
                orangeObjectData.position[0],
                orangeObjectData.position[1],
                orangeObjectData.position[2]
            );
            transform.eulerAngles = new Vector3(0, 0, orangeObjectData.rotation);

            // Set respawn pos and rot
            orangeObject.respawnPos = new Vector3(
                orangeObjectData.respawnPos[0],
                orangeObjectData.respawnPos[1],
                orangeObjectData.respawnPos[2]
            );
            orangeObject.respawnRot = orangeObjectData.respawnRot;

            // Set body type
            if (orangeObjectData.isDynamic)
                rb2d.bodyType = RigidbodyType2D.Dynamic;
        }
    }

    // Returns a serializable version of the orange object's data
    public SerializableOrangeObjectData Capture()
    {
        Vector3 currentPos = transform.position;
        Vector3 respawnPos = orangeObject.respawnPos;
        RigidbodyType2D type = rb2d.bodyType;
        return new SerializableOrangeObjectData()
        {
            // Get current pos and rot
            position = new float[] {
                currentPos.x,
                currentPos.y,
                currentPos.z
            },
            rotation = transform.eulerAngles.z,

            // Get respawn pos and rot
            respawnPos = new float[] {
                respawnPos.x,
                respawnPos.y,
                respawnPos.z,
            },
            respawnRot = orangeObject.respawnRot,

            // Get body type
            isDynamic = rb2d.bodyType == RigidbodyType2D.Dynamic,
            ID = ID
        };
    }
    [ContextMenu("Regenerate GUID")]
    private void RegenerateGUID()
    {
        myID = System.Guid.NewGuid().ToString();
    }
}
