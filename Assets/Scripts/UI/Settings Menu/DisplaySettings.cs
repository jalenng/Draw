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
        {
            fullScreenModes.Add(FullScreenMode.ExclusiveFullScreen);
        }
        // All platforms
        fullScreenModes.Add(FullScreenMode.FullScreenWindow);
        // macOS only
        if (Application.platform == RuntimePlatform.OSXPlayer)
        {
            fullScreenModes.Add(FullScreenMode.MaximizedWindow);
        }
        // Desktop platforms only
        if (SystemInfo.deviceType == DeviceType.Desktop)
        {
            fullScreenModes.Add(FullScreenMode.Windowed);
        }

        // Retrieve list of supported resolutions
        resolutions = Screen.resolutions;

        // Populate options and set values for display setting dropdowns
        UpdateFullscreenModeOptions();
        UpdateResolutionOptions();
    }

    void Update()
    {
        // Only enable resolution picker if in fullscreen mode
        resolutionDropdown.interactable = Screen.fullScreen;
    }

    private void UpdateFullscreenModeOptions()
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

        // Set current value
        int index = fullScreenModes.IndexOf(Screen.fullScreenMode);
        fullscreenModeDropdown.value = index > -1 ? index : (fullScreenModes.Count - 1);
    }

    private void UpdateResolutionOptions()
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

        // Set current value if fullscreen
        if (Screen.fullScreen)
        {
            Resolution currentRes = Screen.currentResolution;
            int index = Array.FindIndex(
                Screen.resolutions,
                res => (
                    res.width == currentRes.width &&
                    res.height == currentRes.height &&
                    res.refreshRate == currentRes.refreshRate
                )
            );
            resolutionDropdown.value = index > -1 ? index : (resolutions.Length - 1);
        }
    }

    // Dropdown value change handlers
    public void OnFullScreenModeChange()
    {
        // Cache current fullscreen mode
        FullScreenMode oldFullScreenMode = Screen.fullScreenMode;

        // Get resolution option
        int selectedIndex = fullscreenModeDropdown.value;

        // Assertion
        bool inBounds = selectedIndex >= 0 && selectedIndex < fullScreenModes.Count;
        Debug.Assert(inBounds, "[SettingsMenu] Selected fullscreen mode is out of index");

        // Change the fullscreen mode
        FullScreenMode selectedFullScreenMode = fullScreenModes[selectedIndex];
        Debug.Log($"[SettingsMenu] Setting fullscreen mode to {selectedFullScreenMode}");

        // If coming from Windowed mode, set to the highest resolution
        if (oldFullScreenMode == FullScreenMode.Windowed && selectedFullScreenMode != FullScreenMode.Windowed)
        {
            Debug.Log($"[SettingsMenu] Entering fullscreen and leaving windowed mode. Setting resolution to the highest resolution.");
            resolutionDropdown.value = resolutions.Length - 1;
        }
        // Else, only just change fullscreen mode
        else
        {
            Screen.fullScreenMode = selectedFullScreenMode;
        }
    }
    public void OnResolutionChange()
    {
        if (!Screen.fullScreen)
        {
            Debug.Log($"[SettingsMenu] Tried to change resolution but game is not in fullscreen");
            return;
        }

        // Get resolution option
        int selectedIndex = resolutionDropdown.value;

        // Assertion
        bool inBounds = selectedIndex >= 0 && selectedIndex < resolutions.Length;
        Debug.Assert(inBounds, "[SettingsMenu] Selected resolution is out of index");

        // Change the resolution
        Resolution selectedResolution = resolutions[selectedIndex];
        string resolutionToString = selectedResolution.ToString();
        Debug.Log($"[SettingsMenu] Setting resolution to {resolutionToString}");
        Screen.SetResolution(selectedResolution.width, selectedResolution.height, Screen.fullScreenMode, selectedResolution.refreshRate);
    }
}
