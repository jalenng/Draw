using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject settingsMenu;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject levelMenu;

    Canvas mmCanvas;
    Canvas smCanvas;
    Canvas pmCanvas;
    Canvas lmCanvas;

    void Start()
    {
        // Enabling/Disabling canvases, because it seems that enabling canvases instead of whole objects
        // helps to avoid scaling bugs. 
        mmCanvas = mainMenu.GetComponent<Canvas>();
        smCanvas = settingsMenu.GetComponent<Canvas>();
        pmCanvas = pauseMenu.GetComponent<Canvas>();
        lmCanvas = levelMenu.GetComponent<Canvas>();
        // Only enable Main Menu Canvas if we're in the Main Menu Scene
        mmCanvas.enabled = (GetCurrentSceneIndex() == 0);
        // Disable all other canvases
        smCanvas.enabled = pmCanvas.enabled = lmCanvas.enabled = false;
    }
    // Public functions for OnClick() to call
    public void enableSettings(bool enable) {
        smCanvas.enabled = enable;
    }
    public void enablePause(bool enable) {
        pmCanvas.enabled = enable;
    }
    public void enableLevels(bool enable) {
        lmCanvas.enabled = enable;
    }
    // Shamelessly stolen from another script
    private int GetCurrentSceneIndex()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }
}
