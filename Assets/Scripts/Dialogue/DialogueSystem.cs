using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueSystem : MonoBehaviour
{
    [SerializeField] Dialogue dialogue;
    
    // Cached components
    Textbox textbox;

    // State variables
    int entryIndex = 0;

    private void Start() {
        textbox = GetComponent<Textbox>();

        QueueDialogue(dialogue);
    }

    public void QueueDialogue(Dialogue dialogue)
    {
        this.dialogue = dialogue; 
        this.entryIndex = 0;  
        StartCoroutine(DisplayDialogue());
    }

    IEnumerator DisplayDialogue()
    {
        while (true)
        {
            // Get the dialogue entry
            DialogueEntry dialogueEntry = dialogue.entries[entryIndex];

            // Update the textbox properties
            textbox.setAvatar(dialogueEntry.avatar);
            textbox.setText(dialogueEntry.content);
            if (dialogueEntry.useCPS)
                textbox.setCPS(dialogueEntry.CPS);
            else
                textbox.setCPS(dialogue.CPS);

            // Show the content text
            yield return textbox.Say();

            entryIndex++;

            if (entryIndex >= dialogue.entries.Count)
            {
                textbox.Clear();
                break;
            }
        }
    }

}
