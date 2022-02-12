using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LevelEndTrigger : MonoBehaviour
{
    [SerializeField] UnityEvent onLevelEnd;

    private void OnTriggerEnter2D(Collider2D other) {
        onLevelEnd.Invoke();
    }
}
