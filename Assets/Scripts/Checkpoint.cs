using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    bool activated = false;

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (!activated && other.CompareTag("Player")) 
        {
            PlayerMovement playerMovement = other.GetComponent<PlayerMovement>();

            if (playerMovement) 
            {
                playerMovement.SetRespawnPos(transform.position);
                activated = true;
            }
            else
                Debug.LogError("Cannot set checkpoint because PlayerController was not found");
        }
    }
}
