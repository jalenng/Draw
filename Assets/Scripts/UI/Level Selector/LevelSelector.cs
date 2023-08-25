using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelector : MonoBehaviour
{
    // Object references
    SceneLoader sceneLoader;
    PersistentStoreManager storeManager;

    bool unlockAllLevels;

    [SerializeField] MenuManager menuHolder;
    
    // Configuration parameters
    // For dev/debug only
    [Header("The options below will only take effect in Debug mode")]
    [SerializeField] private bool unlockAllLevelsInDebug = false;

    void Start()
    {
        sceneLoader = FindObjectOfType<SceneLoader>();
        storeManager = FindObjectOfType<PersistentStoreManager>();

        if (Debug.isDebugBuild && unlockAllLevelsInDebug)
        {
            unlockAllLevels = true;
        }
    }

    public bool LevelReached(Global.Level level)
    {
        return unlockAllLevels || storeManager.QueryLevelReached(level);
    }

    public void LoadScene(Global.Level level)
    {
        if (LevelReached(level))
        {
            if (!Global.LevelToBuildIndexMap.TryGetValue(level, out int buildIndex)) {
                Debug.Log($"Tried to load scene {level} from levels menu, but the build index couldn't be found");
            }
            sceneLoader.LoadScene(buildIndex);
        }
        else
        {
            Debug.Log($"Tried to load scene {level} from levels menu, but the level has not been reached yet");
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
