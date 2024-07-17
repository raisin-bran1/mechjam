using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombat : MonoBehaviour
{
    public float damage, health;
    private float damageGradient = 0;
    private static float damageTime = 0.5f;
    private bool started = false;

    SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    public virtual void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        started = true;
    }

    // Update is called once per frame
    public virtual void Update()
    {
        if (damageGradient > 0) {
            UpdateDamageColor();
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
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<Combat>().Damage(damage);
            Vector2 v = collision.gameObject.transform.position - gameObject.transform.position;
            v.Normalize();
            v *= 10;
            v.y *= 2;
            Debug.Log(v.x + " " + v.y);
            //collision.gameObject.GetComponent<PlayerMovement>().UnGround();
            collision.gameObject.GetComponent<Rigidbody2D>().velocity += v;
        }
    }

    public int Damage(float damage)
    {
        if (started)
        {
            health -= damage;
            spriteRenderer.color = Color.red;
            damageGradient = damageTime;
            if (health <= 0)
            {
                Destroy(gameObject);
                return -1;
            }
            return 0;
        }
        return 0;
    }

}
