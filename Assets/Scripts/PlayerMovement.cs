using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Configuration parameters
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

    // State variables
    private Vector3 m_Velocity = Vector3.zero;
    private float horizontal;
    private bool onGround;
    private bool jump;
    private Animator anim;
    private bool isDead = true;
    private Vector3 respawnPos;
    private bool isPaused = false;
    
    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        respawnPos = transform.position;

        StartCoroutine(Spawn());
    }

    private void Update()
    {
        if(!isDead && !isPaused)
        {
            horizontal = Input.GetAxisRaw("Horizontal");
            if (horizontal < 0)
            {
                GetComponent<SpriteRenderer>().flipX = true;
            }
            else if (horizontal > 0)
            {
                GetComponent<SpriteRenderer>().flipX = false;
            }

            anim.SetFloat("Speed", Mathf.Abs(horizontal * speed));
            if (Input.GetButtonDown("Jump"))
            {
                jump = true;
            }
        }

        // Trigger respawn when Stickman falls too far
        if (transform.position.y < minY)
        {
            Die();
        }
    }

    public void TogglePause()
    {
        horizontal = 0;
        anim.SetFloat("Speed",0);
        isPaused = !isPaused;
    }
    private void FixedUpdate()
    {
        if(!isDead)
        {
            Move();
            if (jump) Jump();
            else onGround = feetCollider.IsTouchingLayers(ground);
            jump = false;

            anim.SetBool("isTouchingGround", onGround);

            if (rb2d.velocity.y <= 0 && onGround)
            {
                anim.SetBool("isJumping", false);
            }
        }
    }


    private void Move()
    {
        Vector3 target = new Vector2(horizontal * speed, rb2d.velocity.y);
        rb2d.velocity = Vector3.SmoothDamp(rb2d.velocity, target, ref m_Velocity, movementSmoothing);
    }
    private void Jump()
    {
        if (onGround && jump)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, jumpForce);
            anim.SetBool("isJumping",true);
        }
    }

    public void SetRespawnPos(Vector3 respawnPos)
    {
        this.respawnPos = respawnPos;
    }

    public void Die()
    {
        if(!isDead)
        {
            isDead = true;
            anim.SetBool("isJumping",false);
            anim.SetBool("Dead", true);
            rb2d.gravityScale = 0;
            rb2d.bodyType = RigidbodyType2D.Static;

            IEnumerator unfocusCoroutine = FindObjectOfType<TargetGroupUpdater>().Unfocus(true);
            StartCoroutine(unfocusCoroutine);

            StartCoroutine(Respawn());
        }
    }

    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(1f);
        anim.SetBool("Dead",false);
        rb2d.gravityScale = 1;
        rb2d.bodyType = RigidbodyType2D.Dynamic;
        isDead = false;
        rb2d.velocity = Vector2.zero;
        transform.position = respawnPos;
    }

    IEnumerator Spawn()
    {
        yield return new WaitForSeconds(0.2f);
        anim.SetBool("Spawn",true);
        yield return new WaitForSeconds(2.2f);
        anim.SetBool("Spawn",false);
        isDead = false;
    }

}
