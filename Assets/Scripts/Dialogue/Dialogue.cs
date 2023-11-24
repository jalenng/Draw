
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
    public Sprite avatar;

    // Content-related
    [TextArea(5, 5)]
    public string content;
}

[CreateAssetMenu(fileName = "Dialogue", menuName = "Dialogue", order = 1)]
public class Dialogue : ScriptableObject
{

#if UNITY_EDITOR
    [ContextMenu("Import JSON")]
    private void OpenInportJSON()
    {

        string path = EditorUtility.OpenFilePanel("Import", "", "json");
        if (path.Length != 0)
        {
            string fileContent = File.ReadAllText(path);
            ImportJSON(fileContent);
        }
    }

    [ContextMenu("Export JSON")]
    private void OpenExportJSON()
    {
        string path = EditorUtility.SaveFilePanel("Export", "", name, "json");
        using (FileStream fs = File.Create(path))
        {
            string dataString = ExportJSON();
            byte[] info = new UTF8Encoding(true).GetBytes(dataString);
            fs.Write(info, 0, info.Length);
        }
    }
#endif

    public void ImportJSON(string data)
    {
        JsonUtility.FromJsonOverwrite(data, this);
    }

    public string ExportJSON()
    {
        return JsonUtility.ToJson(this, true);
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
