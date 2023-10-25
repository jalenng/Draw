using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ResetButtonPlacement
{
    TopLeft,
    TopRight,
    BottomLeft,
    BottomRight
}

[ExecuteAlways]
public class ResetButton : MonoBehaviour
{
    // Configuration parameters
    [SerializeField] private Vector3 offset;
    [SerializeField] private ResetButtonPlacement placement = ResetButtonPlacement.TopRight;

    // Object references
    [SerializeField] private DrawingCanvas drawingCanvas;
    [SerializeField] private DrawingArea drawingArea;

    // Cached components
    private AudioSource audioSrc;
    private Animator anim;
    private BoxCollider2D drawingAreaCollider;
    private CircleCollider2D resetButtonCollider;
    private SpriteRenderer spriteRenderer;
    private bool isEnabled = true;

    private void Start()
    {
        audioSrc = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        drawingAreaCollider = drawingArea.GetComponent<BoxCollider2D>();
        resetButtonCollider = GetComponent<CircleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        UpdateEnabledStatus();
        SetUpPosition();
    }

    // Retrieve the enabled status from the drawing canvas
    private void UpdateEnabledStatus()
    {
        isEnabled = drawingCanvas != null && drawingCanvas.resetButtonEnabled;
        Color disabledColor = new Color(0.75f, 0.75f, 0.75f, 1);
        spriteRenderer.color = isEnabled ? Color.black : disabledColor;
    }

    // Set up reset button position relative to the folder
    private void SetUpPosition()
    {
        // Get position of top-right corner of drawing area
        Vector3 drawingAreaLocalScale = drawingArea.transform.localScale;
        float drawingAreaHalfWidthScaled = drawingAreaLocalScale.x * (drawingAreaCollider.size.x / 2f);
        float drawingAreaHalfHeightScaled = drawingAreaLocalScale.y * (drawingAreaCollider.size.y / 2f);

        // Calculate the position of the top-right corner of the drawing area
        Vector3 positioning = new Vector3(
            drawingAreaHalfWidthScaled,
            drawingAreaHalfHeightScaled,
            0
        );

        // Calculate the offset from the top-right corner to result in the desired location
        float resetButtonRadius = resetButtonCollider.radius;
        Vector3 anchoringOffset = new Vector3(
            resetButtonRadius,
            -resetButtonRadius,
            0
        );

        // Adjust the placement offset based on the placement setting
        bool isLeft = placement == ResetButtonPlacement.TopLeft || placement == ResetButtonPlacement.BottomLeft;
        bool isBottom = placement == ResetButtonPlacement.BottomLeft || placement == ResetButtonPlacement.BottomRight;
        Vector3 placementOffset =
            Vector3.Scale(
                positioning + anchoringOffset,
                new Vector3(isLeft ? -1 : 1, isBottom ? -1 : 1, 1)
            );

        transform.position =
            // Start with the drawing area world space position
            drawingArea.transform.position +
            // Apply the offset in the drawing area space, then convert to world space
            drawingArea.transform.TransformDirection(placementOffset + offset);
    }

    private void OnMouseDown()
    {
        if (isEnabled && Time.timeScale > 0)
        {
            audioSrc.Play();
            drawingCanvas.Reset();
            anim.SetTrigger("Spin");
        }
    }

}
