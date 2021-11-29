using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [Tooltip("How much to offset from the checkpoint's position (bottom of pole) when respawning the player")]
    [SerializeField] Vector3 respawnOffset = new Vector3(0.5f, 1.25f, 0.0f);
    bool activated = false;

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (!activated && other.CompareTag("Player")) 
        {
            PlayerMovement playerMovement = other.GetComponent<PlayerMovement>();

            if (playerMovement) 
            {
                playerMovement.SetRespawnPos(transform.position + respawnOffset);
                activated = true;
            }
            else
                Debug.LogError("Cannot set checkpoint because PlayerController was not found");
        }
    }
}
