using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public float speed, jump;
    private bool grounded = true;
    private bool ungrounded = false;
    public GameObject ground;
    private static float epsilon = 0.02f;

    Animator animator;
    SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space)) && grounded) // Later: add jump buffer
        {
            Vector2 v = rb.velocity;
            v.y = jump;
            rb.velocity = v;
        }
    }

    void FixedUpdate()
    {
        if (ungrounded) {
            ungrounded = false;
            if (gameObject.GetComponent<Collider2D>().IsTouching(ground.GetComponent<Collider2D>()))
            {
                grounded = true;
            }
        }
        Vector2 v = rb.velocity;
        if (grounded)
        {
            if (Input.GetKey(KeyCode.A))
            {
                v.x -= speed * Time.deltaTime * 3;
                v.x = Math.Max(v.x, -speed);
            } else if (Input.GetKey(KeyCode.D))
            {
                v.x += speed * Time.deltaTime * 3;
                v.x = Math.Min(v.x, speed);
            } else
            {
                v.x = 0;
            }
        }
        else
        {
            if (Input.GetKey(KeyCode.A))
            {
                v.x -= speed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.D))
            {
                v.x += speed * Time.deltaTime;
            }
        }
        rb.velocity = v;

        if (Math.Abs(rb.velocity.x - 0) < epsilon)
        {
            animator.SetFloat("xVelocity", -1);
        }
        else
        {
            animator.SetFloat("xVelocity", 1);
        }
        if (rb.velocity.x > epsilon)
        {
            spriteRenderer.flipX = false;
        }
        else if (rb.velocity.x < -epsilon)
        {
            spriteRenderer.flipX = true;
        }

    }

    public void UnGround()
    {
        grounded = false;
        ungrounded = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            grounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            grounded = false;
        }
    }
}
