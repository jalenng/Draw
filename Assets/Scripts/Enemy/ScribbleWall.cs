using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random=UnityEngine.Random;

public class ScribbleWall : Enemy
{
    [SerializeField] private PlayerMovement playerMovement;

    private bool respawning = false;
    void Update() {
        // If we're respawning, don't do anything.
        if(!respawning) {
            // If the player is dead & wall is not respawning, respawn this
            if(playerMovement.GetPlayerDead()) {
                respawning = true;
                StartRespawn(); 
            } else {
            // Otherwise, move forward
                MoveStraight();
            }
        }
    }
    // Vector3(-45,121.220001,0) OG Starting Position 
    public void StartRespawn() {
        StartCoroutine(Respawn(1f));
    }
    IEnumerator Respawn(float wait)
    {
        yield return new WaitForSeconds(wait);
        transform.position = originalPosition;
        respawning = false;
    }
    public void setRespawnPosition(Vector3 pos) {
        originalPosition = pos;
    }
}
