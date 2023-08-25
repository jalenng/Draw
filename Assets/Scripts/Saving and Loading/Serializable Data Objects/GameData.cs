using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The GameData class aggregates the data of the player and the checkpoints.

// It gets serialized by the GameManager which uses
// the ObjectSerializer.

[System.Serializable]
public class GameData {
    public Global.Level level;
    public SerializablePlayerData playerData;
    public List<SerializableCheckpointData> checkpointData = new List<SerializableCheckpointData>();
    public List<SerializableCutsceneData> cutsceneData = new List<SerializableCutsceneData>();
}