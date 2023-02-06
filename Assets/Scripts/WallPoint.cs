using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallPoint : MonoBehaviour
{
    [SerializeField] Vector3 respawnOffset; 
     bool isActivated = false;
     void Start()
     {

     }
     void Update()
     {

     }
    private void OnTriggerEnter2D(Collider2D other) 
    {
        Debug.Log("something blah blah");
        if(!isActivated && other.CompareTag("ScribbleWall"))
        {
            ScribbleWall scribbleWall = other.GetComponent<ScribbleWall>();
            Debug.Log("Setting new respawn pos");
            scribbleWall.SetRespawnPos(transform.position + respawnOffset);
            isActivated = true;
        }
    }
}
