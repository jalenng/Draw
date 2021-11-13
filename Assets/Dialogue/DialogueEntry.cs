using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "DialogueEntry", menuName = "Dialogue/Dialogue Entry", order = 1)]
public class DialogueEntry : ScriptableObject
{
    public Image avatar;
    [TextArea(5, 5)] [SerializeField] public string content;

    [Range(0, 120)] [Tooltip("Set to 0 to disable typewriting effect")]
    [SerializeField] 
    public int charactersPerSecond = 30;

}
