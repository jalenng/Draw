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
    bool playerDead = false;
    Vector3 playerPos;

    void Update() {
        // Only move if player isn't dead
        if(!playerDead) MoveStraight();
    }
    private void OnCollisionEnter2D(Collision2D other) 
    {
        // If collider into player, kill it and disable movement, respawn wall
        if(other.gameObject.CompareTag("Player"))
        {
            playerPos = other.transform.position;
            other.gameObject.GetComponent<PlayerMovement>().Die();
            playerDead = true;
            StartCoroutine(Respawn(1f));
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
        Debug.Log(direction);
        transform.position -= direction;
        Debug.Log("Respawning at " + transform.position);
        playerDead = false;
    }
}
