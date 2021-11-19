using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseSystem : MonoBehaviour
{   
    // Configuration parameters
    [SerializeField] GameObject pauseMenuCanvas;

    // State variables
    bool paused = false;    // Whether the game is paused or not
    bool canOpen = true;    // Whether the pause menu can be opened or not

    // Start is called before the first frame update
    void Start()
    {
        pauseMenuCanvas.SetActive(false);
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
        pauseMenuCanvas.SetActive(true);
    }

    // Resume the game
    public void Resume()
    {
        paused = false;
        Time.timeScale = 1;
        pauseMenuCanvas.SetActive(false);
    }
    
    // Transition to the main menu with an animation
    public void LoadMainMenu()
    {  
        // Don't allow the player to open the pause menu
        canOpen = false;

        // Resume the game
        Resume();

        // Update the fade-out color to white to match the main menu's fade-in color
        FindObjectOfType<LevelTransition>().SetFadeOutColor(Color.white);

        // Load the main menu with the level manager
        FindObjectOfType<LevelLoader>().LoadMainMenu();
    }

}
