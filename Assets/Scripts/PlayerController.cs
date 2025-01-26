using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] public Rigidbody2D rb2d;

    //Player Movement
    [SerializeField] private float moveSpeed = 5f;  
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float groundCheckRadius = 0.2f; 
    [SerializeField] private LayerMask groundLayer;  
    [SerializeField] private bool isGrounded;

    public bool goalReached = false;

    private void Start()
    {
    }

    private void Update()
    {
        HandleMovement();
        HandleJumping();
    }

    private void HandleMovement()
    {
        float horizontalInput = 0f;
        if (Input.GetKey(KeyCode.A))
        {
            horizontalInput = -1f;  
        }
        else if (Input.GetKey(KeyCode.D))
        {
            horizontalInput = 1f;  
        }
        rb2d.velocity = new Vector2(horizontalInput * moveSpeed, rb2d.velocity.y);
    }

    private void HandleJumping()
    {
        isGrounded = Physics2D.OverlapCircle(transform.position, groundCheckRadius, groundLayer);

        if (isGrounded && Input.GetKeyDown(KeyCode.W))
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, jumpForce);
        }
    }

    public void resetVelocity()
    {
        rb2d.velocity = new Vector2 (0f, 0f);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Goal")
        {
            goalReached = true;
        }
    }

}
