using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEndTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) {
        FindObjectOfType<LevelManager>().LoadNextScene();
    }
}
