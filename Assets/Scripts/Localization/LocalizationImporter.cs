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

public class LocalizationImporter : MonoBehaviour
{
    [SerializeField] private bool importOnStart = true;
    [SerializeField] private LocaleOptions options;

    private void Start()
    {
        if (importOnStart)
            ImportLocale();
    }

    [ContextMenu("Import Locale")]
    private void ImportLocale()
    {
        // Initially make the locale unavailable
        SetCustomLocaleAvailability(false);

        string localeDir = GetLocaleDir();
        if (Directory.Exists(localeDir))
        {
            try
            {
                Debug.Log($"[LocalizationImporter] Starting import from {localeDir}");
                ImportMetadata();
                ImportStringTables();
                ImportDialogueObjects();
                Debug.Log($"[LocalizationImporter] Finished import from {localeDir}");

                // Make the locale available
                SetCustomLocaleAvailability(true);
            }
            catch (Exception e)
            {
                Debug.LogError($"[LocalizationImporter] [FATAL] Error importing translations: {e}");
            }
        }
        else
        {
            Debug.Log($"[LocalizationImporter] Directory not found: {localeDir}");
        }
    }

    public void ImportMetadata()
    {
        // Read the locale metadata file
        string localeMetadataPath = Path.Combine(GetLocaleDir(), options.metadataFilename);
        string localeMetadata = File.ReadAllText(localeMetadataPath);
        string[] splitLocaleMetadata = localeMetadata.Split("\n");
        options.locale.LocaleName = splitLocaleMetadata[0];
    }

    public void ImportStringTables()
    {
        string fullStringsDir = Path.Combine(GetLocaleDir(), options.stringsDir);
        foreach (FileToStringTableMapEntry entry in options.fileToTableMap)
        {
            string filePath = Path.Combine(fullStringsDir, entry.filename);
            LocalizationTableSerializerUtils.ImportJsonFromFile(filePath, entry.table);
            Debug.Log($"[LocalizationImporter] [SUCCESS] {filePath} --> {entry.table}");
        }
    }

    private void ImportDialogueObjects()
    {
        AssetTable table = options.dialogueTable;
        string fullDialogueDir = Path.Combine(GetLocaleDir(), options.dialogueDir);

        // Iterate through the json files in the directory
        DirectoryInfo dirInfo = new DirectoryInfo(fullDialogueDir);
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
                    Debug.Log($"[LocalizationImporter] [SUCCESS] {filePath} --> {table}.{tableKey}");
                }
                catch (Exception)
                {
                    Debug.LogError($"[LocalizationImporter] [FAIL] {filePath}: JSON parse error");
                }
            }
            // Else, report error
            else
            {
                Debug.LogError($"[LocalizationImporter] [FAIL] {filePath}: Bad file name");
            }
        }
    }

    // Add/remove the locale to/from the list of available locales
    private void SetCustomLocaleAvailability(bool value)
    {
        if (value)
        {
            // Show
            if (!LocalizationSettings.AvailableLocales.GetLocale(options.locale.Identifier))
            {
                LocalizationSettings.AvailableLocales.AddLocale(options.locale);
            }
            Debug.Log($"[LocalizationImporter] Enabled availability for locale {options.locale}");
        }
        else
        {
            // Hide
            LocalizationSettings.AvailableLocales.RemoveLocale(options.locale);
            Debug.Log($"[LocalizationImporter] Disabled availability for locale {options.locale}");
        }
    }

    private string GetLocaleDir()
    {
        return Path.Combine(Application.streamingAssetsPath, options.localeDir);
    }
}