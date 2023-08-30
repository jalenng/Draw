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

    void Awake()
    {
        sceneLoader = FindObjectOfType<SceneLoader>();
        storeManager = FindObjectOfType<PersistentStoreManager>();
    }

    void OnEnable()
    {
        // Debug build only:
        // If left shift is held while the level selector opens, all leves are unlocked
        unlockAllLevels = Debug.isDebugBuild && Input.GetKey(KeyCode.LeftShift);
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
                Debug.LogError($"[LevelSelector] Tried to load scene {level} from levels menu, but the build index couldn't be found");
            }
            sceneLoader.LoadScene(buildIndex);
        }
        else
        {
            Debug.LogError($"[LevelSelector] Tried to load scene {level} from levels menu, but the level has not been reached yet");
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
