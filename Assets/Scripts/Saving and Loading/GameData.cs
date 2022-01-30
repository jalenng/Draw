using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The GameData class aggregates the data of the player and the checkpoints.

// It gets serialized by the GameManager which uses
// the GameSerializer.

[System.Serializable]
public class GameData {
    public int sceneIndex;
    public SerializablePlayerData playerData;
    // public List<SerializableCheckpointData> checkpointData;

}