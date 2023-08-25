using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LevelEndTrigger : MonoBehaviour
{
    private bool triggered;
    void Start() {
        triggered = false;
    }
    private void OnTriggerEnter2D(Collider2D other) {
        LoadNextScene();
    }

    public void LoadNextScene() {
        if(!triggered) {
            FindObjectOfType<SceneLoader>().LoadNextScene();
            triggered = true;
        }
    }
}
