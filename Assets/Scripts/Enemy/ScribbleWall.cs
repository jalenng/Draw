using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random=UnityEngine.Random;

public class ScribbleWall : Enemy
{
    [SerializeField] Rigidbody2D rb2d;
    [SerializeField] private float respawnOffset;
    [SerializeField] private PlayerMovement playerMovement;
    Vector3 playerPos;
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
                playerPos = playerMovement.transform.position;
            }
        }
    }
    public void StartRespawn() {
        StartCoroutine(Respawn(1f));
    }
    IEnumerator Respawn(float wait)
    {
        yield return new WaitForSeconds(wait);

        // Get direction of previous point in path
        Vector3 direction = transform.position - points[index - 1].transform.position;
        direction = direction.normalized;

        // Find distance of player to last respawn point to add to offset
        float distToRespawn = Vector3.Distance(playerPos, playerMovement.respawnPos);

        // Create Vector3 with magnitude of distance to respawnpoint plus our offset
        direction *= (respawnOffset + distToRespawn);
        transform.position -= direction;
        respawning = false;
    }
}
