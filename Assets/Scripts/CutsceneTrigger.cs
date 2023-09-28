using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;

public class CutsceneTrigger : MonoBehaviour
{
    // Configuration variables
    [SerializeField] private PlayableAsset cutscene;
    [SerializeField] private LayerMask triggerableLayers;
    [SerializeField] private List<CutsceneTrigger> dependencies;

    // Cached objects
    private TimelineTrigger trigger;

    // State variables
    public bool hasPlayed;
    public bool hasTriggered;

    private void Start()
    {
        trigger = FindObjectOfType<TimelineTrigger>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // See if all conditions are met before triggering
        bool layerIsTriggerable = triggerableLayers == (triggerableLayers | (1 << other.gameObject.layer));
        if (!hasTriggered && layerIsTriggerable)
        {
            if (hasPlayed)
            {
                Debug.Log("[CutsceneTrigger] Tried to trigger cutscene but it has already been played", gameObject);
                return;
            }

            bool dependenciesMet = dependencies.All(dependency => dependency.hasPlayed);
            if (!dependenciesMet)
            {
                Debug.Log("[CutsceneTrigger] Tried to trigger cutscene but its dependencies have not been played", gameObject);
                return;
            }

            TriggerCutscene();
        }
    }

    [ContextMenu("Go to End State")]
    public void GoToEndState()
    {
        Debug.Log($"[CutsceneTrigger] Going to end state of a cutscene", gameObject);
        trigger.GoToEndState(cutscene);
        hasPlayed = true;
    }

    [ContextMenu("Trigger Cutscene")]
    public void TriggerCutscene()
    {
        hasTriggered = true;

        Debug.Log($"[CutsceneTrigger] Cutscene triggered", gameObject);

        // Bring player into the cutscene group GameObject
        Transform playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        Transform cutsceneGroupTransform = transform.parent.transform;
        playerTransform.SetParent(cutsceneGroupTransform, true);

        StartCoroutine(CutsceneCoroutine());
    }

    public IEnumerator CutsceneCoroutine()
    {
        yield return trigger.StartCutscene(cutscene);
        hasPlayed = true;
    }
}
