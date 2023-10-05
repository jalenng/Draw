using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using System.Linq;

public class TimelineTrigger : MonoBehaviour, INotificationReceiver
{
    public PlayableDirector timeline;
    public SignalAsset pauseSignal;

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

    // Listen for the timeline pause signal.
    // If emitted, set the timeline to the signal time, as the way the timeline triggers can end up overshoot past the stop signal.
    public void OnNotify(Playable origin, INotification notification, object context)
    {
        if (!Application.isPlaying) return;

        SignalEmitter signalEmitter = notification as SignalEmitter;
        bool shouldStop = signalEmitter && signalEmitter.asset == pauseSignal;
        if (shouldStop)
        {
            double time = signalEmitter.time;
            Debug.Log($"[TimelineTrigger] Stop signal received. Pausing timeline and setting time to {time}. ");
            PauseTimeline();
            timeline.time = time + 0.01f;
        }
    }

    public void PauseTimeline()
    {
        timeline.playableGraph.GetRootPlayable(0).SetSpeed(0);
    }

    public void ResumeTimeline()
    {
        timeline.playableGraph.GetRootPlayable(0).SetSpeed(1);
    }

}
