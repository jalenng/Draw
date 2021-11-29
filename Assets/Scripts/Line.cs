using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{
    // Configuration parameters
    [Header("Appearance")]
    [SerializeField] float lineWidth = 0.05f;
    [SerializeField] Color lineColor = Color.black;
    [SerializeField] float pointsMinDistance = 0.1f; // The minimum distance between line's points.
    
    [Header("Physics")]
    [SerializeField] bool affectedByGravity = false;
    [SerializeField] float massPerPoint = 0.1f;
    [SerializeField] float linearDrag = 1.2f;
    [SerializeField] float angularDrag = 0.4f;
    [SerializeField] float gravityScale = 1.0f;

    // Cached components
    LineRenderer lineRenderer;
    EdgeCollider2D edgeCollider;
    Rigidbody2D rb2d;

    // State variables
    [HideInInspector] public List<Vector2> points = new List<Vector2>();
    [HideInInspector] public int pointsCount = 0;
    float circleColliderRadius; // A circle collider is added to each line point

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        edgeCollider = GetComponent<EdgeCollider2D>();
        rb2d = GetComponent<Rigidbody2D>();

        // Make the collision mode continuous.
        // Yields more accurate but also more computationally expensive collision.
        rb2d.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        
        // Update the other physics properties
        rb2d.drag = linearDrag;
        rb2d.angularDrag = angularDrag;
        rb2d.gravityScale = gravityScale;
        rb2d.mass = 0;
    }

    void Start()
    {
        // Set line width
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;

        // Set line color
        lineRenderer.startColor = lineColor;
        lineRenderer.endColor = lineColor;

        // Set circle collider and edge collider's radius
        circleColliderRadius = lineWidth / 2.0f;
        edgeCollider.edgeRadius = circleColliderRadius;
    }

    // Adds a point to the line
    public void AddPoint(Vector2 newPoint)
    {
        // If distance between last point and new point is less than pointsMinDistance, do nothing (return)
        if (pointsCount >= 1 && Vector2.Distance(newPoint, GetLastPoint()) < pointsMinDistance)
            return;

        points.Add(newPoint);
        pointsCount++;

        // Add Circle Collider to the Point
        CircleCollider2D circleCollider = this.gameObject.AddComponent<CircleCollider2D>();
        circleCollider.offset = newPoint;
        circleCollider.radius = circleColliderRadius;

        // Line Renderer
        lineRenderer.positionCount = pointsCount;
        lineRenderer.SetPosition(pointsCount - 1, newPoint);

        // Update the mass
        rb2d.mass += massPerPoint;

        // Edge Collider
        // Edge colliders accept only 2 points or more (we can't create an edge with one point :D )
        if (pointsCount > 1)
            edgeCollider.points = points.ToArray();
    }

    // Retrieves the last point on the line
    public Vector2 GetLastPoint()
    {
        return (Vector2)lineRenderer.GetPosition(pointsCount - 1);
    }

    // Returns the line width
    public float GetWidth()
    {
        return lineWidth;
    }

    public Color GetLineColor()
    {
        return lineColor;
    }

    public void ApplyBodyTypeProperty()
    {
        // If the line supposed to be affected by gravity, make its body type dynamic
        if (affectedByGravity)
            rb2d.bodyType = RigidbodyType2D.Dynamic;

        // Else, set body type to static
        else
            rb2d.bodyType = RigidbodyType2D.Static;
    }

}
