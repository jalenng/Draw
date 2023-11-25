
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
    private void OpenInportJson()
    {

        string path = EditorUtility.OpenFilePanel("Import", "", "json");
        if (path.Length != 0)
        {
            string fileContent = File.ReadAllText(path);
            ImportJson(fileContent);
        }
    }

    [ContextMenu("Export JSON")]
    private void OpenExportJson()
    {
        string path = EditorUtility.SaveFilePanel("Export", "", name, "json");
        ExportJsonToFile(path);
    }

    [ContextMenu("Export JSON to Assets > ..")]
    private void ExportJsonToAssets()
    {
        string path = Path.Combine(Application.dataPath, "..", $"{name}.json");
        ExportJsonToFile(path);
    }

    private void ExportJsonToFile(string path) {
        using (FileStream fs = File.Create(path))
        {
            string dataString = ExportJson();
            byte[] info = new UTF8Encoding(true).GetBytes(dataString);
            fs.Write(info, 0, info.Length);
        }
    }
#endif

    public void ImportJson(string data)
    {
        JsonUtility.FromJsonOverwrite(data, this);
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
