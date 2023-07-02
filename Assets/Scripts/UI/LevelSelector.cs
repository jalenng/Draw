using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class LevelSelector : MonoBehaviour
{
    // Buttons
    [Header("Buttons")]

    // Object references
    SceneLoader levelLoader;

    void Awake()
    {
        levelLoader = FindObjectOfType<SceneLoader>();
    }

    public void LoadScene(int sceneIndex)
    {
        levelLoader.LoadScene(sceneIndex);
    }

}
