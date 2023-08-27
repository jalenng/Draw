using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Configuration parameters
    [SerializeField] private bool animateSpawnOnLoad = false;

    [Header("Movement")] [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float movementSmoothing = 0.05f;
    [SerializeField] private int jumpBufferFramesMax = 5;
    private float SFXDelay = 0.09f;


    [Header("Ground Check")] [SerializeField]
    private LayerMask ground;

    [SerializeField] private Collider2D feetCollider;

    [Header("Auto Respawn")] [SerializeField]
    private float minY = -20f;

    // Cached components
    private Rigidbody2D rb2d;
    private Animator anim;
    private AudioSource footstepAS;
    [SerializeField] private AudioSource jumpAS;
    [SerializeField] private AudioSource deathAS;
    [SerializeField] private AudioSource SpawnAS;
    // State variables

    private float curSFXDelay;
    public RespawnManager respawner;
    public Vector3 respawnPos;
    private Vector3 prevPos;

    private bool isDead = false;
    [SerializeField] private bool isPaused = false;

    // Input variables
    private bool jumpRequested = false;
    private int jumpBufferFrames = 0;
    private float horizontal;

    private void Awake()
    {
        // Get components
        footstepAS = GetComponent<AudioSource>();
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        respawner = FindObjectOfType<RespawnManager>();
        // Set initial respawn position
        prevPos = respawnPos = transform.position;
        if (animateSpawnOnLoad) {
            rb2d.simulated = false;
            Spawn();
        }
    }

    private void Update()
    {
        if (isDead)
        {
            return;
        }

        if (isPaused)
        {
            anim.SetBool("Moving", false);
            anim.SetFloat("YVelocity", 0);
            anim.SetBool("isTouchingGround", true);
            anim.SetTrigger("Reset");
            return;
        }

        GetInput();
        UpdateSpriteDirection();

        // Trigger respawn when Stickman falls too far
        if (transform.position.y < minY)
            Die();

        anim.SetBool("Moving", (transform.position.x - prevPos.x) != 0 && horizontal != 0);
        anim.SetFloat("YVelocity", rb2d.velocity.y);
    }

    private void FixedUpdate()
    {
        if (isDead || isPaused) return;

        Walk();

        if (feetCollider.IsTouchingLayers(ground))
            anim.SetBool("isTouchingGround", true);
        else
            anim.SetBool("isTouchingGround", false);

        if (jumpRequested)
        {
            if (feetCollider.IsTouchingLayers(ground))
            {
                Jump();
                jumpAS.Play();
                anim.SetTrigger("Jump");
                jumpRequested = false;
            }
            else
            {
                jumpBufferFrames--;
                if (jumpBufferFrames < 0)
                    jumpRequested = false;
            }
        }
        prevPos = transform.position;
    }

    private void GetInput()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        if(horizontal == 0) footstepAS.Stop();
        if (Input.GetButtonDown("Jump"))
        {
            jumpRequested = true;
            jumpBufferFrames = jumpBufferFramesMax;
        }
    }

    private void UpdateSpriteDirection()
    {
        if (horizontal < 0)
            GetComponent<SpriteRenderer>().flipX = true;
        else if (horizontal > 0)
            GetComponent<SpriteRenderer>().flipX = false;
    }

    public void TogglePause()
    {
        Debug.Log($"[PlayerMovement] Pause: {isPaused}");
        if (isPaused) transform.parent = null;

        rb2d.velocity = Vector2.zero;
        isPaused = !isPaused;
    }

    private void Walk()
    {
        // The SFXDelay stuff is added so that it will only play sounds after the player
        // has been walking for a certaint amount of time, just so things like
        // walking up stairs doesnt spam a bunch of short audio clips.
        rb2d.velocity = new Vector2(horizontal * speed, rb2d.velocity.y);
        // The basic audio logic is: if not on ground, then stop footsteps.
        // If velocity isn't 0 and right now we aren't playing sound, then play a sound.
        if(!feetCollider.IsTouchingLayers(ground)) {
            footstepAS.Stop();
            curSFXDelay = SFXDelay;
        } else if((anim.GetBool("Moving")) && !footstepAS.isPlaying) {
            if(curSFXDelay > 0) {
                curSFXDelay -= Time.deltaTime;
            } else {
                footstepAS.Play();
                curSFXDelay = SFXDelay;
            }   
        }
    }

    private void Jump()
    {
        rb2d.velocity = new Vector2(rb2d.velocity.x, jumpForce);
    }

    public void SetRespawnPos(Vector3 respawnPos)
    {
        this.respawnPos = respawnPos;
    }

    public void Die()
    {
        if (isDead) return;
        
        isDead = true;
        
        // Freeze the player
        rb2d.simulated = false;
        footstepAS.Stop();
        deathAS.Play();

        // Trigger death animation
        anim.SetTrigger("Dead");
        StartCoroutine(Respawn());
    }
    void Spawn()
    {
        anim.SetTrigger("Spawn");
    }
    // Code to check if the player position is inside of another collider 

    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(1f);

        // Unfreeze the player
        rb2d.simulated = true;
        rb2d.velocity = Vector2.zero;

        respawner?.StartObjectRespawn();    
        // Move the player to the respawn position
        SpawnAS.Play();
        transform.position = respawnPos;

        isDead = false;
    }
    public void setCanMove(int isOne) {
        rb2d.simulated = (isOne == 1);
    }
}
