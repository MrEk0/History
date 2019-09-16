using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float speed = 5f;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private PlayerInput input;
    private float xVelocity = 0f;
    private float yVelocity = -9.81f;
    private float inputX = 0f;
    private float inputY = 0f;

    private float rayLength = 0.1f;
    private float width;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        width = GetComponent<Collider2D>().bounds.extents.x + 0.01f;
    }

    // Update is called once per frame
    void Update()
    {
        inputX = Input.GetAxis("Horizontal");
        inputY = Input.GetAxis("Vertical");

        if (inputX > 0)
        {
            sr.flipX = false;
        }
        else if (inputX < 0)
        {
            sr.flipX = true;
        }
    }

    private void FixedUpdate()
    {
        if (inputX == 0)
        {
            xVelocity = 0f;
        }
        else
        {
            xVelocity = inputX * speed;
        }

        rb.velocity = new Vector2(xVelocity, yVelocity);
    }

    private bool IsOnTheGround()
    {
        bool rightFoot = Physics2D.Raycast(new Vector2(transform.position.x + width, transform.position.y), Vector2.down, rayLength);
        Debug.DrawRay(new Vector2(transform.position.x + width, transform.position.y), Vector2.down, Color.green, rayLength);

        bool leftFoot = Physics2D.Raycast(new Vector2(transform.position.x - width, transform.position.y), Vector2.down, rayLength);
        Debug.DrawRay(new Vector2(transform.position.x - width, transform.position.y), Vector2.down, Color.green, rayLength);

        if (rightFoot && leftFoot)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
