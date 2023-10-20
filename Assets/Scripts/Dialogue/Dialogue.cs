
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueEntry
{
    public Sprite avatar;

    // Content-related
    [TextArea(5, 5)]
    public string content;

    [Tooltip("Check this box to use the CPS below. Otherwise, will use the default.")]
    public bool useCPS;

    [Range(0, 120)]
    [Tooltip("Characters to reveal per second. Set this to 0 to disable the typewriting effect")]
    public int CPS = 30;

}

[CreateAssetMenu(fileName = "Dialogue", menuName = "Dialogue", order = 1)]
public class Dialogue : ScriptableObject
{
    // Default parameters
    [Header("Defaults")]
    [Range(0, 120)]
    [Tooltip("Set to 0 to disable typewriting effect")]
    public int CPS = 30;
    public string SFXDirectory = "";

    [Space(16)]

    // Dialogue
    public List<DialogueEntry> entries = new List<DialogueEntry>();
}
