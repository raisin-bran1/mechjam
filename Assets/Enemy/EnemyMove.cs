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
    public static float epsilon = 0.1f;
    public bool dead = false;

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
        if (!dead)
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

            if (Math.Abs(rb.velocity.x - 0) < epsilon)
            {
                animator.SetFloat("xVelocity", 0);
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

    }

    public void Kill()
    {
        Vector2 v = new Vector2();
        v.y = 10 + UnityEngine.Random.Range(-3.0f, 3.0f);
        v.x = UnityEngine.Random.Range(-4.0f, 4.0f);
        rb.velocity = v;
        rb.constraints = RigidbodyConstraints2D.None;
        rb.angularVelocity = UnityEngine.Random.Range(-300.0f, 300.0f);
        dead = true;
    }

    public float GetRand()
    {
        return rand;
    }

    public void SetRand(float r)
    {
        rand = r;
    }

    public void InvertMove()
    {
        moving = !moving;
    }

    public bool IsMoving()
    {
        return moving;
    }

    public void SetFloat(string name, float value)
    {
        animator.SetFloat(name, value);
    }

    public void SetBool(string name, bool value)
    {
        animator.SetBool(name, value);
    }

    public void SetFlipX(bool flipX)
    {
        spriteRenderer.flipX = flipX;
    }

}
