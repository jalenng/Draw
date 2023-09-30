using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
public class DrawingArea : MonoBehaviour
{
    // Configuration parameters
    [SerializeField] DrawingCanvas drawingCanvas;
    [Range(0, 1)]
    [SerializeField] private float startOpacity = 1f;
    [Range(0, 1)]
    [SerializeField] private float endOpacity = 0f;

    // Cached components
    LineRenderer lineRenderer;
    BoxCollider2D boxCollider;
    CursorManager cursorManager;
    MenuManager menuManager;

    // State variables
    Color baseOutlineColor;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();

        baseOutlineColor = drawingCanvas.GetLinePrefab().GetLineColor();

        cursorManager = FindObjectOfType<CursorManager>();
        menuManager = FindObjectOfType<MenuManager>();

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
    }

    private void UpdateBorderOutlineColor()
    {
        // Set outline color to match the color of the line.
        // The amount of remaining ink is represented by the alpha value.
        float inkRemaining = Mathf.Lerp(startOpacity, endOpacity, drawingCanvas.GetInkRatio());
        Color outlineColor = new Color(baseOutlineColor.r, baseOutlineColor.g, baseOutlineColor.b, inkRemaining);
        lineRenderer.startColor = outlineColor;
        lineRenderer.endColor = outlineColor;
    }

    // Update cursor conditionally when the mouse enters/exits the drawing area
    private void OnMouseEnter()
    {
        if (drawingCanvas.CanDraw())
        {
            cursorManager.EnableCursor("pencil");
        }
    }

    private void OnMouseExit()
    {
        cursorManager.DisableCursor("pencil");
    }

    // Invoked when the player clicks inside the drawing area's collider
    private void OnMouseDown()
    {
        // Stops from drawing when pauseMenu enabled.
        // To do: Find better solution to this. Since our game is small, this is fine since it'll be like... 10 max Drawing Areas that need to initialize
        // a ref to PauseMenu. 
        if (Time.timeScale > 0) drawingCanvas.BeginDraw();
    }
}
