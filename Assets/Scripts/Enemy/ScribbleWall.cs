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
    private bool respawning = false;

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
        // If we're respawning, don't do anything.
        UpdateSpeed();
        if (!respawning)
        {
            // If the player is dead & wall is not respawning, respawn this
            if (playerMovement.GetPlayerDead())
            {
                respawning = true;
                StartRespawn();
            }
            else
            {
                // Otherwise, move forward
                MoveStraight();
            }
        }
    }
    void Awake()
    {
        baseSpeed = speed;
    }
    // Vector3(-45,121.220001,0) OG Starting Position 
    public void StartRespawn() {
        StartCoroutine(Respawn(.99f));
    }
    IEnumerator Respawn(float wait)
    {
        yield return new WaitForSeconds(wait);
        transform.position = originalPosition;
        respawning = false;
    }
    public void setRespawnPosition(Vector3 pos)
    {
        originalPosition = pos;
    }
    private void UpdateSpeed()
    {
        float distanceFromPlayer = Vector3.Distance(transform.position, playerMovement.transform.position);
        if(distanceFromPlayer > speedUpDistance) {
            // Find how far away from "speedUpDistance" the wall is
            distanceFromPlayer -= speedUpDistance;
            // Divide by number to make smaller ;p
            // Add exponential number to speed based on this number
            distanceFromPlayer = distanceFromPlayer * 10;
            this.speed = baseSpeed + 5 + distanceFromPlayer;
        } else {
            // If the wall gets within the threshhold, set speed back to the original speed.
            this.speed = baseSpeed;
        }
    }
}
