using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

// A FocusArea is a trigger box. 
// When the player enters the trigger area, the camera shifts its focus to it. 
// When the player exits the trigger area, the camera shifts its focus back to the player.
public class FocusArea : MonoBehaviour
{
    // Configurable parameters
    [SerializeField] [Tooltip("How far to zoom the camera relative to the focus area's size")] [Range(0, 2)]
    float zoomFactor = 0.8f;

    // Cached object references
    CameraBlender blender;

    private void Start() {
        blender = FindObjectOfType<CameraBlender>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player")) {
            // Unity camera size is 1/2 of its height
            float camSize = transform.lossyScale.y / 2 / zoomFactor;   
            blender.UpdateFocusCamera(transform.position, camSize);
            blender.Focus();
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.CompareTag("Player")) {
            blender.Unfocus();
        }
    }

}
