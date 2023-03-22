using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LevelEndTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) {
        LoadNextScene();
    }

    public void LoadNextScene() {
        FindObjectOfType<SceneLoader>().LoadNextScene();
    }
}
