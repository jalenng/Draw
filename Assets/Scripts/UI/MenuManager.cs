using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;

public class MenuManager : MonoBehaviour
{
    [SerializeField] CanvasGroupVisibility settingsMenu;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] CanvasGroupVisibility levelMenu;
    [SerializeField] GameObject sendFeedbackCanvas;
    PauseMenu pauseMenuScript;

    private void Start()
    {
        // Show the "Feedback" canvas only if it is a debug or beta build
        string lowercaseVersion = Application.version.ToLower();
        string[] betaSuffixes = { "beta", "b" };
        bool isBeta = betaSuffixes.Any((suffix) => lowercaseVersion.EndsWith(suffix));

        bool showFeedbackButton = Debug.isDebugBuild || isBeta;
        sendFeedbackCanvas?.SetActive(showFeedbackButton);
    }

    // Public functions for OnClick() to call
    public void enableSettings(bool enable)
    {
        if (settingsMenu)
            settingsMenu.SetVisibility(enable);
        else
            Debug.LogError($"[MenuManager] Tried to set settings menu enabled to {enable} but it's undefined");
    }
    public void enablePause(bool enable)
    {
        if (pauseMenu)
        {
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            if (Global.GetLevelFromBuildIndex(currentSceneIndex) != Global.Level.NONE)
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
            levelMenu.SetVisibility(enable);
        else
            Debug.LogError($"[MenuManager] Tried to set level menu enabled to {enable} but it's undefined");
    }
}
