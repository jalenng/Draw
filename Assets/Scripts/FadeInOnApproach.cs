using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(CanvasGroup))]
public class FadeInOnApproach : MonoBehaviour
{
  // Configuration parameters  
  [SerializeField] float fadeDuration = 1.5f;
  [SerializeField] int distanceToFadeIn = 7;
  [SerializeField] Transform targetTransform;

  private Transform playerTransform;
  private bool triggered = false;
  private CanvasGroup canvasGroup;

  void Start()
  {
    canvasGroup = GetComponent<CanvasGroup>();

    SetTransparency(0);
    playerTransform = GameObject.FindWithTag("Player").transform;
    if (targetTransform == null)
    {
      targetTransform = transform;
    }
  }

  void Update()
  {
    float distance = Vector2.Distance(targetTransform.position, playerTransform.position);
    if (!triggered && distance <= distanceToFadeIn)
    {
      triggered = true;
      StartCoroutine(FadeIn());
    }
  }

  IEnumerator FadeIn()
  {
    float startTime = Time.time;
    while (Time.time < startTime + fadeDuration)
    {
      SetTransparency(Mathf.Lerp(0, 1, (Time.time - startTime) / fadeDuration));
      yield return null;
    }
    SetTransparency(1);
  }

  void SetTransparency(float transparency)
  {
    canvasGroup.alpha = transparency;
  }
}