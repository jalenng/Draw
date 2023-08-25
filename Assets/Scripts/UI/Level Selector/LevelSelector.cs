using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelector : MonoBehaviour
{
    // Object references
    SceneLoader levelLoader;
    GameManager gameManager;

    bool unlockAllLevels;

    [SerializeField] MenuManager menuHolder;
    
    // Configuration parameters
    // For dev/debug only
    [Header("The options below will only take effect in Debug mode")]
    [SerializeField] private bool unlockAllLevelsInDebug = false;

    void Start()
    {
        levelLoader = FindObjectOfType<SceneLoader>();
        gameManager = FindObjectOfType<GameManager>();    // GameManager is a singleton

        if (Debug.isDebugBuild && unlockAllLevelsInDebug)
        {
            unlockAllLevels = true;
        }
    }

    public bool LevelReached(int sceneIndex)
    {
        return unlockAllLevels || gameManager.LevelReached(sceneIndex);
    }

    public void LoadScene(int sceneIndex)
    {
        if (LevelReached(sceneIndex))
        {
            levelLoader.LoadScene(sceneIndex);
        }
        else
        {
            Debug.Log($"Tried to load scene {sceneIndex} from levels menu, but the level has not been reached yet");
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            menuHolder.enableLevels(false);
        }
    }
}
