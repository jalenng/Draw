using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelector : MonoBehaviour
{
    // Buttons
    [Header("Buttons")]

    // Object references
    SceneLoader levelLoader;
    [SerializeField] MenuManager menuHolder;

    void Awake()
    {
        levelLoader = FindObjectOfType<SceneLoader>();
    }

    public void LoadScene(int sceneIndex)
    {
        levelLoader.LoadScene(sceneIndex);
    }
    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            menuHolder.enableLevels(false);
        }
    }
}
