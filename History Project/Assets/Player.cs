using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float speed = 5f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] LayerMask layerMask;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private PlayerInput input;
    private float xVelocity = 0f;
    private float yVelocity = 0f;
    //private float gravVelocity = -9.81f;
    private float inputX = 0f;

    private float inputY = 0f;
    private bool isJumping = false;
    private float jumpDuration = 0f;
    private float maxJumpTime = 0.3f;

    private float rayLength = 0.01f;
    private float offsetY = 0.005f;
    private float offsetX = 0.05f;
    private float width;
    private float height;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        width = GetComponent<Collider2D>().bounds.extents.x;
        height = GetComponent<Collider2D>().bounds.extents.y + 0.01f;
    }

    // Update is called once per frame
    void Update()
    {
        inputX = Input.GetAxis("Horizontal");
        inputY = Input.GetAxis("Vertical");

        FlipPlayer();

        JumpBehaviour();

    }

    private void FixedUpdate()
    {
        Movement();
        JumpMovement();

        //if (true)
        //{
        //    BlockBehaviour();
        //}
    }

    private void JumpMovement()
    {
        if (isJumping && jumpDuration < maxJumpTime)
        {
            rb.velocity = new Vector2(xVelocity, jumpSpeed);
        }
    }

    private void Movement()
    {
        if (inputX == 0)
        {
            xVelocity = 0f;
        }
        else
        {
            xVelocity = inputX * speed;
        }

        rb.velocity = new Vector2(xVelocity, rb.velocity.y);
    }

    private void JumpBehaviour()
    {
        if (IsOnTheGround() && isJumping == false)
        {
            if (inputY > 0f)
            {
                isJumping = true;
            }
        }

        if (isJumping && inputY > 0f)
        {
            jumpDuration += Time.deltaTime;
            if (jumpDuration > maxJumpTime)
            {
                inputY = 0f;
            }
        }
        else
        {
            jumpDuration = 0f;
            isJumping = false;
        }
    }

    private void FlipPlayer()
    {
        if (inputX > 0f)
        {
            sr.flipX = false;
        }
        else if (inputX < 0f)
        {
            sr.flipX = true;
        }
    }

    private bool IsOnTheGround()
    {
        bool rightFoot = Physics2D.Raycast(new Vector2((transform.position.x + width-offsetX), transform.position.y-offsetY),
            Vector2.down, rayLength, layerMask);
        Debug.DrawRay(new Vector2((transform.position.x + width-offsetX), transform.position.y-offsetY),
            Vector2.down, Color.green, rayLength);

        bool leftFoot = Physics2D.Raycast(new Vector2((transform.position.x - width), transform.position.y - offsetY),
            Vector2.down, rayLength, layerMask);
        Debug.DrawRay(new Vector2((transform.position.x - width), transform.position.y-offsetY),
            Vector2.down, Color.green, rayLength);

        if (rightFoot || leftFoot)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Block"))
        {
            Debug.Log("Block touch");
            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();

            //rb.AddForce(Vector2.left * 100);
            BlockBehaviour(rb);
        }
    }

    //block component
    private void BlockBehaviour(Rigidbody2D rigidbody)
    {
        RaycastHit2D rightSide = Physics2D.Raycast(rigidbody.transform.position, Vector2.right, 1);
        RaycastHit2D leftSide = Physics2D.Raycast(rigidbody.transform.position, Vector2.left, 1);

        if (rightSide.collider.CompareTag("Player"))
        {
            Debug.Log("player from the right");
            rigidbody.AddForce(Vector2.left * 100);
        }
        else if(leftSide.collider.CompareTag("Player"))
        {
            rigidbody.AddForce(Vector2.right * 100);
        }
    }
}
