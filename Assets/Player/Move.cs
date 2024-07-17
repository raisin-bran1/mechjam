using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public float speed, jump;
    private bool grounded = true;
    private int ungrounded = 0;
    private static float epsilon = 0.02f;
    
    GameObject sprite;
    Animator animator;
    SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        sprite = gameObject.transform.GetChild(0).gameObject;
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
        if (ungrounded > 0) {
            if (ungrounded == 1)
            {
                grounded = true;
            }
            ungrounded -= 1;
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

        /*if (Math.Abs(rb.velocity.x - 0) < epsilon)
        {
            animator.SetFloat("xVelocity", 0);
        }
        else
        {
            animator.SetFloat("xVelocity", 1);
        }*/
        animator.SetFloat("xVelocity", Math.Abs(rb.velocity.x));
        if (rb.velocity.x > epsilon)
        {
            Vector3 scale = sprite.transform.localScale;
            scale.x = 1;
            sprite.transform.localScale = scale;
        }
        else if (rb.velocity.x < -epsilon)
        {
            Vector3 scale = sprite.transform.localScale;
            scale.x = -1;
            sprite.transform.localScale = scale;
        }

    }

    public void UnGround()
    {
        if (grounded)
        {
            ungrounded = 3;
        }
        grounded = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            grounded = true;
            ungrounded = 0;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            grounded = false;
            ungrounded = 0;
        }
    }
}
