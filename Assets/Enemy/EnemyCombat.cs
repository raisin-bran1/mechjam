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
    public bool dead = false;
    public float energyGiven;

    EnemyMove move;
    SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    public virtual void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        started = true;
        move = gameObject.GetComponent<EnemyMove>();
    }

    // Update is called once per frame
    public virtual void Update()
    {
        if (!dead && damageGradient > 0) {
            UpdateDamageColor();
        }
    }

    public void Kill()
    {
        dead = true;
        move.Kill();
        Destroy(gameObject, 4.0f);
        spriteRenderer.color = Color.white;
        spriteRenderer.sortingLayerName = "Super Foreground";
        gameObject.layer = 6;
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
        if (!dead && collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<Combat>().Damage(damage);
            Vector2 v = collision.gameObject.transform.position - gameObject.transform.position;
            v.Normalize();
            v *= 10;
            v.y *= 2;
            collision.gameObject.GetComponent<PlayerMovement>().UnGround();
            collision.gameObject.GetComponent<Rigidbody2D>().velocity += v;
        }
    }

    public float Damage(float damage)
    {
        if (!dead && started)
        {
            health -= damage;
            spriteRenderer.color = Color.red;
            damageGradient = damageTime;
            if (health <= 0)
            {
                Kill();
                return energyGiven;
            }
            return 0;
        }
        return 0;
    }

}
