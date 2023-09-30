using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LevelEndTrigger : MonoBehaviour
{
    [SerializeField] private LayerMask triggerableLayers;
    private bool hasTriggered;

    void Start()
    {
        hasTriggered = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        bool layerIsTriggerable = triggerableLayers == (triggerableLayers | (1 << other.gameObject.layer));
        if (!hasTriggered && layerIsTriggerable)
        {
            LoadNextScene();
        }
    }

    [ContextMenu("Load Next Scene")]
    public void LoadNextScene()
    {
        FindObjectOfType<SceneLoader>().LoadNextScene();
        FindObjectOfType<PlayerMovement>()?.SetCanMove(0);
        hasTriggered = true;
    }
}
