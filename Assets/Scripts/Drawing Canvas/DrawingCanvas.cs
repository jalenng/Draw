using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawingCanvas : MonoBehaviour
{
    public GameObject linePrefab;
    public GameObject linesParent;
    public LayerMask cantDrawOverLayer;
    int cantDrawOverLayerIndex;

    [Header ("Line Properties")] 
    public bool affectedByGravity = false;
    public Gradient lineColor;
    public float linePointsMinDistance = 0.05f;
    public float lineWidth = 0.05f;
    public int minLineLength = 2;

    Line currentLine;
    Camera cam;

    Collider2D canvasCollider;

    void Start() {
        cam = Camera.main;
    }

    void Update() {
        // If current line exists, draw
        if (currentLine != null)
            Draw();

        // If the mouse button is released, end drawing
        if (Input.GetMouseButtonUp(0))
            EndDraw();
    }

    // Invoked when the player clicks inside the canvas's collider
    private void OnMouseDown() {
        BeginDraw();
    }

    // Begin Draw ----------------------------------------------
    void BeginDraw() {
        // Instantiate a new line
        currentLine = Instantiate(linePrefab, this.transform).GetComponent<Line>();

        // Make it a child of the drawer's "Lines" (for organization)
        currentLine.transform.parent = linesParent.transform;
        
        // Set line properties
        currentLine.SetLineColor(lineColor);
        currentLine.SetPointsMinDistance(linePointsMinDistance);
        currentLine.SetLineWidth(lineWidth);

    }

    void Draw() {
        Vector2 canvasPosition = transform.position;
        Vector2 mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
        
        RaycastHit2D hit = Physics2D.CircleCast(mousePosition, lineWidth / 3f, Vector2.zero, 1f, cantDrawOverLayer);

        if (hit)
            EndDraw();
        else
            currentLine.AddPoint(mousePosition - canvasPosition);    // Account for canvas position
    }

    void EndDraw () {
        if (currentLine != null) {
            
            // If the line is too short, ignore the line that was just drawn.
            if (currentLine.pointsCount < minLineLength) 
            {
                Destroy (currentLine.gameObject);
            } 
            else 
            {
                // Make the line dynamic (affected by gravity) if desired.
                if (affectedByGravity)
                    currentLine.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;

                // Make currentLine null. We're done with this line.
                currentLine = null;
            }
        }
    }
}
