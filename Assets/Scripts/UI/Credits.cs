using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[ExecuteAlways]
public class Credits : MonoBehaviour
{
  // Configuration variables
  [Header("Speed multipliers")]
  [SerializeField] public float progress = 0f;
  [SerializeField] public float normalSpeedMultiplier = 1f;
  [SerializeField] public float speedUpMultiplier = 5f;
  [SerializeField] public RectTransform creditsContentRT;
  [SerializeField] public RectTransform viewportRT;

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
    // Update position based on progress
    creditsContentRT.anchoredPosition = new Vector2(
      0,
      progress * (creditsContentRT.rect.height - viewportRT.rect.height)
    );

    // Update isSpedUp
    bool newIsSpedUp = Input.GetButton("Submit") || Input.GetButton("Advance");

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

