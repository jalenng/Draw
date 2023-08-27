using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

using UnityEngine;

public class ObjectSerializer<T>
{
    BinaryFormatter bf;

    public ObjectSerializer()
    {
        bf = new BinaryFormatter();
    }

    // Serialize the object and write it to disk
    public void Write(T obj, string path)
    {
        FileStream file = File.Create(path);
        bf.Serialize(file, obj);
        file.Close();
    }

    // Read the object from disk and deserialize it
    public T Read(string path)
    {
        // Check if the file exists first
        if (!CanRead(path)) 
        {
            Debug.Log($"Tried to load {path} but the file does not exist");
            return default(T);
        };
        
        FileStream file = File.Open(path, FileMode.Open);
        T obj = (T)bf.Deserialize(file);
        file.Close();

        return obj;
    }

    // Check if the file exists
    public bool CanRead(string path)
    {
        return File.Exists(path);
    }
}
