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
            TriggerCutscene();
            other.gameObject.transform.parent = transform.parent.transform;
        }
    }

    public void TriggerCutscene()
    {
        trigger.Trigger(cutscene);
        hasPlayed = true;
    }

    public void ToggleTriggerable()
    {
        triggerable = true;
    }
    
}
