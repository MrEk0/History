using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float speed = 5f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] LayerMask layerMask;
    [SerializeField] float climbSpeed=3f;
    [SerializeField] float crounchSpeed = 3f;
    //[SerializeField] float rayDistance;
    //[SerializeField] LayerMask boxMask;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private BoxCollider2D collider;
    private PlayerInput input;
    private float xVelocity = 0f;
    private float yVelocity = 0f;
    private float inputX = 0f;

    //jump
    private float inputY = 0f;
    private bool isJumping = false;
    private float jumpDuration = 0f;
    private float maxJumpTime = 0.3f;

    //raycast
    private float rayLength = 0.1f;
    private float headOffset = 0.03f;
    private float offsetY = -0.01f;
    private float offsetX = 0.05f;
    private float width;
    private float height;

    //crounch
    private Vector2 crounchSizeCollider;
    private Vector2 startCollider;
    //private Vector2 startScale;
    //private Vector2 crounchScale;
    private Color crounchColor = Color.red;
    private Color startColor;
    private bool isCrounch = false;
    private float startSpeed;
    private float crounchRayLength = 1.3f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<BoxCollider2D>();
        width = GetComponent<Collider2D>().bounds.extents.x;
        height = GetComponent<BoxCollider2D>().size.y;
        sr = GetComponent<SpriteRenderer>();

        crounchSizeCollider = new Vector2(collider.size.x, collider.size.y / 2);
        startSpeed = speed;
        startColor = GetComponent<SpriteRenderer>().color;
        //startScale = transform.localScale;
        //crounchScale = new Vector2(transform.localScale.x, transform.localScale.y / 2);
        startCollider = collider.size;
    }

    // Update is called once per frame
    void Update()
    {
        inputX = Input.GetAxis("Horizontal");
        inputY = Input.GetAxis("Vertical");

        JumpBehaviour();
        Crounch();
    }

    private void FixedUpdate()
    {
        Movement();
        JumpMovement();
        FlipPlayer();
        ClimbRope();
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
        bool isMoving = Mathf.Abs(rb.velocity.x) > Mathf.Epsilon;
        if (isMoving)
        {
            transform.localScale = new Vector2(Mathf.Sign(rb.velocity.x), 1f);
        }
    }

    private bool IsOnTheGround()
    {
        bool rightFoot = Physics2D.Raycast(new Vector2((transform.position.x + width - offsetX), transform.position.y - offsetY),
            Vector2.down, rayLength, layerMask);
        Debug.DrawRay(new Vector2((transform.position.x + width - offsetX), transform.position.y - offsetY),
            Vector2.down, Color.green, rayLength);

        bool leftFoot = Physics2D.Raycast(new Vector2((transform.position.x - width+offsetX), transform.position.y - offsetY),
            Vector2.down, rayLength, layerMask);
        Debug.DrawRay(new Vector2((transform.position.x - width+offsetX), transform.position.y - offsetY),
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

    private void ClimbRope()
    {
        if (!collider.IsTouchingLayers(LayerMask.GetMask("Climb")))
        {
            rb.gravityScale = 1;
            isCrounch = false;
            return;
        }

        isCrounch = true; ;
        rb.gravityScale = 0;
        Vector2 climbVelocity = new Vector2(rb.velocity.x, climbSpeed * inputY);
        rb.velocity = climbVelocity;
    }

    private void Crounch()
    {
        RaycastHit2D headhit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + height/2 + headOffset),
            Vector2.up, crounchRayLength, layerMask);
        Debug.DrawRay(new Vector2(transform.position.x, transform.position.y + height/2 + headOffset), 
            Vector2.up, Color.red, crounchRayLength);
        //Debug.Log(headhit.collider);

        if (Input.GetKeyDown(KeyCode.R) && !isCrounch)
        {
            collider.size = crounchSizeCollider;
            sr.color = crounchColor;
            //transform.localScale = crounchScale;
            speed = crounchSpeed;
        }

        if (Input.GetKeyUp(KeyCode.R) && headhit.collider==null)
        {
            speed = startSpeed;
            collider.size = startCollider;
            sr.color = startColor;
        }
    }
}
