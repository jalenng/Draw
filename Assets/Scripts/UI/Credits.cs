using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;

public class Credits : MonoBehaviour
{
  // Configuration variables
  [Header("Speed multipliers")]
  [SerializeField] public float normalSpeedMultiplier = 1f;
  [SerializeField] public float speedUpMultiplier = 5f;

  [Header("Inputs to activate speed-up")]
  [SerializeField] public KeyCode[] speedUpKeys = { KeyCode.Space, KeyCode.Return };
  [SerializeField] public int[] speedUpButtons = { 0, 1 };

  // Cached components
  private PlayableDirector timeline;

  // State variables
  private bool isSpedUp = false;

  private void Start()
  {
    timeline = GetComponent<PlayableDirector>();
    SetCreditsSpeed(normalSpeedMultiplier);
  }

  private void Update()
  {
    // Update isSpedUp
    bool newIsSpedUp =
      speedUpButtons.Any(button => Input.GetMouseButton(button)) ||
      speedUpKeys.Any(keyCode => Input.GetKey(keyCode));

    // If isSpedUp has changes, update the actual speed multiplier of the playable object
    if (newIsSpedUp != isSpedUp)
    {
      isSpedUp = newIsSpedUp;
      float newMultiplier = isSpedUp ? speedUpMultiplier : normalSpeedMultiplier;
      SetCreditsSpeed(newMultiplier);
    }
  }

  private void SetCreditsSpeed(float speed)
  {
    if (timeline != null && timeline.playableGraph.IsValid())
    {
        Playable playable = timeline.playableGraph.GetRootPlayable(0);
        playable.SetSpeed(speed);
    }
  }
}

