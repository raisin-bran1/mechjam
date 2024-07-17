using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 20, damage = 5;
    public Rigidbody2D rb;
    private Camera cam;
    public GameObject explosion;
    private Collider2D[] collisions = new Collider2D[20];
    private bool hasCollided = false;
    [SerializeField] AudioClip explode;

    GameObject player;
    Combat combat;

    // Start is called before the first frame update
    void Start()
    {
        Physics2D.IgnoreCollision(GameObject.FindWithTag("Player").GetComponent<Collider2D>(), GetComponent<Collider2D>());
        rb = GetComponent<Rigidbody2D>();
        cam = Camera.main;
        Vector3 point = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = new Vector2(point.x - transform.position.x, point.y - transform.position.y);
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * 180 / Mathf.PI;
        transform.rotation = Quaternion.Euler(Vector3.forward * angle);
        direction *= speed;
        rb.velocity = direction;

        player = GameObject.Find("Player");
        combat = player.GetComponent<Combat>();
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
        if (!hasCollided)
        {
            if (collision.gameObject.tag == "Enemy")
            {
                // Damage enemy
                combat.AddEnergy(collision.gameObject.GetComponent<EnemyCombat>().Damage(damage));
            }
            //make explosion
            GameObject e = Instantiate(explosion, gameObject.transform.position, Quaternion.identity);
            e.GetComponent<Animator>().Play("Explosion", -1, 0f);
            AudioSource.PlayClipAtPoint(explode, transform.position, 1);
            //explosion damage
            int cols = e.GetComponent<Collider2D>().OverlapCollider(new ContactFilter2D().NoFilter(), collisions);
            for (int i = 0; i < cols; i++)
            {
                Collider2D col = collisions[i];
                if (col.gameObject.tag == "Enemy")
                {
                    combat.AddEnergy(col.gameObject.GetComponent<EnemyCombat>().Damage(damage));
                }
            }
            Destroy(e, 0.35f);
            hasCollided = true;
        }
        Destroy(gameObject);
    }
}
