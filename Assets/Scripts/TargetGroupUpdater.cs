using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class TargetGroupUpdater : MonoBehaviour
{
    // Configuration parameters
    [Header("Drawing Area Detection")]
    [SerializeField] float distanceThreshold = 10f;
    [SerializeField] GameObject player;

    
    [Header("Drawing Area Weight")]
    [SerializeField] float currentWeight = 0f;

    
    [Header("Target Group Index")]
    [SerializeField] int dynamicTargetIndex = 0;
    
    // Cached components and objects
    Animator anim;
    CinemachineTargetGroup ctg;
    DrawingArea[] drawingAreas;

    void Start()
    {
        anim = GetComponent<Animator>();
        ctg = GetComponent<CinemachineTargetGroup>();
        drawingAreas = FindObjectsOfType<DrawingArea>();
    }

    void Update()
    {
        SearchClosestDrawingArea();
        UpdateCurrentWeight();
    }

    void SearchClosestDrawingArea()
    {
        // Distance is based on player position
        Vector3 playerPosition = player.transform.position;

        // Iterate through each drawing area to find the closest one
        float closestDistance = distanceThreshold;
        Transform closestTransform = null;
        foreach (DrawingArea a in drawingAreas)
        {
            Transform areaTransform = a.transform;
            float distance = Vector2.Distance(playerPosition, areaTransform.position);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestTransform = areaTransform;
            }
        }
        
        Debug.Log(closestDistance);
        
        // If a drawing area within the threshold is found, set it as a target
        if (closestTransform != null)
        {
            SetTarget(closestTransform);
            anim.SetBool("hasTarget", true);
        }
        // Otherwise, remove it as a target
        else
            anim.SetBool("hasTarget", false);   // Note: an animation event will invoke ReleaseTarget() when the time is right
    }

    public void ReleaseTarget()
    {
        UpdateTarget(null, 0);
    }

    void SetTarget(Transform t)
    {
        // Calculate the target's radius
        BoxCollider2D boxCollider = t.GetComponent<BoxCollider2D>();
        Vector2 collisionBoundsSize = boxCollider.size;
        float width = collisionBoundsSize.x * t.localScale.x;
        float height = collisionBoundsSize.y * t.localScale.y;
        float radius = Mathf.Max(width, height) / 2;

        // Upadte the target
        UpdateTarget(t, radius);
    }

    void UpdateTarget(Transform t, float r) {
        ctg.m_Targets[dynamicTargetIndex].target = t;
        ctg.m_Targets[dynamicTargetIndex].radius = r;
    }

    void UpdateCurrentWeight()
    {
        ctg.m_Targets[dynamicTargetIndex].weight = currentWeight;
    }

}
