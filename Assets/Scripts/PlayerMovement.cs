using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Configuration parameters
    [SerializeField] private bool animateSpawnOnLoad = false;

    [Header("Movement")]
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float movementSmoothing = 0.05f;
    
    [Header("Ground Check")]
    [SerializeField] private LayerMask ground;
    [SerializeField] private Collider2D feetCollider;

    [Header("Auto Respawn")]
    [SerializeField] private float minY = -20f;

    // Cached components
    private Rigidbody2D rb2d;
    private Animator anim;

    // State variables
    public Vector3 respawnPos;
    private bool isDead = false;
    [SerializeField]private bool isPaused = false;
    
    // Input variables
    private bool jumpRequested = false;
    private float horizontal;

    private void Awake()
    {
        // Get components
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

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
        if (isDead || isPaused)
        {
            
            return;
        }

        Walk();

        if (feetCollider.IsTouchingLayers(ground))
            anim.SetBool("isTouchingGround", true);
        else 
            anim.SetBool("isTouchingGround", false);

        if (jumpRequested)
        {
            if (feetCollider.IsTouchingLayers(ground)) {
                Jump();
                anim.SetTrigger("Jump");
            }
            jumpRequested = false;
        }
                
    }

    private void GetInput()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        if (Input.GetButtonDown("Jump"))
            jumpRequested = true;
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
        rb2d.velocity = Vector2.zero;
        if (isPaused)
            transform.parent = null;
        
        isPaused = !isPaused;
    }

    private void Walk()
    {
        rb2d.velocity = new Vector2(horizontal * speed, rb2d.velocity.y);
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

        // Trigger death animation
        anim.SetTrigger("Dead");

        StartCoroutine(Respawn());
    }
    void Spawn()
    {
        anim.SetTrigger("Spawn");
    }

    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(1f);

        // Unfreeze the player
        rb2d.simulated = true;
        rb2d.velocity = Vector2.zero;

        // Move the player to the respawn position
        transform.position = respawnPos;

        isDead = false;
    }

}
