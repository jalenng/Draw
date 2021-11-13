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
    [Range(0, 120)] [Tooltip("Set to 0 to disable typewriting effect")]
    [SerializeField] int charactersPerSecond = 30;

    [SerializeField] DialogueEntry testEntry;

    // State variables
    string targetText;

    bool isSpeaking = false;

    // Start is called before the first frame update
    void Start()
    {   
        StartCoroutine(Say(testEntry));
    }

    // Unpacks the dialogue entry to get the dialogue properties, then updates the textbox
    IEnumerator Say(DialogueEntry dialogue)
    {
        // Unpack the dialogue entry
        this.avatarImage = dialogue.avatar;
        this.targetText = dialogue.content;
        this.charactersPerSecond = dialogue.charactersPerSecond;

        yield return Say();
    }

    // Updates the textbox with the target text
    IEnumerator Say()
    {
        // Update flag
        isSpeaking = true;

        // If typewriting effect is enabled
        if (charactersPerSecond > 0)
        {
            // Get the number of characters
            int totalCharacters = targetText.Length;

            // At every interval, update the textbox text to achieve a typewriter effect
            for (int i = 0; i <= totalCharacters; i++)
            {
                dialogueContentTMP.text = targetText.Substring(0, i);
                yield return new WaitForSeconds(1.0f / charactersPerSecond);
            }
        }
        // Else, don't typewrite the text.
        else
        {
            dialogueContentTMP.text = targetText;
        }

        // Update flag
        isSpeaking = false;
    }

}