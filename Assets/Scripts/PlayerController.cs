using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField] public Rigidbody2D rb2d;

    //Player Movement
    [SerializeField] private float moveSpeed = 5f;  
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private LayerMask groundLayer;  
    [SerializeField] private bool isGrounded;

    [SerializeField] private float jumpBoxSizeY;
    [SerializeField] private float jumpBoxSizeX;
    [SerializeField] private float jumpBoxOffsetY;

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
        //isGrounded = Physics2D.BoxCast(transform.position + new Vector3(0, -0.5f, 0), new Vector2(0.5f, 0.3f), 0f, Vector2.down, 0f, groundLayer);
        isGrounded = Physics2D.BoxCast(transform.position + new Vector3(0, jumpBoxOffsetY, 0), new Vector2(jumpBoxSizeX, jumpBoxSizeY), 0f, Vector2.down, 0f, groundLayer);

        if (isGrounded && Input.GetKeyDown(KeyCode.W))
        {
            SFXManager.Instance.PlaySFX("JumpSFX");
            rb2d.velocity = new Vector2(rb2d.velocity.x, jumpForce);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red; // Set Gizmo color to red

        // Define the ground check box position (offset downward)
        Vector2 boxPosition = (Vector2)transform.position + new Vector2(0, jumpBoxOffsetY); // Adjust as needed

        // Define the size of the box
        Vector2 boxSize = new Vector2(jumpBoxSizeX, jumpBoxSizeY); // Adjust width and height as needed

        // Draw the wireframe rectangle
        Gizmos.DrawWireCube(boxPosition, boxSize);
    }

    public void resetVelocity()
    {
        rb2d.velocity = new Vector2 (0f, 0f);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Goal" && !goalReached)
        {
            SFXManager.Instance.PlaySFX("VictorySFX");
            goalReached = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (!(col.gameObject.tag == "HealthyBubble") && !(goalReached))
        {
            SFXManager.Instance.PlaySFX("DeathSFX");
            SFXManager.Instance.StopLoopingMusic();
            Scene currentScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(currentScene.name);
        }
    }

}
