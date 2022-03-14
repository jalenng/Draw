using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

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
    
    // Buttons
    [Header("Buttons")]
    public Button returnButton;

    SceneLoader levelLoader;

    void Start()
    {
        levelLoader = FindObjectOfType<SceneLoader>();

        // Set sliders to current values of the audio mixer
        float masterVolume;
        audioMixer.GetFloat(mixerParams.masterVolume, out masterVolume);
        masterVolumeSlider.value = masterVolume;

        float BGMVolume;
        audioMixer.GetFloat(mixerParams.BGMVolume, out BGMVolume);
        BGMVolumeSlider.value = BGMVolume;

        float SFXVolume;
        audioMixer.GetFloat(mixerParams.SFXVolume, out SFXVolume);
        SFXVolumeSlider.value = SFXVolume;
    }

    public void OnMasterVolumeChanged()
    {
        audioMixer.SetFloat(mixerParams.masterVolume, masterVolumeSlider.value);
    }

    public void OnBGMVolumeChanged()
    {
        audioMixer.SetFloat(mixerParams.BGMVolume, BGMVolumeSlider.value);
    }

    public void OnSFXVolumeChanged()
    {
        audioMixer.SetFloat(mixerParams.SFXVolume, SFXVolumeSlider.value);
    }

    public void Return()
    {
        levelLoader.LoadMainMenu();
    }

}
