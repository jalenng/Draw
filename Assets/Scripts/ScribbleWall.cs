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
        if(other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(Respawn(1f));
        }
    }
    public void StartRespawn() {
        StartCoroutine(Respawn(0f));
    }
    IEnumerator Respawn(float wait)
    {
        yield return new WaitForSeconds(wait);
        transform.position = respawnPos;
    }
}
