using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrangeObject : RespawnInterface
{
    Rigidbody2D rb2d;
    [SerializeField] private bool staticBodyByDefault = true;
    public Vector3 respawnPos;
    public float respawnRot;
    AudioSource audioSource;
    AchievementUnlocker achievementUnlocker;

    // Run initialization in Awake so OrangeObjectData's Start() can 
    // override the initialized values
    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        achievementUnlocker = GetComponent<AchievementUnlocker>();

        // Set body type to default
        rb2d.bodyType = staticBodyByDefault ? RigidbodyType2D.Static : RigidbodyType2D.Dynamic;

        // Save original position as respawn position
        respawnPos = transform.position;
        respawnRot = transform.eulerAngles.z;
    }

    // Make object affected by gravity upon collision
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (rb2d.bodyType != RigidbodyType2D.Dynamic)
        {
            audioSource.Play();
            rb2d.bodyType = RigidbodyType2D.Dynamic;
            achievementUnlocker.SetAchievement();
        }
    }

    public override void StartRespawn()
    {
        StartCoroutine(Respawn(0f));
    }

    IEnumerator Respawn(float wait)
    {
        yield return new WaitForSeconds(wait);

        rb2d.bodyType = staticBodyByDefault ? RigidbodyType2D.Static : RigidbodyType2D.Dynamic;
        rb2d.velocity = Vector2.zero;
        transform.position = respawnPos;
        transform.eulerAngles = new Vector3(0, 0, respawnRot);
    }
}
