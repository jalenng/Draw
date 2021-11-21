using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineTrigger : MonoBehaviour
{
    public PlayableDirector timeline;

    public void Trigger(PlayableAsset cutscene)
    {
        StartCoroutine(StartCutscene(cutscene));
    }

    private IEnumerator StartCutscene(PlayableAsset cutscene)
    {
        yield return new WaitForSeconds(.1f);
        timeline.playableAsset = cutscene;
        timeline.Play();
    }
}
