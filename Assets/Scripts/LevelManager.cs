using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public Animator levelTransitionAnimator;

    bool isLoading = false;

    public void LoadNextScene()
    {
        // Ensure only one scene gets loaded
        if (isLoading) return;

        isLoading = true;
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        StartCoroutine(BeginTransition(currentSceneIndex + 1));
    }

    IEnumerator BeginTransition(int index)
    {
        // Begin loading the scene asynchronously
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(index, LoadSceneMode.Single);
        asyncLoad.allowSceneActivation = false;

        // Start fading to black
        Time.timeScale = 1f;
        levelTransitionAnimator.SetTrigger("end");

        // Wait for given amount of time
        yield return new WaitForSeconds(1.5f);

        // Wait for scene to finish loading before activation
        while (asyncLoad.progress < 0.9f)
        {
            yield return null;
        }
        asyncLoad.allowSceneActivation = true;
    }
}