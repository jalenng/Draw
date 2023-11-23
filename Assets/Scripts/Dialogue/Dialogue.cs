
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text;

[System.Serializable]
public class DialogueEntry
{
    public Sprite avatar;

    // Content-related
    [TextArea(5, 5)]
    public string content;
}

[CreateAssetMenu(fileName = "Dialogue", menuName = "Dialogue", order = 1)]
public class Dialogue : ScriptableObject
{

    [ContextMenu("Import JSON")]
    void ImportJSON()
    {
        string path = EditorUtility.OpenFilePanel("Import", "", "json");
        if (path.Length != 0)
        {
            string fileContent = File.ReadAllText(path);
            JsonUtility.FromJsonOverwrite(fileContent, this);
        }
    }

    [ContextMenu("Export JSON")]
    void ExportJSON()
    {
        string path = EditorUtility.SaveFilePanel("Export", "", name, "json");
        using (FileStream fs = File.Create(path))
        {
            string dataString = JsonUtility.ToJson(this, true);
            byte[] info = new UTF8Encoding(true).GetBytes(dataString);
            fs.Write(info, 0, info.Length);
        }
    }

    // Default parameters
    [Range(0, 120)]
    [Tooltip("Set to 0 to disable typewriting effect")]
    public int CPS = 30;
    public string SFXDirectory = "";

    [Space(16)]

    // Dialogue
    public List<DialogueEntry> entries = new List<DialogueEntry>();
}
