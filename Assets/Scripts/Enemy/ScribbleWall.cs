using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random=UnityEngine.Random;

public class ScribbleWall : Enemy
{
    [SerializeField] Rigidbody2D rb2d;

    [SerializeField] private float respawnOffset;

    Vector3 respawnPos;
    bool playerDead = false;
    void Awake()
    {
        respawnPos = transform.position;
    }
    void Update() {
        if(!playerDead) MoveStraight();
    }
    public void SetRespawnPos(Vector3 pos) 
    {
        respawnPos = pos;
    }
    private void OnCollisionEnter2D(Collision2D other) 
    {
        if(other.gameObject.CompareTag("Player"))
        {
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
        Vector3 direction = transform.position - points[index - 1].transform.position;
        direction = direction.normalized;
        direction *= respawnOffset;
        transform.position -= direction;
        playerDead = false;
    }
}
