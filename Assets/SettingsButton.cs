using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsButton : MonoBehaviour
{
    [SerializeField] GameObject settingsMenu;
    [SerializeField] GameObject PauseMenu;

    public void enableMenu() {
        settingsMenu.SetActive(true);
        //PauseMenu.GetComponent<PauseMenu>().enableCanvas(false);
    }
}
