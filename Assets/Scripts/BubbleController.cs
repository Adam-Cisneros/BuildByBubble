using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BubbleController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb2d;
    [SerializeField] private SpriteRenderer spriteRenderer;

    public static int numBubbles = 0;

    //Bubble Movement
    float currentHorizontalInput = 0f;
    private float movement = 0f;
    [SerializeField] public float bubbleSpeed;
    [SerializeField] private float accelerationRate;
    private float tempYVelocity;
    [SerializeField] private float forcedYVelocity;

    //Bubble GetStuck
    [SerializeField] ParticleSystem bubbleCollisionPS;
    private int numCollisions = 0;

    //StuckBubble
    public bool isStuck;
    [SerializeField] private Sprite dirtyBubbleSprite;
    [SerializeField] private Sprite healthyBubbleSprite;
    private List<GameObject> chain;

    void Start()
    {
        numBubbles += 1;
        isStuck = false;
        chain = new List<GameObject>();
    }

    void Update()
    {
        //Stop if attatched to object
        if (!isStuck)
        {
            getMovement();
        }

        if (gameObject.tag == "DirtyBubble")
        {
            foreach (var elem in chain)
            {
                elem.GetComponent<BubbleController>().convertToDirty();
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
        numCollisions++;
        gameObject.GetComponent<Animator>().enabled = false;
        spriteRenderer.sprite = healthyBubbleSprite;

        //Create fixed joint with sticky surface when collide with sticky surface
        if (col.gameObject.tag == "StickySurface")
        {
            SFXManager.Instance.PlaySFX("HealthyBubbleSFX");
            playCleanCollisionPS();
            createFixedJoint(col);
        }

        //Create hinge with bubble when collide
        if (col.gameObject.tag == "DirtySurface")
        {
            createFixedJoint(col);
            chain.Add(gameObject);
            foreach (var elem in chain)
            {
                elem.GetComponent<BubbleController>().convertToDirty();
            }
        }

        //Create hinge with bubble when collide
        if (col.gameObject.tag == "DirtyBubble")
        {
            createHingeJoint(col);
            foreach (var elem in chain)
            {
                elem.GetComponent<BubbleController>().convertToDirty();
            }
        }

        //Create hinge with bubble when collide with other healthy bubble
        if (col.gameObject.tag == "HealthyBubble")
        {
            SFXManager.Instance.PlaySFX("HealthyBubbleSFX");
            playCleanCollisionPS();
            createHingeJoint(col);
            chain.Add(col.gameObject);
            chain.Add(gameObject);
        }
    }

    private void playCleanCollisionPS()
    {
        if (numCollisions == 1)
        {
            bubbleCollisionPS.Play();
        }
    }

    private void createFixedJoint(Collision2D col)
    {
        isStuck = true;
        FixedJoint2D fixedJoint = gameObject.AddComponent<FixedJoint2D>();

        fixedJoint.connectedBody = col.rigidbody;

        fixedJoint.breakForce = 10000f;
        fixedJoint.breakTorque = 10000f;

    }

    private void createHingeJoint(Collision2D col)
    {
        isStuck = true;
        HingeJoint2D hingeJoint = gameObject.AddComponent<HingeJoint2D>();

        hingeJoint.connectedBody = col.rigidbody;

        hingeJoint.breakForce = 1000f;
        hingeJoint.breakTorque = 1000f;

    }


    public void convertToDirty()
    {
        SFXManager.Instance.PlaySFX("DirtyBubbleSFX");
        gameObject.tag = "DirtyBubble";
        spriteRenderer.sprite = dirtyBubbleSprite;
    }


    private void FixedUpdate()
    {
        //Move by collected movement
        rb2d.velocity = new Vector2(movement, rb2d.velocity.y);

    }




}

