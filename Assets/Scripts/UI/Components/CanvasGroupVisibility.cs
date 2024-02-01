using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Canvas))]
[RequireComponent(typeof(CanvasGroup))]
public class CanvasGroupVisibility : MonoBehaviour
{
    [SerializeField] private GameObject selectionOnShow;
    [SerializeField] private bool returnSelectionOnHide = true;

    // Actions
    [SerializeField] private UnityEvent onShow;
    [SerializeField] private UnityEvent onHide;

    private CanvasGroup canvasGroup;
    private GameObject selectionBeforeOpen;

    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        UpdateFocus();
    }

    public void SetVisibility(bool visibility)
    {
        // Set canvas group properties
        canvasGroup.alpha = visibility ? 1f : 0f;
        canvasGroup.blocksRaycasts = visibility;
        canvasGroup.interactable = visibility;

        UpdateFocus();

        if (visibility)
            onShow.Invoke();
        else
            onHide.Invoke();
    }

    private void UpdateFocus()
    {
        EventSystem currEventSys = EventSystem.current;
        // If showing this UI, focus on selectionOnShow if specified.
        // Also, remember the currently focused object for when this UI hides.
        if (canvasGroup.interactable)
        {
            selectionBeforeOpen = currEventSys.currentSelectedGameObject;
            if (selectionOnShow != null)
            {
                currEventSys.SetSelectedGameObject(selectionOnShow);
            }
        }
        // Else, this UI is hiding.
        // Focus on the object that was focused before this UI was shown.
        else if (returnSelectionOnHide && selectionBeforeOpen != null)
        {
            currEventSys.SetSelectedGameObject(selectionBeforeOpen);
        }
    }
}
