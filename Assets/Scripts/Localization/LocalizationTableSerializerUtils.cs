using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Tables;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.IO;

[Serializable]
public class SerializableStringTable
{
    public List<SerializableStringTableEntry> entries = new List<SerializableStringTableEntry>();
}

[Serializable]
public class SerializableStringTableEntry
{
    public string key;
    public string value;
}

public class LocalizationTableSerializerUtils
{
    public static void ImportJson(string data, StringTable table)
    {
        SerializableStringTable serializedTable = new SerializableStringTable();
        JsonUtility.FromJsonOverwrite(data, serializedTable);

        foreach (SerializableStringTableEntry serializedEntry in serializedTable.entries)
        {
            table.AddEntry(serializedEntry.key, serializedEntry.value);
        }
    }

    public static void ImportJsonFromFile(string filePath, StringTable table)
    {
        string fileContent = File.ReadAllText(filePath);
        ImportJson(fileContent, table);
    }

    public static string ExportJson(StringTable table)
    {
        SerializableStringTable serializedTable = new SerializableStringTable();
        foreach (KeyValuePair<long, StringTableEntry> kvp in table)
        {
            StringTableEntry entry = kvp.Value;
            serializedTable.entries.Add(
                new SerializableStringTableEntry()
                {
                    key = entry.Key,
                    value = entry.Value
                }
            );
        }
        return JsonUtility.ToJson(serializedTable, true);
    }

    public static void ExportJsonToFile(StringTable table, string filePath)
    {
        using (FileStream fs = File.Create(filePath))
        {
            string dataString = ExportJson(table);
            byte[] info = new UTF8Encoding(true).GetBytes(dataString);
            fs.Write(info, 0, info.Length);
        }
    }
}