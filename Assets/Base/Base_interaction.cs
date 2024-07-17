using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base_interaction : MonoBehaviour
{
    private float health = 100;
    private GameControl t;
    private bool gameover = false;

    // Start is called before the first frame update
    void Start()
    {
        t = GameObject.FindWithTag("GameController").GetComponent<GameControl>();
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0 && !gameover)
        {
            gameover = true;
            StartCoroutine(GameOver());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            EnemyCombat ec = collision.gameObject.GetComponent<EnemyCombat>();
            health -= ec.damage;
            Destroy(collision.gameObject);
        }
    }

    IEnumerator GameOver()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds(2);
        t.LoadScene("Deathscreen");
    }
}
