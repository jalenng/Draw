using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoaderParamsConfig : ScriptableObject
{
    [Header("Build Indices")]
    public int mainMenuBuildIndex = 0;
    public int levelSelectorBuildIndex = 1;
    public int settingsBuildIndex = 2;
    public int gameStartBuildIndex = 3;

    [Header("Timing")]
    public float minLoadingTime = 3f;
}
