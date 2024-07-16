using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 20, damage = 5;
    public Rigidbody2D rb;
    private Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        Physics2D.IgnoreCollision(GameObject.FindWithTag("Player").GetComponent<Collider2D>(), GetComponent<Collider2D>());
        rb = GetComponent<Rigidbody2D>();
        cam = Camera.main;
        Vector3 point = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = new Vector2(point.x - transform.position.x, point.y - transform.position.y);
        direction.Normalize();
        direction *= speed;
        rb.velocity = direction;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.magnitude > 50)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "Player")
        {
            if (collision.gameObject.tag == "Enemy")
            {
                // Damage enemy
                EnemyCombat ec = collision.gameObject.GetComponent<EnemyCombat>();
                ec.health -= damage;
                if (ec.health <= 0)
                {
                    Destroy(collision.gameObject);
                }
            }
            Destroy(gameObject);
        }
    }
}
