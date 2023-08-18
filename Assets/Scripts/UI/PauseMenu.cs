using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    // Configuration parameters
    public GameObject PauseMenuCanvas;

    // State variables
    bool paused = false;    // Whether the game is paused
    bool canPause = true;    // Whether the game can be paused

    void Start() {
        PauseMenuCanvas.SetActive(false);
    }
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (paused)
                Resume();
            else
                Pause();
        }
    }

    // Pause the game    
    public void Pause()
    {
        if (!canPause)
            return;
        paused = true;
        Time.timeScale = 0;
        PauseMenuCanvas.SetActive(true);
    }

    // Resume the game
    public void Resume()
    {
        paused = false;
        Time.timeScale = 1;
        PauseMenuCanvas.SetActive(false);
    }

    // Transition to the main menu with an animation
    public void LoadMainMenu()
    {
        // Save the game
        FindObjectOfType<GameManager>()?.Save();

        // Don't allow the player to open the pause menu
        canPause = false;

        // Resume the game
        Resume();

        // Load the main menu with the level manager
        FindObjectOfType<SceneLoader>().LoadMainMenu();
    }
}
