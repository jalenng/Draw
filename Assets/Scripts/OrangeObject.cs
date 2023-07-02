using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrangeObject : RespawnInterface
{
    Rigidbody2D rb2d;
    [SerializeField] Vector3 respawnPos;
    Quaternion respawnRotation;

    // Make object static and unaffected by gravity
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.bodyType = RigidbodyType2D.Static;
        respawnPos = transform.position;
        respawnRotation = transform.rotation;
    }

    // Make object affected by gravity upon collision
    private void OnCollisionEnter2D(Collision2D other) {
        rb2d.bodyType = RigidbodyType2D.Dynamic;
    }
    
    public override void StartRespawn() {
        StartCoroutine(Respawn(0f));
    }
    IEnumerator Respawn(float wait)
    {
        yield return new WaitForSeconds(wait);
        rb2d.bodyType = RigidbodyType2D.Static;
        transform.position = respawnPos;
        transform.rotation = respawnRotation;
    }
}
