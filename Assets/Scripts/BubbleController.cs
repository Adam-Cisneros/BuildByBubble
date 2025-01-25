using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BubbleController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb2d;
    [SerializeField] private SpriteRenderer spriteRenderer;

    public static int numBubbles = 0;

    //Player Movement
    float currentHorizontalInput = 0f;
    private float movement = 0f;
    [SerializeField] public float bubbleSpeed;
    [SerializeField] private float accelerationRate;
    private float tempYVelocity;
    [SerializeField] private float forcedYVelocity;



    //StuckBubble
    public bool isStuck;
    [SerializeField] Sprite dirtyBubbleSprite;
    private List<Collision2D> connectedList = new List<Collision2D>();

    void Start()
    {
        numBubbles += 1;
        isStuck = false;
    }

    void Update()
    {
        //Stop if attatched to object
        if (!isStuck)
        {
            getMovement();
        }


        //Change later to not call all the time
        foreach (var col in connectedList)
        {
            if (col.gameObject.CompareTag("DirtyBubble"))
            {
                convertToDirty();
            }
        }

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


        if (Input.GetKeyDown(KeyCode.S))
        {
            tempYVelocity = rb2d.velocity.y;
            rb2d.velocity = new Vector2(movement, forcedYVelocity);
        }
        else if (Input.GetKeyUp(KeyCode.S))
        {
            rb2d.velocity = new Vector2(movement, tempYVelocity);
        }


        movement = currentHorizontalInput * bubbleSpeed;
    }


    void OnCollisionEnter2D(Collision2D col)
    {

        //Create fixed joint with sticky surface when collide with sticky surface
        if (col.gameObject.tag == "StickySurface")
        {
            createFixedJoint(col);
        }

        //Create hinge with bubble when collide
        if (col.gameObject.tag == "DirtySurface")
        {
            convertToDirty();
            createFixedJoint(col);
        }

        //Create hinge with bubble when collide
        if (col.gameObject.tag == "DirtyBubble")
        {
            convertToDirty();
            createHingeJoint(col);
        }

        //Create hinge with bubble when collide with other healthy bubble
        if (col.gameObject.tag == "HealthyBubble")
        {
            createHingeJoint(col);
        }
    }

    private void createFixedJoint(Collision2D col)
    {
        connectedList.Add(col);
        isStuck = true;
        FixedJoint2D fixedJoint = gameObject.AddComponent<FixedJoint2D>();

        fixedJoint.connectedBody = col.rigidbody;

        fixedJoint.breakForce = 100000f;
        fixedJoint.breakTorque = 100000f;

        Debug.Log("FixedJoint2D created and attached to " + col.gameObject.name);
    }

    private void createHingeJoint(Collision2D col)
    {
        connectedList.Add(col);
        isStuck = true;
        HingeJoint2D hingeJoint = gameObject.AddComponent<HingeJoint2D>();

        hingeJoint.connectedBody = col.rigidbody;

        hingeJoint.breakForce = 1000f;
        hingeJoint.breakTorque = 1000f;

        //Debug.Log("HingeJoint2D created and attached to " + col.gameObject.name);
    }

    private void convertToDirty()
    {
        gameObject.tag = "DirtyBubble";
        spriteRenderer.sprite = dirtyBubbleSprite;
    }


    private void FixedUpdate()
    {
        //Move by collected movement

        rb2d.velocity = new Vector2(movement, rb2d.velocity.y);

    }

}
