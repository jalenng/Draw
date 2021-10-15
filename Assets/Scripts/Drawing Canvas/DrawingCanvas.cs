using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawingCanvas : MonoBehaviour
{
    // Cofiguration parameters
    [SerializeField] GameObject linePrefab;
    [SerializeField] GameObject linesParent;
    [SerializeField] LayerMask cantDrawOverLayer;
    [SerializeField] int minLineLength = 2;

    [Header ("Line Physics Properties")] 
    [SerializeField] bool affectedByGravity = false;
    [SerializeField] float lineMass = 6.0f;
    [SerializeField] float lineLinearDrag = 1.2f;
    [SerializeField] float lineAngularDrag = 0.4f;

    // Cached components
    Line currentLine;
    Camera cam;

    
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

    // Begins adding points to a new line
    void BeginDraw() {
        // Instantiate a new line
        currentLine = Instantiate(linePrefab, this.transform).GetComponent<Line>();

        // Make it a child of the drawer's "Lines" (for organization)
        currentLine.transform.parent = linesParent.transform;
    }

    // Adds a point to the line
    void Draw() {
        Vector2 canvasPosition = transform.position;
        Vector2 mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
        
        RaycastHit2D hit = Physics2D.CircleCast(mousePosition, currentLine.getWidth() / 2f, Vector2.zero, 1f, cantDrawOverLayer);

        if (hit)
            EndDraw();
        else
            currentLine.AddPoint(mousePosition - canvasPosition);    // Account for canvas position
    }

    // Stops adding points to the line to terminate the line
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
                Rigidbody2D currentLineRigidbody = currentLine.GetComponent<Rigidbody2D>();

                // If the line supposed to be affected by gravity
                if (affectedByGravity)
                {
                    // Set body type to dynamic
                    currentLineRigidbody.bodyType = RigidbodyType2D.Dynamic;

                    // Make the collision mode continuous.
                    // Yields more accurate but also more computationally expensive collision.
                    currentLineRigidbody.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
                    currentLineRigidbody.mass = lineMass;
                    currentLineRigidbody.drag = lineLinearDrag;
                    currentLineRigidbody.angularDrag = lineAngularDrag;
                    currentLineRigidbody.gravityScale = 1.0f;
                }

                // Else, set body type to static
                else
                {
                    currentLineRigidbody.bodyType = RigidbodyType2D.Static;
                }

                // Make currentLine null. We're done with this line.
                currentLine = null;
            }
        }
    }
}
