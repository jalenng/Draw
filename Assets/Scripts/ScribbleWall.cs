using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScribbleWall : MonoBehaviour
{
    public Vector3 respawnPos;
    public List<GameObject> touchedPoints;

    void Awake()
    {
        respawnPos = transform.position;
    }

    // Update is called once per frame
    void Update() 
    {
        
    }
    public void SetRespawnPos(Vector3 pos) 
    {
        respawnPos = pos;
    }
    private void OnCollisionEnter2D(Collision2D other) 
    {
        Debug.Log("colliding w something");
        if(other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Collided w Player");
            StartCoroutine(Respawn());
        }
    }
    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(1f);
        transform.position = respawnPos;
    }
}
