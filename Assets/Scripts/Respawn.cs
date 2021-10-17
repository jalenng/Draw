using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    [SerializeField] private Transform respawn;
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            other.transform.position = respawn.transform.position;
        }
    }
}
