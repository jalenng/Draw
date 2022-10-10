using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

using UnityEngine;

public class GameSerializer
{
    BinaryFormatter bf;

    public GameSerializer()
    {
        bf = new BinaryFormatter();
    }

    public void Save(GameData data, string path)
    {
        // Write to file
        FileStream file = File.Create(path);
        bf.Serialize(file, data);
        file.Close();
    }

    public GameData Load(string path)
    {
        // Check if the save file exists
        if (!File.Exists(path)) 
        {
            Debug.Log("No save file found");
            return null;
        };
        
        // Read from file
        FileStream file = File.Open(path, FileMode.Open);
        GameData data = (GameData)bf.Deserialize(file);
        file.Close();

        return data;
    }
}
