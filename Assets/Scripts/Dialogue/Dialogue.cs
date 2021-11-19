using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dialogue", menuName = "Dialogue", order = 1)]
public class Dialogue : ScriptableObject
{
    // Default parameters
    [Header("Defaults")]
    [Range(0, 120)] [Tooltip("Set to 0 to disable typewriting effect")] 
    public int CPS = 30;

    [Space(16)]

    // Dialogue
    public List<DialogueEntry> entries = new List<DialogueEntry>();
}
