using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DisplaySettings : MonoBehaviour
{
    // Object references
    [Header("Option widgets")]
    public TMP_Dropdown fullscreenModeDropdown;
    public TMP_Dropdown resolutionDropdown;

    // State variables
    private List<FullScreenMode> fullScreenModes;
    private Resolution[] resolutions;

    // Start is called before the first frame update
    void Start()
    {
        // Create list of fullscreen modes based on current platform's support
        fullScreenModes = new List<FullScreenMode>();
        // Windows only
        if (Application.platform == RuntimePlatform.WindowsPlayer)
            fullScreenModes.Add(FullScreenMode.ExclusiveFullScreen);

        // All platforms
        fullScreenModes.Add(FullScreenMode.FullScreenWindow);

        // macOS only
        if (Application.platform == RuntimePlatform.OSXPlayer)
            fullScreenModes.Add(FullScreenMode.MaximizedWindow);

        // Desktop platforms only
        if (SystemInfo.deviceType == DeviceType.Desktop)
            fullScreenModes.Add(FullScreenMode.Windowed);

        // Retrieve list of supported resolutions
        resolutions = Screen.resolutions;

        // Populate dropdown menu with options and update its selection values
        CreateFullscreenModeOptions();
        CreateResolutionOptions();
        UpdateDropdownValues(Screen.currentResolution, Screen.fullScreenMode);
    }

    void Update()
    {
        // Only enable resolution picker if in fullscreen mode
        resolutionDropdown.interactable = Screen.fullScreen;
    }

    // Create the menu options for the fullscreen dropdown
    private void CreateFullscreenModeOptions()
    {
        Dictionary<FullScreenMode, string> fullscreenModeToLabelMap = new Dictionary<FullScreenMode, string>()
        {
            { FullScreenMode.ExclusiveFullScreen, "Exclusive Fullscreen"},
            { FullScreenMode.FullScreenWindow, "Fullscreen Window"},
            { FullScreenMode.MaximizedWindow, "Maximized Window"},
            { FullScreenMode.Windowed, "None (Windowed)"}
        };

        // Create options based on the supported fullscreen modes
        List<TMP_Dropdown.OptionData> optionsList = new List<TMP_Dropdown.OptionData>();
        for (int i = 0; i < fullScreenModes.Count; i++)
        {
            FullScreenMode fullscreenMode = fullScreenModes[i];
            TMP_Dropdown.OptionData newOption = new TMP_Dropdown.OptionData();
            fullscreenModeToLabelMap.TryGetValue(fullscreenMode, out string fullScreenModeLabel);
            newOption.text = fullScreenModeLabel;
            optionsList.Add(newOption);
        }

        // Update the dropdown options
        fullscreenModeDropdown.ClearOptions();
        fullscreenModeDropdown.AddOptions(optionsList);
    }

    // Create the menu options for the resolution dropdown
    private void CreateResolutionOptions()
    {
        List<TMP_Dropdown.OptionData> optionsList = new List<TMP_Dropdown.OptionData>();

        // Create options based on the supported fullscreen resolutions
        for (int i = 0; i < resolutions.Length; i++)
        {
            Resolution resolution = resolutions[i];
            TMP_Dropdown.OptionData newOption = new TMP_Dropdown.OptionData();
            newOption.text = resolution.ToString();
            optionsList.Add(newOption);
        }

        // Update the dropdown options
        resolutionDropdown.ClearOptions();
        resolutionDropdown.AddOptions(optionsList);
    }

    // Update the dropdown selection values in the UI
    private void UpdateDropdownValues(Resolution resolution, FullScreenMode fullScreenMode)
    {
        // Set fullscreen mode value
        int fullscreenModeIndex = fullScreenModes.IndexOf(fullScreenMode);
        fullscreenModeDropdown.SetValueWithoutNotify(fullscreenModeIndex > -1 ? fullscreenModeIndex : (fullScreenModes.Count - 1));

        // Set resolution value
        int resolutionIndex = Array.FindIndex(
            Screen.resolutions,
            resItem => (
                resItem.width == resolution.width &&
                resItem.height == resolution.height &&
                resItem.refreshRate == resolution.refreshRate
            )
        );
        resolutionDropdown.SetValueWithoutNotify(resolutionIndex > -1 ? resolutionIndex : (resolutions.Length - 1));
    }

    // Handle fullscreen mode dropdown selection change
    public void OnFullScreenModeChange()
    {
        // Get fullscreen mode option
        int selectedIndex = fullscreenModeDropdown.value;

        // Assertion
        bool inBounds = selectedIndex >= 0 && selectedIndex < fullScreenModes.Count;
        Debug.Assert(inBounds, "[DisplaySettings] Tried to set fullscreen mode but index is out of range");

        // Get resolution and fullscreen mode.
        // If entering fullscreen mode, set to the highest resolution.
        FullScreenMode fullscreenMode = fullScreenModes[selectedIndex];
        bool isEnteringFullscreen = fullscreenMode != FullScreenMode.Windowed;
        Resolution resolution = isEnteringFullscreen ? resolutions[resolutions.Length - 1] : Screen.currentResolution;
        Debug.Log($"[DisplaySettings] Fullscreen mode dropdown value set to {fullscreenMode}");

        // Apply change
        SetResolution(resolution, fullscreenMode);
    }

    // Handle resolution dropdown selection change
    public void OnResolutionChange()
    {
        // Get resolution option
        int selectedIndex = resolutionDropdown.value;

        // Assertion
        bool inBounds = selectedIndex >= 0 && selectedIndex < resolutions.Length;
        Debug.Assert(inBounds, "[DisplaySettings] Tried to set resolution but index is out of range");

        // Get resolution and fullscreen mode
        FullScreenMode fullscreenMode = Screen.fullScreenMode;
        Resolution resolution = resolutions[selectedIndex];
        string resolutionToString = resolution.ToString();
        Debug.Log($"[DisplaySettings] Resolution dropdown value set to {resolutionToString}");

        // Apply change
        SetResolution(resolution, fullscreenMode);
    }

    // Set the selected resolution and fullscreen mode and update the UI
    private void SetResolution(Resolution resolution, FullScreenMode fullScreenMode)
    {
        string resolutionToString = resolution.ToString();
        Debug.Log($"[DisplaySettings] Setting resolution to {resolutionToString} and fullscreen mode to {fullScreenMode}");
        Screen.SetResolution(resolution.width, resolution.height, fullScreenMode, resolution.refreshRate);

        UpdateDropdownValues(resolution, fullScreenMode);
    }
}