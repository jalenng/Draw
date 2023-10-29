using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

// A FocusArea is a trigger box. 
// When the player enters the trigger area, the camera shifts its focus to it. 
// When the player exits the trigger area, the camera shifts its focus back to the player.

[ExecuteAlways]
public class FocusArea : MonoBehaviour
{
    [SerializeField] BoxCollider2D boxCollider;
    [SerializeField] LineRenderer visibleAreaLineRenderer;

    // Configurable parameters
    [SerializeField]
    [Tooltip("How far to zoom the camera relative to the focus area's size")]
    [Range(0, 2)]
    float zoomFactor = 0.8f;
    private bool isLocked = false;
    private bool playerEntered = false;

    // Cached object references
    CameraBlender blender;

    private void Start()
    {
        blender = FindObjectOfType<CameraBlender>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Only player can trigger
        if (other.CompareTag("Player"))
        {
            playerEntered = true;
            if (!isLocked) Focus();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Only player can trigger
        if (other.CompareTag("Player"))
        {
            playerEntered = false;
            if (!isLocked) Unfocus();
        }
    }

    private void Update()
    {
        SetUpVisibleAreaBorderOutline();
    }

    // Set up the line renderer for the visible area outline based on zoomFactor
    private void SetUpVisibleAreaBorderOutline()
    {
        // Get the collision box size
        Vector2 collisionBoundsSize = boxCollider.size;

        float triggerWidth = transform.localScale.x;
        float triggerHeight = transform.localScale.y;
        float triggerAspectRatio = triggerWidth / triggerHeight;
        float width;
        float height;
        if (triggerAspectRatio >= 1.5f)
        {
            width = 1;
            height = triggerWidth / triggerHeight / 1.5f;
        }
        else
        {
            width = triggerHeight * 1.5f / triggerWidth;
            height = 1;
        }
        float halfWidth = width / 2f / zoomFactor;
        float halfHeight = height / 2f / zoomFactor;

        // Set the positions of the border line based on the collision box size
        visibleAreaLineRenderer.positionCount = 4;  // Drawing area is a rectangle (4 corners). Line drawn needs 5 vertices.

        Vector3[] positions = {
            new Vector3(halfWidth, halfHeight, 0),      // Top right
            new Vector3(halfWidth, -halfHeight, 0),     // Bottom right
            new Vector3(-halfWidth, -halfHeight, 0),    // Bottom left
            new Vector3(-halfWidth, halfHeight, 0),     // Top left
        };

        visibleAreaLineRenderer.SetPositions(positions);

        // Set outline width
        visibleAreaLineRenderer.startWidth = 0.1f;
        visibleAreaLineRenderer.endWidth = 0.1f;
    }

    public void Focus()
    {
        // Unity camera size is 1/2 of its height
        float camSizeByHeight = transform.lossyScale.y / 2;
        float camSizeByWidth = transform.lossyScale.x / 3;  // 3:2 ratio
        float camSize = Mathf.Max(camSizeByHeight, camSizeByWidth) / zoomFactor;

        blender.Focus(transform.position, camSize);
    }

    public void Unfocus()
    {
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
        else if (!playerEntered)
            Unfocus();
    }
}
