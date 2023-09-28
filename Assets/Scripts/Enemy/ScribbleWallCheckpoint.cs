using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ScribbleWallCheckpoint : Checkpoint
{
    private ScribbleWall wall;
    [SerializeField] Vector3 wallRespawnPos;

    private void Start() 
    {
        wall = FindObjectOfType<ScribbleWall>();
    }
    private void OnTriggerEnter2D(Collider2D other) 
    {
        // If the checkpoint has not been activated,
        // and the player has collided with the checkpoint...
        if (!isActivated && other.CompareTag("Player")) 
        {
            wall.setRespawnPosition(wallRespawnPos);
        }
    }
}
