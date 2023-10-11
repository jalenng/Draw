using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[ExecuteAlways]
public class ScribbleWall : Enemy
{
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private float speedUpDistance = 40;
    [SerializeField] private float baseSpeed;
    private Transform respawnTarget;
    void Update()
    {
        // Iterate through the points and connect them with a line in the editor.
        // This makes it easier to see the path that the scribble wall will take.
        if (points.Count > 0)
        {
            Vector3 pos1 = points[0].position;
            for (int i = 1; i < points.Count; i++)
            {
                Vector3 pos2 = points[i].position;
                Debug.DrawLine(pos1, pos2, Color.red);
                pos1 = pos2;
            }
        }
    }
    void FixedUpdate()
    {
        UpdateSpeed();
        MoveStraight();
    }
    void Awake()
    {
        baseSpeed = speed;
    }
    // Vector3(-45,121.220001,0) OG Starting Position 
    public void Respawn()
    {
        transform.position = originalPosition;
        if (respawnTarget) index = points.IndexOf(respawnTarget);
    }
    public void setRespawnPosition(Vector3 pos)
    {
        originalPosition = pos;
    }
    private void UpdateSpeed()
    {
        if (!playerMovement) return;

        float distanceFromPlayer = Vector3.Distance(transform.position, playerMovement.transform.position);
        if (distanceFromPlayer > speedUpDistance)
        {
            // Find how far away from "speedUpDistance" the wall is
            distanceFromPlayer -= speedUpDistance;
            // Divide by number to make smaller ;p
            // Add exponential number to speed based on this number
            distanceFromPlayer = distanceFromPlayer * 10;
            this.speed = baseSpeed + 5 + distanceFromPlayer;
        }
        else
        {
            // If the wall gets within the threshhold, set speed back to the original speed.
            this.speed = baseSpeed;
        }
    }
    public void setRespawnTarget(Transform target)
    {
        respawnTarget = target;
    }
}
