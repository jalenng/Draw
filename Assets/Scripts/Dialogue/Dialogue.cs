
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

#if UNITY_EDITOR
using UnityEditor;
#endif

[System.Serializable]
public class DialogueEntry
{
    public string avatarName;

    // Content-related
    [TextArea(5, 5)]
    public string content;
}

[CreateAssetMenu(fileName = "Dialogue", menuName = "Dialogue", order = 1)]
public class Dialogue : ScriptableObject
{

#if UNITY_EDITOR
    [ContextMenu("Import JSON")]
    private void OpenImportJson()
    {
        string filePath = EditorUtility.OpenFilePanel("Import", "", "json");
        if (filePath.Length != 0)
        {
            string fileContent = File.ReadAllText(filePath);
            ImportJson(fileContent);
        }
    }

    [ContextMenu("Export JSON")]
    private void OpenExportJson()
    {
        string filePath = EditorUtility.SaveFilePanel("Export", "", name, "json");
        ExportJsonToFile(filePath);
    }
#endif

    public void ImportJsonFromFile(string filePath)
    {
        string fileContent = File.ReadAllText(filePath);
        ImportJson(fileContent);
    }

    public void ExportJsonToFile(string filePath)
    {
        using (FileStream fs = File.Create(filePath))
        {
            string dataString = ExportJson();
            byte[] info = new UTF8Encoding(true).GetBytes(dataString);
            fs.Write(info, 0, info.Length);
        }
    }

    public void ImportJson(string data)
    {
        JsonUtility.FromJsonOverwrite(data, this);
#if UNITY_EDITOR
        EditorUtility.SetDirty(this);
#endif
    }

    public string ExportJson()
    {
        return JsonUtility.ToJson(this, true);
    }

    [Range(0, 120)]
    [Tooltip("Set to 0 to disable typewriting effect")]
    public int CPS = 30;
    public string SFXDirectory = "";

    [Space(16)]

    public List<DialogueEntry> entries = new List<DialogueEntry>();
}
