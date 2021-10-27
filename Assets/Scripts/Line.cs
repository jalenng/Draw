using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{
    // Configuration parameters
    [SerializeField] PhysicsMaterial2D physicsMaterial;
    [SerializeField] float lineWidth = 0.05f;
    [SerializeField] Color lineColor = Color.black;
    [SerializeField] float pointsMinDistance = 0.1f; // The minimum distance between line's points.

    // Cached components
    LineRenderer lineRenderer;
    EdgeCollider2D edgeCollider;

    // State variables
    [HideInInspector] public List<Vector2> points = new List<Vector2>();
    [HideInInspector] public int pointsCount = 0;
    float circleColliderRadius; // A circle collider is added to each line point

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        edgeCollider = GetComponent<EdgeCollider2D>();
    }

    void Start()
    {
        edgeCollider.sharedMaterial = physicsMaterial;

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
        circleCollider.sharedMaterial = physicsMaterial;

        // Line Renderer
        lineRenderer.positionCount = pointsCount;
        lineRenderer.SetPosition(pointsCount - 1, newPoint);

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

}
