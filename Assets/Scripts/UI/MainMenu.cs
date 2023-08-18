using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("Game Objects")]
    public GameObject loadButton;
    public GameObject playConfirmation;

    SceneLoader levelLoader;
    GameManager gameManager;

    private bool canLoad;

    void Start()
    {
        // Only enable the canvas if we're in the Main Menu Level. I don't really wanna disable the main
        // Menu in each level.
        levelLoader = FindObjectOfType<SceneLoader>();
        gameManager = FindObjectOfType<GameManager>();    // GameManager is a singleton

        canLoad = gameManager.CanLoad();

        // Show the Load button only if there is a game save
        loadButton.SetActive(canLoad);
    }

    public void RequestPlay()
    {
        // If there is an existing game save, prompt for confirmation
        if (canLoad) {
            playConfirmation.SetActive(true);
        }
        else {
            Play();
        }
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
