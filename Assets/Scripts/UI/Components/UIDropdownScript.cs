using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIDropdownScript : MonoBehaviour
{
    // Configuration parameters
    [SerializeField] private Color enabledFGColor = Color.black;
    [SerializeField] private Color disabledFGColor = new Color(0.75f, 0.75f, 0.75f, 1f);

    // Cached components
    [SerializeField] private GameObject textObject;

    private TextMeshProUGUI tmp;
    private TMP_Dropdown dropdown;
    private Outline outline;

    private void Start()
    {
        tmp = textObject.GetComponent<TextMeshProUGUI>();
        dropdown = GetComponent<TMP_Dropdown>();
        outline = GetComponent<Outline>();
    }

    private void Update()
    {
        // Get the disabled state of the dropdown
        bool isDisabled = !dropdown.interactable;
        Color fgColor = isDisabled ? disabledFGColor : enabledFGColor;

        // Set text color and outline color based on the state
        tmp.color = fgColor;
        outline.effectColor = fgColor;
    }
}
