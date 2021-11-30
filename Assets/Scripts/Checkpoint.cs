using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    // Configuration parameters
    [Tooltip("How much to offset from the checkpoint's position (bottom of pole) when respawning the player")]
    [SerializeField] Vector3 respawnOffset = new Vector3(0.5f, 1.25f, 0.0f);

    // State variables
    bool activated = false;

    // Cached component references
    Animator anim;

    private void Start() 
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        // If the checkpoint has not been activated,
        // and the player has collided with the checkpoint...
        if (!activated && other.CompareTag("Player")) 
        {
            PlayerMovement playerMovement = other.GetComponent<PlayerMovement>();

            // Update the player's respawn position
            if (playerMovement) 
            {
                playerMovement.SetRespawnPos(transform.position + respawnOffset);
                activated = true;

                // Animate the checkpoint activation
                anim.SetBool("Activated", true);
            }
            else
                Debug.LogError("Cannot set checkpoint because PlayerController was not found");
        }
    }
}
