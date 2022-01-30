using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    // Configuration parameters
    [Header("Object references")]
    [SerializeField] LevelTransition transition;
    [SerializeField] GameObject loadingAnimation;

    [Header("Configuration")]
    [SerializeField] int mainMenuBuildIndex = 0;
    [SerializeField] float minLoadingTime = 3f;

    // State variables
    bool isLoading = false;

    private void Start() 
    {
        // Hide the loading animation
        loadingAnimation.SetActive(false);    
    }

    // Loads the main menu
    public void LoadMainMenu()
    {
        LoadScene(mainMenuBuildIndex);
    }

    // Loads the next scene, or the main menu if there are no more scenes
    public void LoadNextScene()
    {
        // Get the next scene index
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        // Go to main menu if there are no more scenes
        if (nextSceneIndex >= SceneManager.sceneCountInBuildSettings)
            nextSceneIndex = mainMenuBuildIndex;

        LoadScene(nextSceneIndex);
    }

    // Reloads the current scene. Should be used for debugging, not in production.
    public void ReloadScene()
    {
        LoadScene(SceneManager.GetActiveScene().buildIndex);
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

        // Start the transition animation 
        yield return AnimateTransition();
        
        // Show the loading animation
        loadingAnimation.SetActive(true);

        // Begin loading the scene asynchronously
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(index, LoadSceneMode.Single);
        asyncLoad.allowSceneActivation = false;

        // Wait for the scene to finish loading before activating it
        yield return new WaitForSeconds(minLoadingTime);
        yield return new WaitUntil(() => asyncLoad.progress >= 0.9f);
        asyncLoad.allowSceneActivation = true;
    }

    // A coroutine that animates the level transition
    IEnumerator AnimateTransition()
    {
        Time.timeScale = 1f;
        yield return transition.FadeOut();
    }
}