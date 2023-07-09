using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FadeInOnApproach : MonoBehaviour
{
  // Configuration parameters
  [SerializeField] float fadeDuration = 1.5f;
  [SerializeField] int distanceToFadeIn = 7;
  [SerializeField] Transform targetTransform;

  private Transform playerTransform;

  void Start()
  {
    SetTransparency(transform, 0);
    playerTransform = GameObject.FindWithTag("Player").transform;
    if (targetTransform == null)
    {
      targetTransform = transform;
    }
  }

  void Update()
  {
    float distance = Vector2.Distance(targetTransform.position, playerTransform.position);
    if (distance <= distanceToFadeIn)
    {
      StartCoroutine(FadeIn());
    }
  }

  IEnumerator FadeIn()
  {
    float startTime = Time.time;
    while (Time.time < startTime + fadeDuration)
    {
      SetTransparency(transform, Mathf.Lerp(0, 1, (Time.time - startTime) / fadeDuration));
      yield return null;
    }
    SetTransparency(transform, 1);
  }

  void SetTransparency(Transform transform, float transparency)
  {
    // Set transparency for SpriteRenderer components
    SpriteRenderer[] spriteRenderers = transform.GetComponentsInChildren<SpriteRenderer>();
    foreach (SpriteRenderer spriteRenderer in spriteRenderers)
    {
      Color color = spriteRenderer.color;
      color.a = transparency;
      spriteRenderer.color = color;
    }

    // Set transparency for TextMeshPro components
    TextMeshPro[] textMeshPros = transform.GetComponentsInChildren<TextMeshPro>();
    foreach (TextMeshPro textMeshPro in textMeshPros)
    {
      Color color = textMeshPro.color;
      color.a = transparency;
      textMeshPro.color = color;
    }
  }
}