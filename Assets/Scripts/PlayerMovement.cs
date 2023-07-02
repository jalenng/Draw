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

    [Header("Ground Check")] [SerializeField]
    private LayerMask ground;

    [SerializeField] private Collider2D feetCollider;

    [Header("Auto Respawn")] [SerializeField]
    private float minY = -20f;

    // Cached components
    private Rigidbody2D rb2d;
    private Animator anim;
    private AudioSource playerSound;
    private AudioSystem audSysSound;
    [SerializeField] private GameObject audSys;
    // State variables

    public RespawnManager respawner;
    public Vector3 respawnPos;
    private bool isDead = false;
    [SerializeField] private bool isPaused = false;

    // Input variables
    private bool jumpRequested = false;
    private int jumpBufferFrames = 0;
    private float horizontal;

    private void Awake()
    {
        // Get components
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        playerSound = GetComponent<AudioSource>();

        respawner = FindObjectOfType<RespawnManager>();
        audSysSound = audSys.GetComponent<AudioSystem>();
        // Set initial respawn position
        respawnPos = transform.position;
        if (animateSpawnOnLoad)
            Spawn();
    }

    private void Update()
    {
        if (isDead)
        {
            return;
        }

        if (isPaused)
        {
            anim.SetFloat("XSpeed", 0);
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

        anim.SetFloat("XSpeed", Mathf.Abs(rb2d.velocity.x));
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
                audSysSound.PlaySFX("penciljump");
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

    }

    private void GetInput()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        if(horizontal == 0) playerSound.Stop();
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
        Debug.Log("Pause " + isPaused);
        if (isPaused) transform.parent = null;

        rb2d.velocity = Vector2.zero;
        isPaused = !isPaused;
    }

    private void Walk()
    {
        rb2d.velocity = new Vector2(horizontal * speed, rb2d.velocity.y);
        if(!feetCollider.IsTouchingLayers(ground)) {
            playerSound.Stop();
        } else if(rb2d.velocity.x != 0 && !playerSound.isPlaying) {
            playerSound.Play(0);
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
        playerSound.Stop();
        audSysSound.PlaySFX("eraser");

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

        respawner.StartObjectRespawn();    
        // Move the player to the respawn position
        audSysSound.PlaySFX("pop");
        transform.position = respawnPos;

        isDead = false;
    }

}
