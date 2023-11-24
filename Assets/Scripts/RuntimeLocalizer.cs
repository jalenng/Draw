using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization;
using UnityEngine.Localization.Tables;
using UnityEngine.Localization.Settings;
using System.IO;

public class RuntimeLocalizer : MonoBehaviour
{
    [SerializeField] private Locale customLocale;

    private void Start()
    {
        InitCustomLocale();
    }

    [ContextMenu("Initialize Custom Locale")]
    private void InitCustomLocale()
    {
        string localeDir = Path.Combine(Application.streamingAssetsPath, "Localization", "custom");
        if (Directory.Exists(localeDir))
        {
            Debug.Log($"[RuntimeLocalizer] Importing custom translations from {localeDir}");

            // Read the locale metadata file
            string localeMetadataPath = Path.Combine(localeDir, "index.txt");
            string localeMetadata = File.ReadAllText(localeMetadataPath);
            string[] splitLocaleMetadata = localeMetadata.Split("\n");
            string localeName = splitLocaleMetadata[0];

            // Set up string tables
            string stringsDir = Path.Combine(localeDir, "strings");
            string csvColumnHeader = customLocale.Identifier.ToString();

            DetailedLocalizationTable<StringTableEntry> uiTable = LocalizationSettings.StringDatabase.GetTable("UI", customLocale);
            string uiCsvPath = Path.Combine(stringsDir, "UI.csv");
            Dictionary<string, string> uiMap = GetDictFromCsv(uiCsvPath, "Key", csvColumnHeader);
            AddEntriesToTable<StringTableEntry>(uiTable, uiMap);

            DetailedLocalizationTable<StringTableEntry> creditsTable = LocalizationSettings.StringDatabase.GetTable("Credits", customLocale);
            string creditsCsvPath = Path.Combine(stringsDir, "Credits.csv");
            Dictionary<string, string> creditsMap = GetDictFromCsv(creditsCsvPath, "Key", csvColumnHeader);
            AddEntriesToTable<StringTableEntry>(creditsTable, creditsMap);

            // Set up dialogue asset table
            string dialogueDir = Path.Combine(localeDir, "dialogue");
            DetailedLocalizationTable<AssetTableEntry> dialogueTable = LocalizationSettings.AssetDatabase.GetTable("Dialogue", customLocale);
            Dictionary<string, string> dialogueGUIDMap = CreateDictFromDialogues(dialogueDir);
            AddEntriesToTable<AssetTableEntry>(dialogueTable, dialogueGUIDMap);

            // Done
            Debug.Log($"[RuntimeLocalizer] Custom translations successfully imported"); SetCustomLocaleAvailability(true);

        }
        // Else, do not make the custom locale option available
        else
        {
            Debug.Log($"[RuntimeLocalizer] No custom translations found in {localeDir}"); SetCustomLocaleAvailability(false);
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

    private Dictionary<string, string> GetDictFromCsv(string filePath, string keyHeader, string valueHeader)
    {
        Dictionary<string, string> dict = new Dictionary<string, string>();

        List<string[]> parsedCsv = CSVParser.ParseCsv(filePath);

        // Get column indices based on first (header) row
        string[] firstLine = parsedCsv[0];
        int keyColumnIndex = Array.FindIndex(firstLine, (item) => item == keyHeader);
        int valueColumnIndex = Array.FindIndex(firstLine, (item) => item == valueHeader);
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

    private Dictionary<string, string> CreateDictFromDialogues(string filePath)
    {
        Dictionary<string, string> dict = new Dictionary<string, string>();
        // TODO
        return dict;
    }


    private void AddEntriesToTable<T>(DetailedLocalizationTable<T> table, Dictionary<string, string> map) where T : TableEntry
    {
        foreach (KeyValuePair<string, string> pair in map)
        {
            table.AddEntry(pair.Key, pair.Value);
        }
    }
}
