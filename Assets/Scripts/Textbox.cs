using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Textbox : MonoBehaviour
{

    // Configuration parameters    
    [SerializeField] Image avatarImage;
    [SerializeField] TextMeshProUGUI dialogueContentTMP;
    [Range(1, 120)] [SerializeField] int charactersPerSecond = 30;

    // State variables
    [Header("For debugging purposes only")]
    [TextArea(5, 5)] 
    [SerializeField] string targetText;

    bool isSpeaking = false;

    // Start is called before the first frame update
    void Start()
    {   
        StartCoroutine(Say());
    }

    // Updates the target text, then updates the textbox
    IEnumerator Say(string targetText)
    {
        this.targetText = targetText;
        yield return Say();
    }

    // Updates the textbox with the target text
    IEnumerator Say()
    {
        // Update flag
        isSpeaking = true;

        // Get the number of characters
        int totalCharacters = targetText.Length;

        // At every interval, update the textbox text to achieve a typewriter effect
        for (int i = 0; i < totalCharacters; i++)
        {
            dialogueContentTMP.text = targetText.Substring(0, i);
            yield return new WaitForSeconds(1.0f / charactersPerSecond);
        }

        // Update flag
        isSpeaking = false;
    }

}