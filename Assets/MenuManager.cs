using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject settingsMenu;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject levels;

    void Start()
    {
        if(GetCurrentSceneIndex() != 0) mainMenu.SetActive(false);
        settingsMenu.SetActive(false);
        // I'm making pause menu active with a deactived canvas b/c pausemenu needs to be active
        // For its script to run and check for an "esc" button press.
        pauseMenu.SetActive(true);
        levels.SetActive(false);
    }
    public void enableSettings() {
        settingsMenu.SetActive(true);
    }
    public void enablePause() {
        pauseMenu.SetActive(true);
    }
    public void enableLevels() {
        levels.SetActive(true);
    }

    // Returns the current scene index (stolen from SceneLoader muahaha)
    private int GetCurrentSceneIndex()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }
}
