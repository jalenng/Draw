using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
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
    }

    public void Return()
    {
        levelLoader.LoadMainMenu();
    }

}
