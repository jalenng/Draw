using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour
{
    [Header("Game Objects")]
    public GameObject playButton;
    public GameObject loadButton;
    public GameObject playConfirmation;
    public GameObject preEndBackground;
    public GameObject postEndBackground;

    SceneLoader levelLoader;
    GameManager gameManager;
    PersistentStoreManager storeManager;

    private bool canLoad;

    void Start()
    {
        levelLoader = FindObjectOfType<SceneLoader>();
        gameManager = FindObjectOfType<GameManager>();
        storeManager = FindObjectOfType<PersistentStoreManager>();

        // Show the Load button only if there is a game save
        canLoad = gameManager.CanLoad();
        loadButton.SetActive(canLoad);

        // Show the background variation if the player has completed the game.
        bool hasCompletedGame = storeManager.QueryLevelReached(Global.Level.THE_END);
        preEndBackground.SetActive(!hasCompletedGame);
        postEndBackground.SetActive(hasCompletedGame);

        EventSystem.current.SetSelectedGameObject(playButton);
    }

    public void RequestPlay()
    {
        // If there is an existing game save, prompt for confirmation
        if (canLoad)
        {
            playConfirmation.GetComponent<CanvasGroupVisibility>().SetVisibility(true);
        }
        else
        {
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
        Debug.Log("[MainMenu] Quitting the game");
        Application.Quit();
    }
}
