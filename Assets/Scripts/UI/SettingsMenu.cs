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

    LevelLoader levelLoader;

    void Start()
    {
        levelLoader = FindObjectOfType<LevelLoader>();
    }

    public void Return()
    {
        levelLoader.LoadMainMenu();
    }

}
