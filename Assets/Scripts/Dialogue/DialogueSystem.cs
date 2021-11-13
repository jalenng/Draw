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
        
        SetTextboxVisibility(true);
        StartCoroutine(DisplayDialogue());
    }

    private IEnumerator DisplayDialogue()
    {
        while (true)
        {
            // Get the dialogue entry
            DialogueEntry dialogueEntry = dialogue.entries[entryIndex];

            // Update the textbox properties
            textbox.setAvatar(dialogueEntry.avatar);
            textbox.setText(dialogueEntry.content);
            if (dialogueEntry.useCPS)   // If the dialogue entry uses custom CPS, use it.
                textbox.setCPS(dialogueEntry.CPS);
            else                        // Otherwise, use the default CPS.
                textbox.setCPS(dialogue.CPS);

            // Show the content text
            yield return textbox.Say();

            // Move to the next entry, or end the dialogue if we're at the end
            if (++entryIndex >= dialogue.entries.Count)
            {
                textbox.Clear();
                SetTextboxVisibility(false);
                break;
            }
        }
    }

    private void SetTextboxVisibility(bool visible)
    {
        textbox.gameObject.SetActive(visible);
    }

}
