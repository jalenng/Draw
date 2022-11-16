using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class OrangeObjectTrigger : MonoBehaviour
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
        if (!hasPlayed && other.gameObject.CompareTag("OrangeObject"))
        {
            hasPlayed = true;
            Debug.Log("OrangeObject trigger");
            other.gameObject.transform.parent = transform.parent.transform;
            // TriggerCutscene();
            StartCoroutine(TriggerCutsceneRoutine());
        }
    }

    public IEnumerator TriggerCutsceneRoutine()
    {
        yield return trigger.StartCutscene(cutscene);
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
