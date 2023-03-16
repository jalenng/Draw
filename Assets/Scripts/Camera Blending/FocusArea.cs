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
    bool isLocked = false;

    // Cached object references
    CameraBlender blender;

    private void Start() {
        blender = FindObjectOfType<CameraBlender>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (isLocked)
            return;
        if(other.CompareTag("Player")) {
            Focus();
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (isLocked)
            return;
        if(other.CompareTag("Player"))
            Unfocus();
    }

    private void Focus() {
        // Unity camera size is 1/2 of its height
        float camSizeByHeight = transform.lossyScale.y / 2;
        float camSizeByWidth = transform.lossyScale.x / 3;  // 3:2 ratio
        float camSize = Mathf.Max(camSizeByHeight, camSizeByWidth) / zoomFactor;
        
        blender.Focus(transform.position, camSize);
    }

    private void Unfocus() {
        blender.Unfocus();
    }

    public void UpdateArea(int x)
    {
        transform.localScale = new Vector3(x, transform.localScale.y, transform.localScale.z);
        Focus();
    }

    public void SetLock(bool locked)
    {
        isLocked = locked;
        if (isLocked)
            Focus();
        else
            Unfocus();
    }
}
