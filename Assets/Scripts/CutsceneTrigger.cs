using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CutsceneTrigger : MonoBehaviour
{
    public PlayableDirector timeline;

    public PlayableAsset cutscene;

    public bool hasPlayed;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!hasPlayed && other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(StartCutscene());
        }
    }

    private IEnumerator StartCutscene()
    {
        yield return new WaitForSeconds(.1f);
        hasPlayed = true;
        timeline.playableAsset = cutscene;
        timeline.Play();
    }
}
