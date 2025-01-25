using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BubbleController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb2d;

    public static int numBubbles = 0;

    //Player Movement
    float currentHorizontalInput = 0f;
    private float movement = 0f;
    [SerializeField] public float bubbleSpeed;
    [SerializeField] private float accelerationRate;

    //StuckBubble
    public bool isStuck;

    void Start()
    {
        numBubbles += 1;
        isStuck = false;
    }

    void Update()
    {
        getMovement();
    }

    private void getMovement()
    {
        if (Input.GetKey(KeyCode.A))
        {
            currentHorizontalInput = Mathf.MoveTowards(currentHorizontalInput, -1f, accelerationRate * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            currentHorizontalInput = Mathf.MoveTowards(currentHorizontalInput, 1f, accelerationRate * Time.deltaTime);
        }
        else
        {
            currentHorizontalInput = Mathf.MoveTowards(currentHorizontalInput, 0, accelerationRate * Time.deltaTime);
        }

        movement = currentHorizontalInput * bubbleSpeed;
    }


    void OnCollisionEnter2D(Collision2D col)
    {
        //Create fixed joint with sticky surface when collide
        if (col.gameObject.tag == "StickySurface")
        {
            isStuck = true;
            FixedJoint2D fixedJoint = gameObject.AddComponent<FixedJoint2D>();

            fixedJoint.connectedBody = col.rigidbody;

            fixedJoint.breakForce = 1000f; 
            fixedJoint.breakTorque = 1000f; 

            Debug.Log("FixedJoint2D created and attached to " + col.gameObject.name);
        }

        //Create hinge with bubble when collide
        if (col.gameObject.tag == "HealthyBubble")
        {
            isStuck = true;
            HingeJoint2D hingeJoint = gameObject.AddComponent<HingeJoint2D>();

            hingeJoint.connectedBody = col.rigidbody;

            hingeJoint.breakForce = 1000f;
            hingeJoint.breakTorque = 1000f; 

            Debug.Log("HingeJoint2D created and attached to " + col.gameObject.name);
        }
    }


    private void FixedUpdate()
    {
        //Move by collected movement and stop if attatched to object
        if (!isStuck)
        {
            rb2d.velocity = new Vector2(movement, rb2d.velocity.y);
        }

    }

}
