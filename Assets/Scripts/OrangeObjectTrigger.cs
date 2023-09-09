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

    private void Start()
    {
        trigger = FindObjectOfType<TimelineTrigger>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!hasPlayed && other.gameObject.CompareTag("OrangeObject"))
        {
            hasPlayed = true;
            Debug.Log("[OrangeObjectTrigger] Triggered");
            other.gameObject.transform.SetParent(transform.parent.transform, true);
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
}
