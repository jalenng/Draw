using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectorButton : MonoBehaviour
{
    // Configuration parameters
    [SerializeField] public int buildIndex;

    // References
    [SerializeField] public LevelSelector levelSelector;

    // Start is called before the first frame update
    void Start()
    {
        // Enable the button only if there exists a game save that indicates
        // the level has already been reached in the past
        bool levelReached = levelSelector.LevelReached(buildIndex);
        GetComponent<Button>().interactable = levelReached;
    }

    public void OnClick()
    {
        levelSelector.LoadScene(buildIndex);
    }
}
