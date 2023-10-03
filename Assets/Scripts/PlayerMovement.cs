using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed = 4.25f;
    [SerializeField] private float jumpForce = 30f;
    [SerializeField] private float deferredJumpDelay = 0.1f;
    [SerializeField] private float coyoteJumpDelay = 0.1f;
    [SerializeField] private float jumpCooldownTime = 0.2f;
    [SerializeField] private float quickFallGravityScale = 1.5f;

    [Header("Ground Check")]
    [SerializeField] private LayerMask ground;
    [SerializeField] private Collider2D feetCollider;

    [Header("Auto Respawn")]
    [SerializeField] private float minY = -20f;

    [Header("Audio Sources")]
    // Audio sources
    [SerializeField] private AudioSource jumpAudioSource;
    [SerializeField] private AudioSource footstepAudioSource;
    [SerializeField] private AudioSource deathAudioSource;
    [SerializeField] private AudioSource spawnAudioSource;

    // Cached components
    private Rigidbody2D rb2d;
    private Animator anim;
    private AchievementUnlocker achievementUnlocker;

    // State variables
    private float SFXDelay = 0.09f;
    private float curSFXDelay;
    public RespawnManager respawner;
    public Vector3 respawnPos;
    private Vector3 lastUnpausedPos;
    private bool isDead = false;
    private bool isPaused = false;
    private int numRespawnsAtRespawnPoint = 0;

    // Input variables
    private bool jumpRequested = false;
    private float horizontal = 0f;
    private float lastGroundContactTime = -1f;
    private float lastJumpRequestedTime = -1f;
    private float lastJumpTime = -1f;

    // Consts
    private const int numRespawnsAtPointForAchievement = 10;

    private void Awake()
    {
        // Get components
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        achievementUnlocker = GetComponent<AchievementUnlocker>();

        respawner = FindObjectOfType<RespawnManager>();

        // Set initial respawn position
        SetRespawnPos(transform.position);
    }

    private void Update()
    {
        if (isDead) return;

        if (isPaused)
        {
            anim.SetBool("Walking", false);
            anim.SetFloat("YVelocity", 0);
            anim.SetBool("TouchingGround", true);
            return;
        }

        GetInput();
        UpdateSpriteDirection();

        lastUnpausedPos = this.transform.position;

        // Trigger respawn when Stickman falls too far
        if (transform.position.y < minY)
            Die();

        anim.SetBool("Walking", horizontal != 0);
        anim.SetFloat("YVelocity", rb2d.velocity.y);

        // Draw velocity vector in editor
        Debug.DrawLine(transform.position, transform.position + (Vector3)rb2d.velocity, Color.red);
    }

    private void FixedUpdate()
    {
        if (isDead || isPaused) return;

        UpdateGravityScale();

        Walk();

        // Capture the time if the player is touching the ground.
        bool isTouchingGround = feetCollider.IsTouchingLayers(ground);
        if (isTouchingGround) lastGroundContactTime = Time.time;
        anim.SetBool("TouchingGround", isTouchingGround);   // update animator as well

        // Capture the time if the player has requested to jump after the cooldown period
        bool recentlyJumped = lastJumpTime > 0 && (Time.time - lastJumpTime) < jumpCooldownTime;
        if (!recentlyJumped && jumpRequested) lastJumpRequestedTime = Time.time;
        jumpRequested = false;

        // Negative time values are considered as invalid. Check to see if they're are positive.
        if (lastGroundContactTime > 0 && lastJumpRequestedTime > 0)
        {
            // If so, get the time in which the values were last true.
            // We will use this for jump deferring and coyote jump.
            bool recentlyContactedGround = (Time.time - lastGroundContactTime) < deferredJumpDelay;
            bool recentlyRequestedJump = (Time.time - lastJumpRequestedTime) < coyoteJumpDelay;

            // Determine if we should jump
            bool shouldJumpNow = recentlyContactedGround && recentlyRequestedJump;
            if (shouldJumpNow)
            {
                lastGroundContactTime = -1f;
                lastJumpRequestedTime = -1f;
                lastJumpTime = Time.time;
                Jump();
            }
        }
    }

    private void GetInput()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        if (Input.GetButtonDown("Jump") || Input.GetButtonDown("Submit"))
            jumpRequested = true;
        if (horizontal == 0)
            footstepAudioSource.Stop();
    }

    private void UpdateSpriteDirection()
    {
        if (horizontal < 0)
            GetComponent<SpriteRenderer>().flipX = true;
        else if (horizontal > 0)
            GetComponent<SpriteRenderer>().flipX = false;
    }

    public void SetMovementPaused(bool value)
    {
        rb2d.velocity = Vector2.zero;
        if (!value)
        {
            transform.SetParent(null, true);
        }
        isPaused = value;
        Debug.Log($"[PlayerMovement] Player movement paused set to {value}");
    }

    private void UpdateGravityScale()
    {
        // Update gravity scale so player falls down quicker
        float max = 5f;
        float downVelocity = -Mathf.Clamp(rb2d.velocity.y, -max, 0);
        float ratio = downVelocity / max;
        rb2d.gravityScale = Mathf.Lerp(1, quickFallGravityScale, ratio);
    }

    private void Walk()
    {
        // The SFXDelay stuff is added so that it will only play sounds after the player
        // has been walking for a certaint amount of time, just so things like
        // walking up stairs doesnt spam a bunch of short audio clips.
        float targetVelocity = horizontal * speed;
        float velocityDiff = targetVelocity - rb2d.velocity.x;
        float movement = Mathf.Pow(Mathf.Abs(velocityDiff), 2) * Mathf.Sign(velocityDiff);
        rb2d.AddForce(movement * Vector2.right, ForceMode2D.Impulse);

        // The basic audio logic is: if not on ground, then stop footsteps.
        // If velocity isn't 0 and right now we aren't playing sound, then play a sound.
        if (!feetCollider.IsTouchingLayers(ground))
        {
            footstepAudioSource.Stop();
            curSFXDelay = SFXDelay;
        }
        else if ((anim.GetBool("Walking")) && !footstepAudioSource.isPlaying)
        {
            if (curSFXDelay > 0)
            {
                curSFXDelay -= Time.deltaTime;
            }
            else
            {
                footstepAudioSource.Play();
                curSFXDelay = SFXDelay;
            }
        }
    }

    private void Jump()
    {
        rb2d.velocity = new Vector2(rb2d.velocity.x, 0);
        rb2d.AddForce(jumpForce * Vector2.up, ForceMode2D.Impulse);

        anim.SetTrigger("Jump");
        jumpAudioSource.Play();
    }

    public void SetRespawnPos(Vector3 respawnPos)
    {
        this.respawnPos = respawnPos;
        numRespawnsAtRespawnPoint = 0;
    }

    [ContextMenu("Die")]
    public void Die()
    {
        if (isDead) return;

        isDead = true;

        // Freeze the player
        rb2d.simulated = false;
        footstepAudioSource.Stop();
        deathAudioSource.Play();

        // Trigger death animation
        anim.SetTrigger("Dead");
        StartCoroutine(Respawn());
    }

    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(1f);

        // Unfreeze the player
        rb2d.simulated = true;
        rb2d.velocity = Vector2.zero;

        respawner?.StartObjectRespawn();

        // Move the player to the respawn position
        spawnAudioSource.Play();
        transform.position = respawnPos;

        // Unlock achievement if respawned a given number of times at the same point
        numRespawnsAtRespawnPoint += 1;
        if (numRespawnsAtRespawnPoint >= numRespawnsAtPointForAchievement)
        {
            achievementUnlocker.SetAchievement();
        }

        isDead = false;
    }

    public void SetCanMove(int isOne)
    {
        rb2d.simulated = (isOne == 1);
    }

    public bool GetPlayerDead()
    {
        return isDead;
    }

    public Vector3 GetLastUnpausedPos()
    {
        return lastUnpausedPos;
    }

    public Vector3 GetRespawnPos()
    {
        return respawnPos;
    }
}
