using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    // Buttons
    [Header("Buttons")]
    public Button loadButton;

    SceneLoader levelLoader;
    GameManager gameManager;
    void Start()
    {
        // Only enable the canvas if we're in the Main Menu Level. I don't really wanna disable the main
        // Menu in each level.
        levelLoader = FindObjectOfType<SceneLoader>();
        gameManager = FindObjectOfType<GameManager>();    // GameManager is a singleton

        // Make the Load button interactable only if there is a game save
        bool canLoad = gameManager.CanLoad();
        loadButton.interactable = canLoad;
    }

    public void Play()
    {
        levelLoader.StartGame();
    }

    public void Load()
    {
        gameManager.Load();
    }

    public void Quit()
    {
        Debug.Log("Quitting the game");
        Application.Quit();
    }
}
