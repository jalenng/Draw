using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Localization.Tables;
using UnityEngine.ResourceManagement.AsyncOperations;

public class LocalizationExporter : MonoBehaviour
{
    [SerializeField] private LocaleOptions options;

    [ContextMenu("Export Locale")]
    public void ExportLocale()
    {
        Debug.Log($"[LocalizationExporter] Starting export for {options.locale}");

        ExportMetadata();
        ExportStringTables();
        ExportDialogueObjects();

        Debug.Log($"[LocalizationExporter] Finished export of {options.locale}");
    }

    public void ExportMetadata()
    {
        // Create directory if DNE
        Directory.CreateDirectory(GetLocaleDir());

        // Write metadata file
        string metadataFilePath = Path.Combine(GetLocaleDir(), options.metadataFilename);
        using (FileStream fs = File.Create(metadataFilePath))
        {
            string localeName = options.locale.LocaleName;
            byte[] info = new UTF8Encoding(true).GetBytes(localeName);
            fs.Write(info, 0, info.Length);
            Debug.Log($"[LocalizationExporter] [SUCCESS] \"{localeName}\" --> {metadataFilePath}");
        }
    }

    public void ExportStringTables()
    {
        // Create directory if DNE
        string fullStringsDir = Path.Combine(GetLocaleDir(), options.stringsDir);
        Directory.CreateDirectory(fullStringsDir);

        foreach (FileToStringTableMapEntry entry in options.fileToTableMap)
        {
            // Get and export string table
            string filePath = Path.Combine(fullStringsDir, entry.filename);
            LocalizationTableSerializerUtils.ExportJsonToFile(entry.table, filePath);
            Debug.Log($"[LocalizationExporter] [SUCCESS] {entry.table} --> {filePath}");
        }
    }

    public void ExportDialogueObjects()
    {
        // Create directory if DNE
        string fullDialogueDir = Path.Combine(GetLocaleDir(), options.dialogueDir);
        Directory.CreateDirectory(fullDialogueDir);

        // Get dialogue table and iterate through it
        AssetTable table = options.dialogueTable;
        foreach (KeyValuePair<long, AssetTableEntry> kvp in table)
        {
            // Get dialogue object
            AssetTableEntry entry = kvp.Value;
            string tableKey = entry.Key;
            AsyncOperationHandle<Dialogue> asyncOperation = table.GetAssetAsync<Dialogue>(tableKey);
            Dialogue dialogueObj = asyncOperation.WaitForCompletion();

            // Export dialogue object
            string exportFilePath = Path.Combine(fullDialogueDir, $"{tableKey}.json");
            dialogueObj.ExportJsonToFile(exportFilePath);
            Debug.Log($"[LocalizationExporter] [SUCCESS] {options.dialogueTable}.{entry.Key} --> {exportFilePath}");
        }
    }

    private string GetLocaleDir()
    {
        return Path.Combine(Application.streamingAssetsPath, options.localeDir);
    }
}
