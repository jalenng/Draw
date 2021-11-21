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

    private void Start()
    {
        trigger = GameObject.Find("CutsceneManager").GetComponent<TimelineTrigger>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!hasPlayed && other.gameObject.CompareTag("Player"))
        {
            trigger.Trigger(cutscene);
            hasPlayed = true;
        }
    }
    

}
