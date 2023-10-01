using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrangeObject : RespawnInterface
{
    Rigidbody2D rb2d;
    [SerializeField] Vector3 respawnPos;
    [SerializeField] public bool staticBodyByDefault = true;
    Quaternion respawnRotation;
    AudioSource audioSource;
    AchievementUnlocker achievementUnlocker;


    // Make object static and unaffected by gravity
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        achievementUnlocker = GetComponent<AchievementUnlocker>();

        if (staticBodyByDefault)
        {
            rb2d.bodyType = RigidbodyType2D.Static;
        }
        respawnPos = transform.position;
        respawnRotation = transform.rotation;
    }

    // Make object affected by gravity upon collision
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (rb2d.bodyType != RigidbodyType2D.Dynamic)
        {
            staticBodyByDefault = false;
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
        if (staticBodyByDefault)
        {
            rb2d.bodyType = RigidbodyType2D.Static;
        }
        rb2d.velocity = Vector2.zero;
        transform.position = respawnPos;
        transform.rotation = respawnRotation;
    }
}
