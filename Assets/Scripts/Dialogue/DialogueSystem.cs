using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class DialogueSystem : MonoBehaviour
{
    // Configuration parameters
    [SerializeField] Dialogue dialogue;
    [SerializeField] KeyCode advanceKey = KeyCode.Space;
    [SerializeField] Textbox textbox;

    // Cached object references
    private PlayableDirector director;

    // State variables
    int entryIndex = 0;

    private void Start() {
        // if (dialogue)
        //     QueueDialogue(dialogue);
        director = FindObjectOfType<PlayableDirector>();
    }

    private void Update() {
        // Fast forward current dialogue entry if the advance key is pressed.
        // Allows players to advance the dialogue while the typewriting effect is still in progress.
        if (Input.GetKeyDown(advanceKey)) 
            textbox.FastForward();
    }

    // Queue a dialogue to be displayed
    public void QueueDialogue(Dialogue dialogue)
    {
        // If there is a playable director, pause the timeline/cutscene
        if (director) 
            PauseTimeline();

        this.dialogue = dialogue; 
        this.entryIndex = 0;  
        
        StartCoroutine(DisplayDialogue());
    }

    public void PauseTimeline()
    {
        director.playableGraph.GetRootPlayable(0).SetSpeed(0);
        // director.Pause();
    }
    public void ResumeTimeline()
    {
        director.playableGraph.GetRootPlayable(0).SetSpeed(1);
        // director.Resume();
    }

    // Display the next entry in the dialogue
    private IEnumerator DisplayDialogue()
    {
        SetTextboxVisibility(true);

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

            // Wait for the user to advance the dialogue
            yield return new WaitUntil(() => Input.GetKeyDown(advanceKey));

            // Move to the next entry, or end the dialogue if we're at the end
            if (++entryIndex >= dialogue.entries.Count)
                break;                
        }

        // Clear and hide the textbox
        textbox.Clear();
        SetTextboxVisibility(false);

        // If there is a playable director, resume the timeline/cutscene
        if (director) 
            ResumeTimeline();
    }

    // Set the visibility of the textbox
    private void SetTextboxVisibility(bool visible)
    {
        textbox.gameObject.SetActive(visible);
    }

}