using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DialogueSystem : MonoBehaviour
{
    // Configuration parameters
    [SerializeField] Dialogue dialogue;
    [SerializeField] KeyCode advanceKey = KeyCode.Space;
    [SerializeField] Textbox textbox;


    // Cached object references
    private PlayableDirector director;
    private Camera camera;

    // All this... to detect if we're clicking the skip button. Lol
    GraphicRaycaster raycaster;
    PointerEventData pointerData;
    EventSystem eventSys;
    Canvas canvas;

    // State variables
    int entryIndex = 0;
    bool skip = false;
    bool skipEnabled = true;

    private void Start()
    {
        director = FindObjectOfType<PlayableDirector>();
        camera = FindObjectOfType<Camera>();
        eventSys = FindObjectOfType<EventSystem>();
        canvas = GetComponentInChildren<Canvas>();
        raycaster = canvas.GetComponent<GraphicRaycaster>();
    }

    private void Update()
    {
        // Fast forward current dialogue entry if the advance key is pressed.
        // Allows players to advance the dialogue while the typewriting effect is still in progress.
        if (CheckIfAdvanceKeyPressed())
            textbox.FastForward();
    }
    // Helper function
    public void enableSkip(bool enabled) {
        skipEnabled = enabled;
    }

    private bool CheckIfAdvanceKeyPressed()
    {
        if(Input.GetKeyDown(advanceKey) || Input.GetMouseButtonDown(0)) {
            // When the player attempts to advance, create this pointer event thing.
            // Raycast from the point clicked at, and check UI objects hit.
            // If any object hit is the skip button, then return false and don't skip.
            // Otherwise, return if we can advance or not.
            pointerData = new PointerEventData(eventSys);
            pointerData.position = Input.mousePosition;
            List<RaycastResult> results = new List<RaycastResult>();
            raycaster.Raycast(pointerData, results);
            foreach (RaycastResult result in results)
            {
                if(result.gameObject.tag == "SkipButton") return false;
            }
            return skipEnabled;
        }
        return false;
    }

    public void Skip() {
        textbox.FastForward();
        skip = true;
        entryIndex = dialogue.entries.Count;
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
            yield return new WaitUntil(() => CheckIfAdvanceKeyPressed() || skip);
            skip = false;

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