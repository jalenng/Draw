using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class SettingsMenu : MonoBehaviour
{
    // Audio mixer
    [Header("Audio Mixer")]
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] AudioMixerParamsConfig mixerParams;
    // Sliders
    [Header("Sliders")]
    public Slider masterVolumeSlider;
    public Slider BGMVolumeSlider;
    public Slider SFXVolumeSlider;

    // Object references
    [SerializeField] private GameObject versionTMPObj;
    [SerializeField] private MenuManager menuHolder;

    SceneLoader levelLoader;

    // Helper function to logarithmically map (0, 1) to a decibel value used for the audio mixer attenuation.
    float RatioToDecibel(float ratio)
    {
        float unclambedDecibel = Mathf.Log10(ratio) * 20;
        return Mathf.Clamp(unclambedDecibel, -80, 0);
    }

    // Helper function to logarithmically map a decibel value used for the audio mixer attenuation to (0, 1).
    float DecibelToRatio(float decibel)
    {
        float unclambedRatio = Mathf.Pow(10, decibel / 20);
        return Mathf.Clamp(unclambedRatio, 0, 1);
    }

    void Start()
    {
        levelLoader = FindObjectOfType<SceneLoader>();

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

        // Set sliders to current values of the audio mixer
        float masterVolume;
        audioMixer.GetFloat(mixerParams.masterVolume, out masterVolume);
        masterVolumeSlider.value = DecibelToRatio(masterVolume);

        float BGMVolume;
        audioMixer.GetFloat(mixerParams.BGMVolume, out BGMVolume);
        BGMVolumeSlider.value = DecibelToRatio(BGMVolume);

        float SFXVolume;
        audioMixer.GetFloat(mixerParams.SFXVolume, out SFXVolume);
        SFXVolumeSlider.value = DecibelToRatio(SFXVolume);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            menuHolder.enableSettings(false);
        }
    }
    public void OnMasterVolumeChanged()
    {
        audioMixer.SetFloat(mixerParams.masterVolume, RatioToDecibel(masterVolumeSlider.value));

    }

    public void OnBGMVolumeChanged()
    {
        audioMixer.SetFloat(mixerParams.BGMVolume, RatioToDecibel(BGMVolumeSlider.value));
    }

    public void OnSFXVolumeChanged()
    {
        audioMixer.SetFloat(mixerParams.SFXVolume, RatioToDecibel(SFXVolumeSlider.value));
    }
    public void ReturnToMenu()
    {
        levelLoader.LoadMainMenu();
    }

}
