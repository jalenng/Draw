using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContentWarning : MonoBehaviour
{
    SceneLoader sceneLoader;

    void Start()
    {
        sceneLoader = FindObjectOfType<SceneLoader>();
    }

    public void Proceed() 
    {
        Debug.Log("[ContentWarning] Proceeding into the game");
        sceneLoader.LoadNextScene();
    }

    public void Quit()
    {
        Debug.Log("[ContentWarning] Quitting the game");
        Application.Quit();
    }
}
