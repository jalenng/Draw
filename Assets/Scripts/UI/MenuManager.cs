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
    [SerializeField] GameObject sendFeedbackCanvas;
    PauseMenu pauseMenuScript;

    private void Start()
    {
        // Show the "Send Feedback" canvas only if it is a debug build
        sendFeedbackCanvas.SetActive(Debug.isDebugBuild);
    }

    // Public functions for OnClick() to call
    public void enableSettings(bool enable)
    {
        if (settingsMenu)
            settingsMenu.SetActive(enable);
        else
            Debug.LogError($"[MenuManager] Tried to set settings menu enabled to {enable} but it's undefined");
    }
    public void enablePause(bool enable)
    {
        if (pauseMenu)
        {
            if (Global.GetLevelFromBuildIndex(GetCurrentSceneIndex()) != Global.Level.NONE)
            {
                pauseMenu.SetActive(enable);
                pauseMenuScript = pauseMenu.GetComponent<PauseMenu>();
            }
            else
                Debug.LogWarning($"[MenuManager] Tried to set pause menu enabled to {enable} but we're not in a game level");
        }
        else
            Debug.LogError($"[MenuManager] Tried to set pause menu enabled to {enable} but it's undefined");
    }
    public void enableLevels(bool enable)
    {
        if (levelMenu)
            levelMenu.SetActive(enable);
        else
            Debug.LogError($"[MenuManager] Tried to set level menu enabled to {enable} but it's undefined");
    }

    // Shamelessly stolen from another script
    private int GetCurrentSceneIndex()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }
    public bool pauseMenuOpen()
    {
        return pauseMenuScript.isCanvasEnabled();
    }
}
