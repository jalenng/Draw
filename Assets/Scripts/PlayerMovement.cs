using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb2d;
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    private bool onGround;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float overlap;
    [SerializeField] private LayerMask ground;
    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        rb2d.velocity = new Vector2(Input.GetAxisRaw("Horizontal")*speed, rb2d.velocity.y);
        onGround = Physics2D.OverlapCircle(groundCheck.position, overlap, ground);
        Jump();

    }
    
    private void Jump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            // Debug.Log("Button Pressed: " + doublejump);
            if (onGround)
            {
                // Debug.Log("Ground Jump");
                rb2d.velocity = new Vector2(rb2d.velocity.x, jumpForce);
            }
        }
    }
}
