using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class ResetButton : MonoBehaviour
{
    // Configuration parameters
    [SerializeField] private Vector3 offset;

    // Cached components and object references
    public DrawingCanvas drawingCanvas;
    public DrawingArea drawingArea;
    public AudioSource audio;

    private BoxCollider2D drawingAreaCollider;
    private BoxCollider2D resetButtonCollider;

    private void Start()
    {
        audio = GetComponent<AudioSource>();
        drawingAreaCollider = drawingArea.GetComponent<BoxCollider2D>();
        resetButtonCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        SetUpPosition();
    }

    // Set up reset button position relative to the folder
    private void SetUpPosition()
    {
        // Get position of top-right corner of drawing area
        float drawingAreaHalfWidth = drawingAreaCollider.size.x / 2f;
        float drawingAreaHalfHeight = drawingAreaCollider.size.y / 2f;
        Vector3 drawingAreaLocalScale = drawingArea.transform.localScale;
        Vector3 positioning = new Vector3(
            (drawingAreaLocalScale.x * drawingAreaHalfWidth),
            (drawingAreaLocalScale.y * drawingAreaHalfHeight),
            0
        );

        // Calculate the offset to result in the desired location
        float resetButtonHalfWidth = resetButtonCollider.size.x / 2f;
        float resetButtonHalfHeight = resetButtonCollider.size.y / 2f;
        Vector3 anchoringOffset = new Vector3(
            resetButtonHalfWidth,
            -resetButtonHalfHeight,
            0
        );
        
        transform.position = drawingArea.transform.position + positioning + anchoringOffset + offset; ;
    }

    private void OnMouseDown()
    {
        if (Time.timeScale > 0)
        {
            audio.Play();
            drawingCanvas.Reset();
        }
    }

}
