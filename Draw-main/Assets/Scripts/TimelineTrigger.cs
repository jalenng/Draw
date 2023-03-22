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

    public IEnumerator StartCutscene(PlayableAsset cutscene)
    {
        yield return new WaitForSeconds(.1f);

        timeline.playableAsset = cutscene;
        timeline.Play();

        // Wait for timeline to finish playing
        yield return new WaitUntil(() => timeline.state == PlayState.Paused);
    }
}
