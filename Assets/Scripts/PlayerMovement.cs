using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb2d;
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float movementSmoothing = 0.05f;
    private bool onGround;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float overlap;
    [SerializeField] private LayerMask ground;
    private Vector3 m_Velocity = Vector3.zero;
    private float horizontal;
    private bool jump;
    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
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
        }
    }
}
