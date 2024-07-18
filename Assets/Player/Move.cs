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
    private Collider2D[] collisions = new Collider2D[20];

    GameObject trigger;

    private float step = 0;
    public AudioClip stomp, longstomp;
    
    GameObject sprite;
    Animator animator;
    SpriteRenderer spriteRenderer;
    BoxCollider2D col;
    Combat combat;

    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        sprite = gameObject.transform.GetChild(0).gameObject;
        col = gameObject.GetComponent<BoxCollider2D>();
        fuel = maxFuel;
        trigger = GameObject.Find("PlayerTrigger");
        combat = gameObject.GetComponent<Combat>();
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

        step += Time.deltaTime;
        if (Math.Abs(rb.velocity.x) > 0 && step > 0.75 && grounded)
        {
            step = 0;
            GameObject.FindWithTag("MainCamera").GetComponent<Screenshake>().shake = 0.05f;
            AudioSource.PlayClipAtPoint(stomp, transform.position, 0.25f);
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
            Vector2 off = col.offset;
            off.x = Math.Abs(off.x);
            col.offset = off;
        }
        else if (rb.velocity.x < -epsilon)
        {
            Vector3 scale = sprite.transform.localScale;
            scale.x = -1;
            sprite.transform.localScale = scale;
            Vector2 off = col.offset;
            off.x = -Math.Abs(off.x);
            col.offset = off;
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
            int cols = trigger.GetComponent<CapsuleCollider2D>().OverlapCollider(new ContactFilter2D().NoFilter(), collisions);
            for (int i = 0; i < cols; i++)
            {
                Collider2D col = collisions[i];
                if (col.gameObject.tag == "Enemy")
                {
                    combat.AddEnergy(col.gameObject.GetComponent<EnemyCombat>().Damage(collision.relativeVelocity.magnitude, 0));
                    if (!col.gameObject.GetComponent<EnemyCombat>().dead)
                    {
                        col.gameObject.GetComponent<EnemyMove>().Ragdoll(3.0f);
                        Vector2 v = gameObject.transform.position + new Vector3(0, -12, 0) - col.gameObject.transform.position;
                        v = -v;
                        v.y = Math.Max(v.y, 0);
                        v.Normalize();
                        v *= 20;
                        Rigidbody2D r = col.gameObject.GetComponent<Rigidbody2D>();
                        r.velocity += v;
                        r.angularVelocity = UnityEngine.Random.Range(-300.0f, 300.0f);
                    }
                }
            }
            GameObject.FindWithTag("MainCamera").GetComponent<Screenshake>().shake = 0.1f;
            AudioSource.PlayClipAtPoint(stomp, transform.position, 0.35f);
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
