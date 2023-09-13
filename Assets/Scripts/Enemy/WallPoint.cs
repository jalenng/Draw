using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallPoint : MonoBehaviour
{
    [SerializeField] Vector3 respawnOffset; 
     bool isActivated = false;
    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(!isActivated && other.CompareTag("ScribbleWall"))
        {
            ScribbleWall scribbleWall = other.GetComponent<ScribbleWall>();
            scribbleWall.SetRespawnPos(transform.position + respawnOffset);
            isActivated = true;
        }
    }
}
