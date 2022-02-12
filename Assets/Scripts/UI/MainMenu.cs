using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    // Buttons
    [Header("Buttons")]
    public Button playButton;
    public Button loadButton;
    public Button settingsButton;
    public Button quitButton;

    SceneLoader levelLoader;
    GameManager gameManager;

    void Start()
    {
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

    public void Settings()
    {
        levelLoader.LoadSettings();
    }

    public void Quit()
    {
        Debug.Log("Quitting the game");
        Application.Quit();
    }

}
