using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyCombat : MonoBehaviour
{
    public float damage, health;
    private float damageGradient = 0;
    private static float damageTime = 0.5f;
    private bool started = false;
    public bool dead = false;
    public float energyGiven;
    public AudioClip hurt, pop;

    public GameObject explosion;

    GameObject player;
    BoxCollider2D playerCollider;
    BoxCollider2D coll;
    Rigidbody2D rb;
    EnemyMove move;
    SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    public virtual void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        started = true;
        move = gameObject.GetComponent<EnemyMove>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        coll = gameObject.GetComponent<BoxCollider2D>();
        player = GameObject.Find("Player");
        playerCollider = player.GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    public virtual void Update()
    {
        if (damageGradient > 0) {
            UpdateDamageColor();
        }
    }

    public void FixedUpdate()
    {

    }

    public void Kill(int type)
    {
        dead = true;
        AudioSource.PlayClipAtPoint(pop, transform.position, 1);
        if (type == 0)
        {
            move.Kill(0);
            Destroy(gameObject, 4.0f);
            spriteRenderer.sortingLayerName = "Super Foreground";
            gameObject.layer = 6;
        }
        if (type == 1)
        {
            GameObject e = Instantiate(explosion, gameObject.transform.position, Quaternion.identity);
            e.GetComponent<Animator>().Play("Explosion", -1, 0f);
            GameObject.FindWithTag("MainCamera").GetComponent<Screenshake>().shake = 0.1f;
            Destroy(e, 0.35f);
            Destroy(gameObject);
        }
        if (type == 2)
        {
            move.Kill(1);
            Destroy(gameObject, 0.417f);
            Color c = spriteRenderer.color;
            c.a = 0f;
            spriteRenderer.color = c;
            gameObject.transform.GetChild(0).GetComponent<Animator>().SetBool("Death", true);
            gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.white;
        }
    }

    private void UpdateDamageColor()
    {
        damageGradient = Math.Max(damageGradient - Time.deltaTime, 0);
        Color c = gameObject.GetComponent<SpriteRenderer>().color;
        c.g = (damageTime - damageGradient) / damageTime;
        c.b = (damageTime - damageGradient) / damageTime;
        gameObject.GetComponent<SpriteRenderer>().color = c;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!dead && collision.gameObject.tag == "Enemy" && collision.gameObject.GetComponent<EnemyMove>().GetRagdoll() > 0)
        {
            move.Ragdoll(0.3f);
            Vector2 v = collision.gameObject.transform.position + new Vector3(0, -1, 0) - gameObject.transform.position;
            v = -v;
            v.y = Math.Max(v.y, 0);
            v.Normalize();
            v *= 6;
            rb.velocity = v;
            collision.gameObject.GetComponent<Rigidbody2D>().velocity += v;
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (!dead && collider is BoxCollider2D && collider.gameObject.transform.parent != null && collider.gameObject.transform.parent.gameObject.tag == "Player")
        {
            if (player.GetComponent<Rigidbody2D>().velocity.y > -5)
            {
                move.Ragdoll(3.0f);
                player.GetComponent<Combat>().Damage(damage);
                Vector2 v = player.transform.position + new Vector3(0, -10, 0) - gameObject.transform.position;
                /*v.Normalize();
                v *= 10;
                v.y *= 2;
                collision.gameObject.GetComponent<PlayerMovement>().UnGround();
                collision.gameObject.GetComponent<Rigidbody2D>().velocity += v;*/
                v = -v;
                v.y = Math.Max(v.y, 0);
                v.Normalize();
                v *= 20;
                rb.velocity += v;
                AudioSource.PlayClipAtPoint(hurt, transform.position, 0.5f);
            }
        }
    }

    public float Damage(float damage, int type)
    {
        if (!dead && started)
        {
            health -= damage;
            spriteRenderer.color = Color.red;
            damageGradient = damageTime;
            if (health <= 0)
            {
                Kill(type);
                return energyGiven;
            }
            return 0;
        }
        return 0;
    }

}
