using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Canvas))]
[RequireComponent(typeof(CanvasGroup))]
public class CanvasGroupVisibility : MonoBehaviour
{
    [SerializeField] private GameObject selectionOnShow;
    [SerializeField] private bool returnSelectionOnHide = true;

    private CanvasGroup canvasGroup;
    private GameObject selectionBeforeOpen;

    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void SetVisibility(bool visibility)
    {
        canvasGroup.alpha = visibility ? 1f : 0f;
        canvasGroup.blocksRaycasts = visibility;
        canvasGroup.interactable = visibility;

        EventSystem currEventSys = EventSystem.current;
        if (visibility)
        {
            selectionBeforeOpen = currEventSys.currentSelectedGameObject;
            if (selectionOnShow != null)
            {
                currEventSys.SetSelectedGameObject(selectionOnShow);
            }
        }
        else if (returnSelectionOnHide && selectionBeforeOpen != null)
        {
            currEventSys.SetSelectedGameObject(selectionBeforeOpen);
        }
    }
}
