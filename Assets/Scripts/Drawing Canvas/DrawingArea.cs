using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class DrawingArea : MonoBehaviour
{
    // Configuration parameters
    [SerializeField] DrawingCanvas drawingCanvas;
    [SerializeField] float outlineWidth = 0.3f;

    // Cached components
    LineRenderer lineRenderer;
    BoxCollider2D boxCollider;

    // State variables
    Color baseOutlineColor;

    private void Start() {
        lineRenderer = GetComponent<LineRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();

        baseOutlineColor = drawingCanvas.GetLinePrefab().GetLineColor();

        SetUpBorderOutline();
    }

    private void Update()
    {
        UpdateBorderOutlineColor();
    }

    private void SetUpBorderOutline()
    {
        // Get the collision box size
        Vector2 collisionBoundsSize = boxCollider.size;
        float halfWidth = collisionBoundsSize.x / 2f;
        float halfHeight = collisionBoundsSize.y / 2f;

        // Set the positions of the border line based on the collision box size
        lineRenderer.positionCount = 4;  // Drawing area is a rectangle (4 corners). Line drawn needs 5 vertices.

        Vector3[] positions = {
            new Vector3(halfWidth, halfHeight, 0),      // Top right
            new Vector3(halfWidth, -halfHeight, 0),     // Bottom right
            new Vector3(-halfWidth, -halfHeight, 0),    // Bottom left
            new Vector3(-halfWidth, halfHeight, 0),     // Top left
        };

        lineRenderer.SetPositions(positions);

        // Set outline width
        lineRenderer.startWidth = outlineWidth;
        lineRenderer.endWidth = outlineWidth;
    }

    private void UpdateBorderOutlineColor()
    {
        // Set outline color to match the color of the line.
        // The amount of remaining ink is represented by the alpha value.
        float inkRemaining = 1.0f - drawingCanvas.GetInkRatio();
        // Debug.Log(inkRemaining);
        Color outlineColor = new Color(baseOutlineColor.r, baseOutlineColor.g, baseOutlineColor.b, inkRemaining);
        lineRenderer.startColor = outlineColor;
        lineRenderer.endColor = outlineColor;
    }

    // Invoked when the player clicks inside the drawing area's collider
    private void OnMouseDown() {
        drawingCanvas.BeginDraw();
    }
    
}
