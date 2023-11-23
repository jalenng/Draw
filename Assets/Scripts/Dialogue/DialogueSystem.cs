using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class DialogueSystem : MonoBehaviour
{
    // Configuration parameters
    [SerializeField] private List<Dialogue> dialogueObjects;

    [Header("Component References")]
    [SerializeField] GameObject dialogueCanvas;
    [SerializeField] Textbox textbox;
    [SerializeField] GameObject skipButton;
    [SerializeField] GameObject skipConfirmationDialog;

    // Cached object references
    private PlayableDirector director;

    // State variables
    private Dialogue dialogue;
    private IEnumerator dialogueCoroutine;
    private int entryIndex = 0;
    private bool skip = false;
    private bool advanceRequested = false;

    private void Start()
    {
        director = FindObjectOfType<PlayableDirector>();
    }

    private void Update()
    {
        // Fast forward current dialogue entry if the advance key is pressed.
        // Allows players to advance the dialogue while the typewriting effect is still in progress.
        if (CheckIfShouldAdvance())
        {
            textbox.FastForward();
        }
    }
    // Function called at the end of all other updates, to let DisplayDialogue check advKeyPressed before we reset it.
    private void LateUpdate()
    {
        advanceRequested = false;
    }

    private bool CheckIfShouldAdvance()
    {
        // If the game is paused, do not advance
        if (Time.timeScale <= 0) return false;

        bool skipConfirmationVisible = skipConfirmationDialog.GetComponent<CanvasGroup>().interactable;
        bool keyDown = !skipConfirmationVisible && Input.GetButtonDown("Submit"); // Space or Enter
        return advanceRequested || keyDown;
    }

    public void Skip()
    {
        textbox.FastForward();
        skip = true;
        entryIndex = dialogue.entries.Count;
    }

    // Queue a dialogue object to be displayed
    public void QueueDialogue(Dialogue dialogue)
    {
        ResetDialogueSystem();

        if (dialogue == null)
        {
            Debug.LogError("[DialogueSystem] Tried to enqueue a dialogue object that does not exist");
            return;
        }

        this.dialogue = dialogue;
        dialogueCoroutine = DisplayDialogue();
        StartCoroutine(dialogueCoroutine);
    }

    // Queue a dialogue object to be displayed based on index
    public void QueueDialogue(int index)
    {
        QueueDialogue(dialogueObjects[index]);
    }

    public void ResumeTimeline()
    {
        if (director)
        {
            director.playableGraph.GetRootPlayable(0).SetSpeed(1);
        }
    }

    [ContextMenu("Reset Dialogue System")]
    public void ResetDialogueSystem()
    {
        if (dialogueCoroutine != null)
        {
            StopCoroutine(dialogueCoroutine);
        }
        entryIndex = 0;
        dialogue = null;
        skip = false;

        textbox.Clear();
        SetTextboxVisibility(false);
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
            textbox.setSFXDirectory(dialogue.SFXDirectory);
            textbox.setCPS(dialogue.CPS);

            // Show the content text
            yield return textbox.Say();

            // Wait for the user to advance the dialogue
            yield return new WaitUntil(() => CheckIfShouldAdvance() || skip);
            skip = false;

            // Move to the next entry, or end the dialogue if we're at the end
            if (++entryIndex >= dialogue.entries.Count)
                break;
        }

        // Clear and hide the textbox
        textbox.Clear();
        SetTextboxVisibility(false);

        // If there is a playable director, resume the timeline/cutscene
        ResumeTimeline();
    }

    // Set the visibility of the textbox
    private void SetTextboxVisibility(bool visible)
    {
        dialogueCanvas.GetComponent<CanvasGroupVisibility>().SetVisibility(visible);
    }

    // Request to advance the dialogue
    public void RequestAdvance()
    {
        advanceRequested = true;
    }
}