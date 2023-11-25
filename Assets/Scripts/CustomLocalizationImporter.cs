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
                Debug.Log($"[CustomLocalizationImporter] Starting custom translations import from {localeDir}");

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
                    UpdateStringTable(entry.table, csvPath, keyHeader, valueHeader);
                }

                // Set up dialogue asset table
                string fullDialogueDir = Path.Combine(localeDir, dialogueDir);
                UpdateTableDialogues(dialogueTable, fullDialogueDir);

                // Done
                Debug.Log($"[CustomLocalizationImporter] Custom translations finished imported");
                SetCustomLocaleAvailability(true);
            }
            catch (Exception e)
            {
                Debug.LogError($"[CustomLocalizationImporter] [FATAL] Error importing custom translations: {e}");
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
            {
                LocalizationSettings.AvailableLocales.AddLocale(customLocale);
            }
            Debug.Log($"[CustomLocalizationImporter] Enabled availability for custom locale {customLocale}");
        }
        else
        {
            // Hide
            LocalizationSettings.AvailableLocales.RemoveLocale(customLocale);
            Debug.Log($"[CustomLocalizationImporter] Disabled availability for custom locale {customLocale}");
        }
    }

    // Parse a CSV and return a dictionary mapping two specified columns
    private void UpdateStringTable(StringTable table, string filePath, string keyHeader, string valueHeader)
    {
        Dictionary<string, string> dict = new Dictionary<string, string>();

        List<string[]> parsedCsv = CSVParser.ParseCsv(filePath);

        // Get column indices based on first (header) row
        string[] firstLine = parsedCsv[0];

        int keyColumnIndex = Array.FindIndex(firstLine, (item) => item == keyHeader);
        int valueColumnIndex = Array.FindIndex(firstLine, (item) => item == valueHeader);

        if (keyColumnIndex < 0)
            Debug.LogError($"[CustomLocalizationImporter] [FAIL] {filePath}: Could not find column \"{keyHeader}\"");
        else if (valueColumnIndex < 0)
            Debug.LogError($"[CustomLocalizationImporter] [FAIL] {filePath}: Could not find column \"{valueHeader}\"");
        else
        {
            int minRowLength = Math.Max(keyColumnIndex, valueColumnIndex) + 1;
            for (int i = 1; i < parsedCsv.Count; i++)
            {
                string[] line = parsedCsv[i];
                // Skip row/line if not enough columns
                if (line.Length < minRowLength) continue;
                dict.Add(line[keyColumnIndex], line[valueColumnIndex]);
            }
        }

        // Set the key-value pairs of the StringTable to match the dictionary
        table.Clear();
        foreach (KeyValuePair<string, string> pair in dict)
            table.AddEntry(pair.Key, pair.Value);

        Debug.Log($"[CustomLocalizationImporter] [SUCCESS] {filePath} --> {table}");
    }

    // Update the dialogue objects in the dialogue table
    private void UpdateTableDialogues(AssetTable table, string dir)
    {
        DirectoryInfo dirInfo = new DirectoryInfo(dir);
        foreach (FileInfo fileInfo in dirInfo.EnumerateFiles("*.json"))
        {
            // Get the table entry
            string tableKey = Path.GetFileNameWithoutExtension(fileInfo.Name);
            string filePath = fileInfo.FullName;
            string jsonContent = File.ReadAllText(filePath);

            // Try to get the dialogue object from the table entry
            AsyncOperationHandle<Dialogue> asyncOperation = table.GetAssetAsync<Dialogue>(tableKey);
            Dialogue dialogueObj = asyncOperation.WaitForCompletion();

            // If found, import the translations from the JSON
            if (dialogueObj != null)
            {
                // Handle JSON parse error
                try
                {
                    dialogueObj.ImportJson(jsonContent);
                    Debug.Log($"[CustomLocalizationImporter] [SUCCESS] {filePath} --> {table}.{tableKey}");
                }
                catch (Exception)
                {
                    Debug.LogError($"[CustomLocalizationImporter] [FAIL] {filePath}: JSON parse error");
                }
            }
            // Else, report error
            else
            {
                Debug.LogError($"[CustomLocalizationImporter] [FAIL] {filePath}: Bad file name");
            }
        }
    }
}