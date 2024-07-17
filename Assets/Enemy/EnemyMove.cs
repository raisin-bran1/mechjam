using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    public Rigidbody2D rb;
    bool moving = true;
    private float rand;
    public float speed;
    private static float epsilon = 0.1f;

    Animator animator;
    SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    public virtual void Start()
    {
        rand = UnityEngine.Random.Range(2, 10);
        animator = gameObject.GetComponent<Animator>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    public virtual void Update()
    {

    }

    public virtual void FixedUpdate()
    {
        if (Time.fixedTime % rand <= 1)
        {
            rand = UnityEngine.Random.Range(2, 10);
            moving = !moving;
        }
        if (moving)
        {
            Vector2 v = rb.velocity;
            v.x = 0;
            if (rb.position.x > 0)
            {
                v.x -= speed;
            }
            else
            {
                v.x += speed;
            }
            rb.velocity = v;
        }

        if (rb.velocity.x - 0 < epsilon)
        {
            animator.SetFloat("xVelocity", -1);
        } else
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

}
