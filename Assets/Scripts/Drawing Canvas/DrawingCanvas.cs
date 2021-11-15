using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawingCanvas : MonoBehaviour
{
    // Cofiguration parameters
    [Header("Line")]
    [SerializeField] Line linePrefab;

    [Tooltip("The parent object that will hold all the drawn lines")]
    [SerializeField] GameObject linesParent;

    [Header("Line Drawing Settings")]
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


    // Begins adding points to a new line
    public void BeginDraw() {
        // Instantiate a new line
        currentLine = Instantiate(linePrefab, this.transform).GetComponent<Line>();

        // Make it a child of the drawer's "Lines" (for organization)
        currentLine.transform.parent = linesParent.transform;
    }

    // Adds a point to the line
    void Draw() {
        Vector2 canvasPosition = transform.position;
        Vector2 mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
        
        float lineRadius = currentLine.GetWidth() / 2f;

        RaycastHit2D hitCantDraw = Physics2D.CircleCast(mousePosition, lineRadius, Vector2.zero, 1f, cantDrawOverLayer);
        RaycastHit2D hitDrawingArea = Physics2D.CircleCast(mousePosition, lineRadius, Vector2.zero, 1f, LayerMask.GetMask("Drawing Area"));
        
        if (!hitCantDraw && hitDrawingArea)
            currentLine.AddPoint(mousePosition - canvasPosition);    // Account for canvas position
        else
            EndDraw();
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

    public Line GetLinePrefab()
    {
        return linePrefab;
    }
}
