using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ScribbleWallCheckpoint : Checkpoint
{
    private ScribbleWall wall;
    [SerializeField] Vector3 wallRespawnPos;

    private void Start() 
    {
        anim = GetComponent<Animator>();
        wall = FindObjectOfType<ScribbleWall>();
    }
    private void OnTriggerEnter2D(Collider2D other) 
    {
        // If the checkpoint has not been activated,
        // and the player has collided with the checkpoint...
        if (!isActivated && other.CompareTag("Player")) 
        {
            wall.setRespawnPosition(wallRespawnPos);

            PlayerMovement playerMovement = other.GetComponent<PlayerMovement>();
            if (playerMovement)
            {
                // Update the player's respawn position
                playerMovement.SetRespawnPos(transform.position + respawnOffset);
                Activate();
            }
            else
                Debug.LogError("[Checkpoint] Cannot set checkpoint because PlayerController was not found");
        }
    }
}
