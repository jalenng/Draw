using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CutsceneTrigger : MonoBehaviour
{
    public PlayableAsset cutscene;
    private TimelineTrigger trigger;

    public bool hasPlayed;
    public bool triggerable;

    private void Start()
    {
        trigger = FindObjectOfType<TimelineTrigger>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!hasPlayed && other.gameObject.CompareTag("Player"))
        {
            other.gameObject.transform.parent = transform.parent.transform;
            // TriggerCutscene();
            StartCoroutine(TriggerCutsceneRoutine());
        }
    }

    public IEnumerator TriggerCutsceneRoutine()
    {
        yield return trigger.StartCutscene(cutscene);
        hasPlayed = true;
    }

    public void TriggerCutscene()
    {
        hasPlayed = true;
        trigger.Trigger(cutscene);
    }

    public void ToggleTriggerable()
    {
        triggerable = true;
    }
    
}
