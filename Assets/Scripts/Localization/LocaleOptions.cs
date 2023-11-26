using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Tables;
using UnityEngine.Localization.Settings;

[System.Serializable]
public class FileToStringTableMapEntry
{
    public string filename;
    public StringTable table;
}

[CreateAssetMenu(fileName = "LocaleOptions", menuName = "Locale Options", order = 2)]
public class LocaleOptions : ScriptableObject
{
    [Header("Locale")]
    public Locale locale;
    public string localeDir = "";
    public string metadataFilename = "index.txt";

    [Header("Strings")]
    public string stringsDir = "strings";
    public List<FileToStringTableMapEntry> fileToTableMap = new List<FileToStringTableMapEntry>() {
        new FileToStringTableMapEntry() {
            filename = "ui.json",
            table = null
        },
        new FileToStringTableMapEntry() {
            filename = "credits.json",
            table = null
        }
    };

    [Header("Dialogue")]
    public string dialogueDir = "dialogue";
    public AssetTable dialogueTable;
}
