using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base_interaction : MonoBehaviour
{
    public const float startingMaxHealth = 100;
    private float health = startingMaxHealth;
    private float maxHealth = startingMaxHealth;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            Debug.Log("You Lose");
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            EnemyCombat ec = collision.gameObject.GetComponent<EnemyCombat>();
            health -= ec.damage;
            ec.Kill();
        }
    }

    public float GetHealth()
    {
        return health;
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }
}
