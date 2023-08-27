using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] SceneLoaderParamsConfig config;

    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject settingsMenu;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject levelMenu;

    // Using Awake() to initialize menu before other Start() functions 
    void Awake()
    {
        // Only enable Main Menu Canvas if we're in the Main Menu Scene
        if (GetCurrentSceneIndex() == config.mainMenuBuildIndex) {
            mainMenu.SetActive(true);
        } else {
            // Enabling pauseMenu in Awake() so FindObjectOfType() in Start() will return properly
            enablePause(true);
        }
        // Disable all other canvases
        enableSettings(false);
        enableLevels(false);
    }

    // Public functions for OnClick() to call
    public void enableSettings(bool enable) {
        if (settingsMenu) 
            settingsMenu.SetActive(enable);
        else 
            Debug.Log("Tried to enable settings menu but it's undefined");
    }
    public void enablePause(bool enable) {
        if (pauseMenu) 
            pauseMenu.SetActive(enable);
        else 
            Debug.Log("Tried to enable pause menu but it's undefined");
    }
    public void enableLevels(bool enable) {
        if (levelMenu) 
            levelMenu.SetActive(enable);
        else 
            Debug.Log("Tried to enable levels menu but it's undefined");
    }
    // Shamelessly stolen from another script
    private int GetCurrentSceneIndex()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }
}
