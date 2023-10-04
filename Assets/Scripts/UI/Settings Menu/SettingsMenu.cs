using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(CanvasGroupVisibility))]
public class SettingsMenu : MonoBehaviour
{
    // Object references
    [SerializeField] private GameObject versionTMPObj;
    private CanvasGroupVisibility settingsMenuCanvas;

    void Start()
    {
        settingsMenuCanvas = GetComponent<CanvasGroupVisibility>();

        // Update version number text
        TextMeshProUGUI versionTMP = versionTMPObj?.GetComponent<TextMeshProUGUI>();
        if (versionTMP)
        {
            versionTMP.text = $"v{Application.version}";
            if (Debug.isDebugBuild)
            {
                versionTMP.text += "-debug";
            }
        }
    }

    void Update()
    {
        if (Input.GetButtonDown("Cancel")) // Esc
        {
            settingsMenuCanvas.SetVisibility(false);
        }
    }
}
