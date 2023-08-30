using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SettingsMenu : MonoBehaviour
{
    // Object references
    [SerializeField] private GameObject versionTMPObj;
    [SerializeField] private MenuManager menuHolder;

    void Start()
    {
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
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            menuHolder.enableSettings(false);
        }
    }
}
