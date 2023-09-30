using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroupVisibility))]
public class PauseMenu : MonoBehaviour
{
    private CanvasGroupVisibility pauseMenuCanvas;

    // State variables
    bool paused = false;    // Whether the game is paused
    
    void Start() {
        pauseMenuCanvas = GetComponent<CanvasGroupVisibility>();
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
        paused = true;
        Time.timeScale = 0;
        pauseMenuCanvas.SetVisibility(true);
    }

    // Resume the game
    public void Resume()
    {
        paused = false;
        Time.timeScale = 1;
        pauseMenuCanvas.SetVisibility(false);
    }

    // Transition to the main menu with an animation
    public void LoadMainMenu()
    {
        // Save the game
        FindObjectOfType<GameManager>()?.Save();

        // Resume the game
        Resume();

        // Load the main menu with the level manager
        FindObjectOfType<SceneLoader>().LoadMainMenu();
    }
}
