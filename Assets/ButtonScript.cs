using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonScript : MonoBehaviour
{
    // Configuration parameters
    [SerializeField] private Color enabledFGColor = Color.black;
    [SerializeField] private Color disabledFGColor = new Color(0.75f, 0.75f, 0.75f, 1f);

    // Cached components
    [SerializeField] private GameObject textObject;

    private TextMeshProUGUI tmp;
    private Button button;
    private Outline outline;

    private void Start()
    {
        tmp = textObject.GetComponent<TextMeshProUGUI>();
        button = GetComponent<Button>();
        outline = GetComponent<Outline>();
    }

    private void Update()
    {
        // Get the disabled state of the button
        bool isDisabled = !button.interactable;
        Color fgColor = isDisabled ? disabledFGColor : enabledFGColor;

        // Set text color and outline color based on the state
        tmp.color = fgColor;
        outline.effectColor = fgColor;
    }
}
