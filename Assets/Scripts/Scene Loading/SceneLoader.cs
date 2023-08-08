using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    // Configuration parameters
    [Header("Object references")]
    [SerializeField] SceneTransition transition;

    [Header("Configuration")]
    [SerializeField] SceneLoaderParamsConfig config;

    // State variables
    bool isLoading = false;
    public AudioSystem audSysSound;

    void Start() {
        audSysSound = AudioSystem.audioPlayer;
    }
    // Load the first level
    public void StartGame()
    {
        LoadScene(config.gameStartBuildIndex);
    }

    // Loads the main menu
    public void LoadMainMenu()
    {
        LoadScene(config.mainMenuBuildIndex);
    }

    // Loads the level selector
    public void LoadLevelSelector()
    {
        LoadScene(config.levelSelectorBuildIndex);
    }

    // Loads the settings
    public void LoadSettings()
    {
        LoadScene(config.settingsBuildIndex);
    }

    // Loads the next scene, or the main menu if there are no more scenes
    public void LoadNextScene()
    {
        // Get the next scene index
        int currentSceneIndex = GetCurrentSceneIndex();
        int nextSceneIndex = currentSceneIndex + 1;
        audSysSound.PlaySFX("pageflip");
        // Go to main menu if there are no more scenes
        if (nextSceneIndex >= SceneManager.sceneCountInBuildSettings)
            LoadMainMenu();
        else
            LoadScene(nextSceneIndex);
    }

    // Reloads the current scene. Should be used for debugging, not in production.
    public void ReloadScene()
    {
        int currentSceneIndex = GetCurrentSceneIndex();
        LoadScene(currentSceneIndex);
    }

    // Loads a scene
    public void LoadScene(int index)
    {
        StartCoroutine(LoadSceneRoutine(index));
    }

    // A coroutine that loads a given scene
    IEnumerator LoadSceneRoutine(int index)
    {
        // Ensure only one scene gets loaded
        if (isLoading)
            yield break;
        isLoading = true;

        // Set the transition color based on the level index.
        bool loadingToMenu = index == config.mainMenuBuildIndex || index == config.settingsBuildIndex || index == config.levelSelectorBuildIndex;

        Color transitionColor = loadingToMenu
            ? Color.white   // White for transitioning to a menu
            : Color.black;  // Black for transitioning to a level

        transition.SetTransitionColor(transitionColor);

        // Start the transition animation 
        Time.timeScale = 1f;
        yield return transition.FadeOut();

        // Begin loading the scene asynchronously
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(index, LoadSceneMode.Single);
        asyncLoad.allowSceneActivation = false;

        // Use the minimum loading time only if we're not navigating from or to the settings menu
        if (index != config.settingsBuildIndex && GetCurrentSceneIndex() != config.settingsBuildIndex)
            yield return new WaitForSeconds(config.minLoadingTime);    

        // Wait for the scene to finish loading before activating it
        yield return new WaitUntil(() => asyncLoad.progress >= 0.9f);
        asyncLoad.allowSceneActivation = true;

        // Start the transition animation 
        yield return transition.FadeIn();

        isLoading = false;
    }

    // Returns the current scene index
    private int GetCurrentSceneIndex()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }

}