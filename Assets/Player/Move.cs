using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public float speed, jump, jet, maxFuel;
    private bool grounded = true;
    private int ungrounded = 0;
    private static float epsilon = 0.02f;
    private float fuel;
    
    GameObject sprite;
    Animator animator;
    SpriteRenderer spriteRenderer;
    BoxCollider2D collider;

    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        sprite = gameObject.transform.GetChild(0).gameObject;
        collider = gameObject.GetComponent<BoxCollider2D>();
        fuel = maxFuel;
    }

    // Update is called once per frame
    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space)) && grounded && speed > 0) // Later: add jump buffer
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
                animator.SetBool("grounded", true);
            }
            ungrounded -= 1;
        }
        if (!grounded)
        {
            Vector2 direction = rb.velocity;
            if (direction.y > 0)
            {
                float angle = Mathf.Atan2(1000, direction.x * Math.Max(direction.y, 1)) * Mathf.Rad2Deg;
                sprite.transform.rotation = Quaternion.Euler(Vector3.forward * angle) * Quaternion.AngleAxis(90, -Vector3.forward);
            } else
            {
                float angle = Mathf.Atan2(3000, -direction.x * Math.Max(-direction.y, 1)) * Mathf.Rad2Deg;
                sprite.transform.rotation = Quaternion.Euler(Vector3.forward * angle) * Quaternion.AngleAxis(90, -Vector3.forward);
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
            if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.Space)) && fuel > 0)
            {
                fuel -= Time.deltaTime;
                v.y += jet * Time.deltaTime;
                animator.SetBool("jet", true);
            } else
            {
                animator.SetBool("jet", false);
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
            Vector2 off = collider.offset;
            off.x = Math.Abs(off.x);
            collider.offset = off;
        }
        else if (rb.velocity.x < -epsilon)
        {
            Vector3 scale = sprite.transform.localScale;
            scale.x = -1;
            sprite.transform.localScale = scale;
            Vector2 off = collider.offset;
            off.x = -Math.Abs(off.x);
            collider.offset = off;
        }

    }

    public void SetFlip(float flipped)
    {
        Vector3 scale = sprite.transform.localScale;
        scale.x = flipped;
        sprite.transform.localScale = scale;
    }

    public float GetFlip()
    {
        return sprite.transform.localScale.x;
    }

    public void UnGround()
    {
        if (grounded)
        {
            ungrounded = 3;
        }
        grounded = false;
        animator.SetBool("grounded", false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            grounded = true;
            animator.SetBool("grounded", true);
            animator.SetBool("jet", false);
            ungrounded = 0;
            fuel = maxFuel;
            sprite.transform.rotation = Quaternion.identity;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            grounded = false;
            animator.SetBool("grounded", false);
            ungrounded = 0;
        }
    }
}
