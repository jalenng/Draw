using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random=UnityEngine.Random;

public class ScribbleWall : Enemy
{
    [SerializeField] private float respawnOffset;
    [SerializeField] private PlayerMovement playerMovement;

    private Vector3 playerPos;
    public bool isMovementVertical;
    private bool respawning;

    void Update() {
        // If we're respawning, don't do anything.
        if(!respawning) {
            // If the player is dead & not respawning, respawn them.
            if(playerMovement.GetPlayerDead()) {
                respawning = true;
                StartRespawn(); 
            } else {
            // Otherwise, move forward and update the last position of the player.    
                MoveStraight();
                UpdateDirection();
                playerPos = playerMovement.transform.position;
            }
        }
    }
    // Vector3(-45,121.220001,0)
    private void UpdateDirection() {
        Vector3 movementDirection = points[index].transform.position - transform.position;
        isMovementVertical = (Math.Abs(movementDirection.y) > Math.Abs(movementDirection.x));
    }
    public void StartRespawn() {
        StartCoroutine(Respawn(1f));
    }
    IEnumerator Respawn(float wait)
    {
        float distToRespawn;
        yield return new WaitForSeconds(wait);

        // Get direction of previous point in path
        Vector3 direction = transform.position - points[index - 1].transform.position;
        direction = direction.normalized;

        // Find distance of player to last respawn point to add to offset
        if(isMovementVertical) {
            distToRespawn = Math.Abs(playerPos.y - playerMovement.respawnPos.y);
        } else {
            distToRespawn = Math.Abs(playerPos.x - playerMovement.respawnPos.x);
        }

        // Create Vector3 with magnitude of distance to respawnpoint plus our offset
        direction *= (respawnOffset + distToRespawn);
        transform.position -= direction;
        respawning = false;
    }
}
