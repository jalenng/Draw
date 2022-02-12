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
    [SerializeField] int mainMenuBuildIndex = 0;
    [SerializeField] int settingsBuildIndex = 1;
    [SerializeField] int gameStartBuildIndex = 2;
    [SerializeField] float minLoadingTime = 3f;

    // State variables
    bool isLoading = false;

    // Load the first level
    public void StartGame()
    {
        LoadScene(gameStartBuildIndex);
    }

    // Loads the main menu
    public void LoadMainMenu()
    {
        LoadScene(mainMenuBuildIndex);
    }

    // Loads the settings
    public void LoadSettings()
    {
        LoadScene(settingsBuildIndex);
    }

    // Loads the next scene, or the main menu if there are no more scenes
    public void LoadNextScene()
    {
        // Get the next scene index
        int currentSceneIndex = GetCurrentSceneIndex();
        int nextSceneIndex = currentSceneIndex + 1;

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

        // Set the transition color based on the level index
        Color transitionColor = Color.black;    // Black for transitioning to a level
        if (index == mainMenuBuildIndex || index == settingsBuildIndex)
            transitionColor = Color.white;      // White for transitioning to a menu
        transition.SetTransitionColor(transitionColor);

        // Start the transition animation 
        Time.timeScale = 1f;
        yield return transition.FadeOut();

        // Begin loading the scene asynchronously
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(index, LoadSceneMode.Single);
        asyncLoad.allowSceneActivation = false;

        // Use the minimum loading time only if we're not navigating from or to the settings menu
        if (index != settingsBuildIndex && GetCurrentSceneIndex() != settingsBuildIndex)
            yield return new WaitForSeconds(minLoadingTime);    

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