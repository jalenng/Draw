using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Configuration parameters
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float movementSmoothing = 0.05f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float overlap;
    [SerializeField] private LayerMask ground;

    // Cached components
    private Rigidbody2D rb2d;

    // State variables
    private Vector3 m_Velocity = Vector3.zero;
    private float horizontal;
    private bool onGround;
    private bool jump;
    private Animator anim;
    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Update()
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
        anim.SetFloat("Speed",Mathf.Abs(horizontal * speed));
        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
        }


    }

    private void FixedUpdate()
    {
        Move();
        if (jump) Jump();
        else onGround = Physics2D.OverlapCircle(groundCheck.position, overlap, ground);
        jump = false;
        if (rb2d.velocity.y == 0)
        {
            anim.SetBool("isJumping",false);
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
}
