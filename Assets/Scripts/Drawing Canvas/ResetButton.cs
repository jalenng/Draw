using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetButton : MonoBehaviour
{
    // Cached components and object references
    public DrawingCanvas drawingCanvas;
    public DrawingArea drawingArea;

    private void Start() {
        SetUpPosition();
    }

    // Set up reset button position relative to the folder
    private void SetUpPosition()
    {        
        // Get position of top-right corner of drawing area
        BoxCollider2D drawingAreaCollider = drawingArea.GetComponent<BoxCollider2D>();
        float drawingAreaHalfWidth = drawingAreaCollider.size.x / 2f;
        float drawingAreaHalfHeight = drawingAreaCollider.size.y / 2f;

        // Calculate the offset to result in the desired location
        BoxCollider2D resetButtonCollider = GetComponent<BoxCollider2D>();
        float resetButtonHalfWidth = resetButtonCollider.size.x / 2f;
        float resetButtonHalfHeight = resetButtonCollider.size.y / 2f;

        Vector3 drawingAreaLocalScale = drawingArea.transform.localScale;

        Vector3 offset = new Vector3(
            (drawingAreaLocalScale.x * drawingAreaHalfWidth) + resetButtonHalfWidth,
            (drawingAreaLocalScale.y * drawingAreaHalfHeight) - resetButtonHalfHeight
            );

        Vector3 trc = drawingArea.transform.position + offset;
        transform.position = trc;
    }

    private void OnMouseDown() {
        drawingCanvas.Reset();
    }

}
