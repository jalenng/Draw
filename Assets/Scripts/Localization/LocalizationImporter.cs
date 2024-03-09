using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Localization;
#endif

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization;
using UnityEngine.Localization.Tables;
using UnityEngine.Localization.Settings;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;

public class LocalizationImporter : MonoBehaviour
{
    [SerializeField] private bool importOnStart = true;
    [SerializeField] private LocaleOptions options;

    [SerializeField] private UnityEvent onCompletedStart;

    private void Start()
    {
        if (importOnStart)
        {
            StartCoroutine(ImportLocaleRoutine(true));
        }
    }

    [ContextMenu("Import Locale")]
    public void ImportLocale()
    {
        StartCoroutine(ImportLocaleRoutine(false));
    }

    public IEnumerator ImportLocaleRoutine(bool isFromStart)
    {
        string localeDir = GetLocaleDir();
        if (Directory.Exists(localeDir))
        {
            Debug.Log($"[LocalizationImporter] Starting import from {localeDir}");

            ImportMetadata();
            yield return ImportStringTables();
            ImportDialogueObjects();

            Debug.Log($"[LocalizationImporter] Finished import from {localeDir}");

            SetLocaleAvailability(true);
        }
        else
        {
            Debug.Log($"[LocalizationImporter] Main directory not found: {localeDir}");

            SetLocaleAvailability(false);
        }

        if (isFromStart)
        {
            onCompletedStart.Invoke();
        }
    }

    public void ImportMetadata()
    {
        // Read the locale metadata file
        string localeMetadataPath = Path.Combine(GetLocaleDir(), options.metadataFilename);
        if (!File.Exists(localeMetadataPath))
        {
            Debug.Log($"[LocalizationImporter] [FAIL] Metadata file not found: {localeMetadataPath}");
            return;
        }

        string localeMetadata = File.ReadAllText(localeMetadataPath);
        string[] splitLocaleMetadata = localeMetadata.Split("\n");

        Locale locale = options.locale;

        // If the game is running (not in the editor)...
        if (!Application.isEditor)
        {
            // We need to load the locale from the locale provider
            locale = LocalizationSettings.AvailableLocales.GetLocale(options.locale.Identifier);
        }

        // Update the locale name
        locale.LocaleName = splitLocaleMetadata[0];
    }

    public IEnumerator ImportStringTables()
    {
        string fullStringsDir = Path.Combine(GetLocaleDir(), options.stringsDir);
        if (!Directory.Exists(fullStringsDir))
        {
            Debug.Log($"[LocalizationImporter] [FAIL] Strings directory not found: {fullStringsDir}");
            yield return null;
        }

        foreach (FileToStringTableMapEntry entry in options.fileToTableMap)
        {
            StringTable table = entry.table;

            // If the game is running (not in the editor)...
            if (!Application.isEditor)
            {
                // We need to load the table from the DB by name and locale
                // instead of directly from the option entires
                LocalizedStringDatabase stringDb = LocalizationSettings.StringDatabase;
                Locale locale = LocalizationSettings.AvailableLocales.GetLocale(table.LocaleIdentifier);
                AsyncOperationHandle<StringTable> asyncOp = stringDb.GetTableAsync(table.TableCollectionName, locale);
                yield return asyncOp;

                table = asyncOp.Result;

                // Prevent the table from being unloaded
                Addressables.ResourceManager.Acquire(asyncOp);
            }

            // Import the contents from the JSON to the table
            string filePath = Path.Combine(fullStringsDir, entry.filename);
            LocalizationTableSerializerUtils.ImportJsonFromFile(filePath, table);
            Debug.Log($"[LocalizationImporter] [SUCCESS] {filePath} --> {table}");
        }
    }

    private void ImportDialogueObjects()
    {
        AssetTable table = options.dialogueTable;
        string fullDialogueDir = Path.Combine(GetLocaleDir(), options.dialogueDir);
        if (!Directory.Exists(fullDialogueDir))
        {
            Debug.Log($"[LocalizationImporter] [FAIL] Dialogue objects directory not found: {fullDialogueDir}");
            return;
        };

        // Iterate through the json files in the directory
        DirectoryInfo dirInfo = new DirectoryInfo(fullDialogueDir);
        foreach (FileInfo fileInfo in dirInfo.EnumerateFiles("*.json"))
        {
            // Get the table entry
            string tableKey = Path.GetFileNameWithoutExtension(fileInfo.Name);
            string filePath = fileInfo.FullName;
            string jsonContent = File.ReadAllText(filePath);

            // Try to get the dialogue object from the table entry
            AsyncOperationHandle<Dialogue> asyncOp = table.GetAssetAsync<Dialogue>(tableKey);
            Dialogue dialogueObj = asyncOp.WaitForCompletion();

#if UNITY_EDITOR
            // If not found and we're running in the editor,
            // create one and add it to the table.
            // This asset will be persisted.
            if (dialogueObj == null && Application.isEditor)
            {
                // Create the object
                dialogueObj = ScriptableObject.CreateInstance<Dialogue>();

                // Create the directory for the asset file
                Locale locale = options.locale;
                string assetParentDir = Path.Combine("Assets", "Localization", "Translated Dialogues", $"{locale.Identifier.Code}");
                if (!Directory.Exists(assetParentDir))
                {
                    Directory.CreateDirectory(assetParentDir);
                    AssetDatabase.Refresh();
                    Debug.Log($"[LocalizationImporter] Created directory {assetParentDir}");
                }

                // Create the asset file
                string assetPath = Path.Combine(assetParentDir, $"{tableKey}.asset");
                AssetDatabase.CreateAsset(dialogueObj, assetPath);
                Debug.Log($"[LocalizationImporter] Created asset file in {assetPath}");

                // Add the asset to the table collection
                AssetTableCollection tableCollection = LocalizationEditorSettings.GetAssetTableCollection(table.TableCollectionName);
                tableCollection.AddAssetToTable(table, tableKey, dialogueObj);
            }
#endif

            // If we successfully found or created a dialogue object...
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
    private void SetLocaleAvailability(bool value)
    {
        ILocalesProvider availableLocales = LocalizationSettings.AvailableLocales;

        if (value)
        {
            // Show
            if (!availableLocales.GetLocale(options.locale.Identifier))
            {
                availableLocales.AddLocale(options.locale);
            }
            Debug.Log($"[LocalizationImporter] Enabled availability for locale {options.locale}");
        }
        else
        {
            // Switch locale if it is currently selected
            if (LocalizationSettings.SelectedLocale == options.locale)
            {
                LocalizationSettings.SelectedLocale = availableLocales.Locales[0];
            }

            // Hide
            availableLocales.RemoveLocale(options.locale);
            Debug.Log($"[LocalizationImporter] Disabled availability for locale {options.locale}");
        }
    }

    [ContextMenu("Clear String Tables")]
    private void ClearStringTables()
    {
        foreach (FileToStringTableMapEntry entry in options.fileToTableMap)
        {
            entry.table.Clear();
        }
    }

    private string GetLocaleDir()
    {
        return Path.Combine(Application.streamingAssetsPath, options.localeDir);
    }
}