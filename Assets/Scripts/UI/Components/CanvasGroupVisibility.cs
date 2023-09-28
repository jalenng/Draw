using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
[RequireComponent(typeof(CanvasGroup))]
public class CanvasGroupVisibility : MonoBehaviour
{
    private CanvasGroup canvasGroup;

    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void SetVisibility(bool visibility) {
        canvasGroup.alpha = visibility ? 1f : 0f;
        canvasGroup.blocksRaycasts = visibility;
        canvasGroup.interactable = visibility;
    }
}
