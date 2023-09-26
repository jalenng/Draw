using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    // Configuration parameters
    [Header("Object references")]
    [SerializeField] SceneTransition transition;

    // State variables
    bool isLoading = false;
    private AudioSource audioSource;

    // Object references
    PersistentStoreManager storeManager;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        storeManager = FindObjectOfType<PersistentStoreManager>();

        StartCoroutine(handleSceneEntrance());
    }
    // Load the first level
    public void StartGame()
    {
        Global.Level gameStartLevel = Global.Level.IRL_DRAWING;
        bool buildIndexFound = Global.LevelToBuildIndexMap.TryGetValue(gameStartLevel, out int buildIndex);
        if (buildIndexFound) {
            LoadScene(buildIndex);
        }
    }

    // Loads the main menu
    public void LoadMainMenu()
    {        
        Global.UIScene mainMenuScene = Global.UIScene.MAIN_MENU;
        bool buildIndexFound = Global.UISceneToBuildIndexMap.TryGetValue(mainMenuScene, out int buildIndex);
        if (buildIndexFound) {
            LoadScene(buildIndex);
        }
    }

    // Loads the next scene, or the main menu if there are no more scenes
    public void LoadNextScene()
    {
        // Get the next scene index
        int currentSceneIndex = GetCurrentSceneIndex();
        int nextSceneIndex = currentSceneIndex + 1;
        audioSource.Play();
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
        // Ensure only one scene gets loaded at a time
        if (isLoading)
            yield break;

        isLoading = true;
        yield return LoadAndEnterScene(index);

        // We should be in the new scene now
        yield return handleSceneEntrance();
        isLoading = false;
    }

    private IEnumerator LoadAndEnterScene(int index)
    {
        // Disable the pause menu in the current scene
        FindObjectOfType<MenuManager>()?.enablePause(false);

        // Set the transition color based on the level index.
        bool loadingToUIScene = Global.GetUISceneFromBuildIndex(index) != Global.UIScene.NONE;
        Color transitionColor = loadingToUIScene
            ? Color.white   // White for transitioning to UI
            : Color.black;  // Black for transitioning to a level

        transition.SetTransitionColor(transitionColor);

        // Start the transition animation 
        Time.timeScale = 1f;
        yield return transition.FadeOut();

        // Begin loading the scene asynchronously
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(index, LoadSceneMode.Single);
        asyncLoad.allowSceneActivation = false;

        // Wait for the scene to finish loading
        yield return new WaitUntil(() => asyncLoad.progress >= 0.9f);
        
        // Activate the scene
        asyncLoad.allowSceneActivation = true;
        yield return new WaitUntil(() => asyncLoad.isDone);
    }

    private IEnumerator handleSceneEntrance()
    {
        TrySetLevelReached(GetCurrentSceneIndex());

        // Start the transition animation
        yield return transition.FadeIn();

        // Enable the pause menu in the new scene
        FindObjectOfType<MenuManager>()?.enablePause(true);
    }

    // Returns the current scene index
    private int GetCurrentSceneIndex()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }

    // Try to mark the current scene as a reached level
    private bool TrySetLevelReached(int buildIndex)
    {
        Global.Level level = Global.GetLevelFromBuildIndex(buildIndex);
        if (level != null)
        {
            storeManager.AddLevelReached(level);
            return true;
        }
        else
        {
            return false;
        }
    }
}