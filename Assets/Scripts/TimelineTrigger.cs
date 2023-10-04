using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using System.Linq;

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
        timeline.time = 0;
        timeline.Play();

        // Wait for timeline to finish playing
        yield return new WaitUntil(() => timeline.state == PlayState.Paused);
    }

    public void GoToEndState(PlayableAsset cutscene, bool ignorePlayer = true)
    {
        timeline.playableAsset = cutscene;

        // When going to the end state of a cutscene, we may not want to affect the player. 
        // Otherwise, the player/Stickman tracks may override the playerData's loaded position.
        if (ignorePlayer)
        {
            GameObject player = GameObject.FindWithTag("Player");

            // Retrieve the bindings from the track to the objects in the currently loaded scene.
            IEnumerable<PlayableBinding> playerOutputs = cutscene.outputs;
            List<PlayableBinding> playerOutputsList = playerOutputs.ToList();

            // Iterate through each track of the timeline.
            for (int i = 0; i < playerOutputsList.Count(); i++)
            {
                TrackAsset track = (playerOutputsList[i].sourceObject) as TrackAsset;
                if (track != null)
                {
                    Object referenceObject = timeline.GetGenericBinding(track);

                    // Does this track reference the player GameObject?
                    // If so, unbind the player from the track.
                    if (referenceObject == player || (referenceObject as Behaviour)?.gameObject == player)
                    {
                        timeline.ClearGenericBinding(track);
                    }
                }
            }
        }

        // Go to the last frame of the cutscene and evaluate it.
        // The scene will be in the state as if the cutscene has already finished.
        timeline.time = timeline.duration;
        timeline.Evaluate();
        timeline.Stop();
    }
}
