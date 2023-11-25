using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization;
using UnityEngine.Localization.Tables;
using UnityEngine.Localization.Settings;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.IO;

[Serializable]
public class CsvToStringTableMapEntry
{
    public string filename;
    public StringTable table;
}

public class CustomLocalizationImporter : MonoBehaviour
{
    [SerializeField] private string customLocaleDir = "CustomLocale";

    [Header("Custom Locale")]
    [SerializeField] private Locale customLocale;
    [SerializeField] private string metadataFilename = "index.txt";

    [Header("Strings")]
    [SerializeField] private string stringsDir = "strings";
    [SerializeField] private string keyHeader = "Key";
    [SerializeField] private string valueHeader = "Value";
    [SerializeField] private List<CsvToStringTableMapEntry> csvToTableMap;

    [Header("Dialogue")]
    [SerializeField] private string dialogueDir = "dialogue";
    [SerializeField] private AssetTable dialogueTable;

    private void Start()
    {
        InitCustomLocale();
    }

    [ContextMenu("Initialize Custom Locale")]
    private void InitCustomLocale()
    {
        string localeDir = Path.Combine(Application.streamingAssetsPath, customLocaleDir);
        if (Directory.Exists(localeDir))
        {
            try
            {
                Debug.Log($"[CustomLocalizationImporter] Importing custom translations from {localeDir}");

                // Read the locale metadata file
                string localeMetadataPath = Path.Combine(localeDir, metadataFilename);
                string localeMetadata = File.ReadAllText(localeMetadataPath);
                string[] splitLocaleMetadata = localeMetadata.Split("\n");
                customLocale.LocaleName = splitLocaleMetadata[0];

                // Set up string tables
                string fullStringsDir = Path.Combine(localeDir, stringsDir);
                foreach (CsvToStringTableMapEntry entry in csvToTableMap)
                {
                    string csvPath = Path.Combine(fullStringsDir, entry.filename);
                    var uiMap = GetDictFromCsv(csvPath, keyHeader, valueHeader);
                    SetStringTableToMap(entry.table, uiMap);
                    Debug.Log($"[CustomLocalizationImporter] Updated string entries from {csvPath} to {entry.table}");
                }

                // Set up dialogue asset table
                string fullDialogueDir = Path.Combine(localeDir, dialogueDir);
                UpdateTableDialogues(dialogueTable, fullDialogueDir);

                // Done
                Debug.Log($"[CustomLocalizationImporter] Custom translations successfully imported");
                SetCustomLocaleAvailability(true);
            }
            catch (Exception e)
            {
                Debug.LogError($"[CustomLocalizationImporter] Failed to import custom translations: {e}");
                SetCustomLocaleAvailability(false);
            }
        }
        // Else, do not make the custom locale option available
        else
        {
            Debug.Log($"[CustomLocalizationImporter] No custom translations found in {localeDir}");
            SetCustomLocaleAvailability(false);
        }
    }

    // Add/remove the custom locale to/from the list of available locales
    private void SetCustomLocaleAvailability(bool value)
    {
        if (value)
        {
            // Show
            if (!LocalizationSettings.AvailableLocales.GetLocale(customLocale.Identifier))
                LocalizationSettings.AvailableLocales.AddLocale(customLocale);
        }
        else
            // Hide
            LocalizationSettings.AvailableLocales.RemoveLocale(customLocale);
    }

    // Parse a CSV and return a dictionary mapping two specified columns
    private Dictionary<string, string> GetDictFromCsv(string filePath, string keyHeader, string valueHeader)
    {
        Dictionary<string, string> dict = new Dictionary<string, string>();

        List<string[]> parsedCsv = CSVParser.ParseCsv(filePath);

        // Get column indices based on first (header) row
        string[] firstLine = parsedCsv[0];

        int keyColumnIndex = Array.FindIndex(firstLine, (item) => item == keyHeader);
        if (keyColumnIndex < 0)
            throw new Exception($"Could not find column \"{keyHeader}\" in {filePath}");

        int valueColumnIndex = Array.FindIndex(firstLine, (item) => item == valueHeader);
        if (valueColumnIndex < 0)
            throw new Exception($"Could not find column \"{valueHeader}\" in {filePath}");

        int minRowLength = Math.Max(keyColumnIndex, valueColumnIndex) + 1;
        for (int i = 1; i < parsedCsv.Count; i++)
        {
            string[] line = parsedCsv[i];
            // Skip row/line if not enough columns
            if (line.Length < minRowLength) continue;
            dict.Add(line[keyColumnIndex], line[valueColumnIndex]);
        }

        return dict;
    }

    // Set the key-value pairs of a StringTable to match a dictionary
    private void SetStringTableToMap(StringTable table, Dictionary<string, string> map)
    {
        table.Clear();
        foreach (KeyValuePair<string, string> pair in map)
            table.AddEntry(pair.Key, pair.Value);
    }

    // Update the dialogue objects in the dialogue table
    private void UpdateTableDialogues(AssetTable table, string filePath)
    {
        DirectoryInfo dirInfo = new DirectoryInfo(filePath);
        foreach (FileInfo fileInfo in dirInfo.EnumerateFiles("*.json"))
        {
            // Get the table entry
            string tableKey = Path.GetFileNameWithoutExtension(fileInfo.Name);
            string fullFilePath = fileInfo.FullName;
            string jsonContent = File.ReadAllText(fullFilePath);

            // Get the dialogue object from the table entry...
            AsyncOperationHandle<Dialogue> asyncOp = table.GetAssetAsync<Dialogue>(tableKey);
            asyncOp.Completed += delegate (AsyncOperationHandle<Dialogue> opResult)
            {
                // ...then import the translations from the JSON
                Dialogue dialogue = opResult.Result;
                dialogue.ImportJSON(jsonContent);
            };

            Debug.Log($"[CustomLocalizationImporter] Updated dialogue entry from {fullFilePath} to {table}.{tableKey}");
        };
    }
}
