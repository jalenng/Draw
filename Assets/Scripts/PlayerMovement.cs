using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

[System.Serializable]
public class RespawnAchievementEntry
{
    public AchievementUnlocker achievementUnlocker;
    public int respawnCount;
}

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed = 4.25f;
    [SerializeField] private float jumpForce = 30f;
    [SerializeField] private float deferredJumpDelay = 0.1f;
    [SerializeField] private float coyoteJumpDelay = 0.1f;
    [SerializeField] private float jumpCooldownTime = 0.2f;
    [SerializeField] private float quickFallGravityScale = 1.5f;
    [SerializeField] private float slopeWalkLimit = 1.5f;

    [Header("Ground Check")]
    [SerializeField] private LayerMask ground;
    [SerializeField] private Collider2D feetCollider;

    [Header("Respawn")]
    [SerializeField] private float minY = -20f; // Respawn once past this level
    [SerializeField] private UnityEvent onRespawn;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource jumpAudioSource;
    [SerializeField] private AudioSource footstepAudioSource;
    [SerializeField] private AudioSource deathAudioSource;
    [SerializeField] private AudioSource spawnAudioSource;

    [Header("Achievements")]
    [SerializeField] private List<RespawnAchievementEntry> respawnAchievements = new List<RespawnAchievementEntry>();

    // Cached components
    private Rigidbody2D rb2d;
    private Animator anim;

    // State variables
    private float SFXDelay = 0.09f;
    private float curSFXDelay;
    public RespawnManager respawner;
    public Vector3 respawnPos;
    private Vector3 lastUnpausedPos;
    private bool isDead = false;
    private bool isPaused = false;
    private float lastNonZeroHorizontal = 0;
    private int numRespawnsAtRespawnPoint = 0;
    private bool isTouchingGround = false;
    private RaycastHit2D groundRayHit;

    // For detecting contacts and slopes
    private ContactFilter2D contactFilter;
    private float slope = 0;

    // Input variables
    private bool jumpRequested = false;
    private float horizontal = 0f;
    private float lastGroundContactTime = -1f;
    private float lastJumpRequestedTime = -1f;
    private float lastJumpTime = -1f;

    private void Awake()
    {
        // Get components
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        respawner = FindObjectOfType<RespawnManager>();

        // Set up contact point filter to only filter ground points
        ContactFilter2D contactFilter = new ContactFilter2D();
        contactFilter.layerMask = ground;

        // Set initial respawn position
        SetRespawnPos(transform.position);
    }

    private void Update()
    {
        if (Time.timeScale == 0) return;
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
        ComputeSlope();
        Walk();
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
        if (Input.GetButtonDown("Jump")) // W, Up, or Space
            jumpRequested = true;

        horizontal = Input.GetAxisRaw("Horizontal");
        if (horizontal == 0)
            footstepAudioSource.Stop();
        else
            lastNonZeroHorizontal = horizontal;
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

    private void ComputeSlope()
    {
        // Get the contact points (up to five)
        ContactPoint2D[] contacts = new ContactPoint2D[5];
        int numContacts = feetCollider.GetContacts(contactFilter, contacts);

        // Get the contact point and the normal vector of interest.
        float furthestX = feetCollider.bounds.center.x;
        Vector2 rayPoint = new Vector2(
            feetCollider.bounds.center.x,
            feetCollider.bounds.min.y // Center of bottom edge of collider
        );
        if (feetCollider is BoxCollider2D)
            rayPoint.y -= ((BoxCollider2D)feetCollider).edgeRadius;
        Vector2 rayDirection = Vector2.down;

        // Try to find the contact point that is furthest out in the given direction.
        for (int i = 0; i < numContacts; i++)
        {
            ContactPoint2D item = contacts[i];

            float thisSlope = item.normal.x / -item.normal.y;
            bool thisPointIsFurther =
                lastNonZeroHorizontal < 0 && item.point.x < furthestX
                || lastNonZeroHorizontal > 0 && item.point.x > furthestX;
            if (thisPointIsFurther)
            {
                furthestX = item.point.x;
                rayPoint = item.point;
                rayDirection = -item.normal;
            }
        }

        // Cast three rays from around the contact point in the direction of the normal
        Debug.DrawRay(rayPoint, rayDirection, Color.cyan);
        RaycastHit2D hit1 = Physics2D.Raycast(rayPoint, rayDirection, 0.5f, ground);
        RaycastHit2D hit2 = Physics2D.Raycast(rayPoint + (Vector2.up * 0.1f), rayDirection, 0.33f, ground);
        RaycastHit2D hit3 = Physics2D.Raycast(rayPoint + (Vector2.up * 0.2f), rayDirection, 0.33f, ground);
        RaycastHit2D[] hits = { hit1, hit2, hit3 };

        // Get the average ground slope from the normals.
        if (hits.Any(hit => hit.collider != null))
        {
            slope = hits
                .Where(hit => hit.collider != null)
                .Select(hit => -hit.normal.x / hit.normal.y)
                .Average();

            for (int i = 0; i < hits.Length; i++)
            {
                Vector2 hitPoint = hits[i].point;
                Vector2 hitNormal = hits[i].normal;
                Debug.DrawRay(hitPoint, hitNormal, Color.magenta);
            }
        }
        else
        {
            slope = 0;
        }

        // Check for ground touch
        groundRayHit = Physics2D.Raycast(rayPoint, rayDirection, 0.25f, ground);
        isTouchingGround = feetCollider.IsTouchingLayers(ground) || groundRayHit.collider != null;
    }

    private void Walk()
    {
        // The SFXDelay stuff is added so that it will only play sounds after the player
        // has been walking for a certaint amount of time, just so things like
        // walking up stairs doesnt spam a bunch of short audio clips.
        float targetVelocity = horizontal * speed;
        bool tooSteep = Mathf.Abs(slope) > slopeWalkLimit;

        float velocityDiff = targetVelocity - rb2d.velocity.x;
        float movement = Mathf.Pow(Mathf.Abs(velocityDiff), 2) * Mathf.Sign(velocityDiff);
        if (tooSteep) movement *= 0.1f;
        movement = Mathf.Clamp(movement, -100, 100);
        rb2d.AddForce(movement * Vector2.right, ForceMode2D.Impulse);

        // The basic audio logic is: if not on ground, then stop footsteps.
        // If velocity isn't 0 and right now we aren't playing sound, then play a sound.
        if (!isTouchingGround)
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
        // Trigger any orange object that the player may have jumped on.
        // This is for if the ground detection is triggered by the raycast and not the collision,
        // which means the player hasn't actually collided with the orange object to make it fall.
        OrangeObject orangeObject = groundRayHit.collider?.gameObject?.GetComponent<OrangeObject>();
        orangeObject?.ActivateOrangeObject();

        // Jump logic
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
        onRespawn.Invoke();

        // Move the player to the respawn position
        spawnAudioSource.Play();
        transform.position = respawnPos;


        // Unlock achievement if respawned a given number of times at the same point
        numRespawnsAtRespawnPoint += 1;
        foreach (RespawnAchievementEntry entry in respawnAchievements)
        {
            AchievementUnlocker achievementUnlocker = entry.achievementUnlocker;
            int respawnCount = entry.respawnCount;
            if (numRespawnsAtRespawnPoint >= respawnCount)
            {
                achievementUnlocker.SetAchievement();
            }
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
