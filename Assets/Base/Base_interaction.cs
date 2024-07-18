using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base_interaction : MonoBehaviour
{
    public const float startingMaxHealth = 100;
    private float health = startingMaxHealth;
    private float maxHealth = startingMaxHealth;
    private GameControl t;
    private bool gameover = false;
    public GameObject deathscreen;

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
            ec.Kill(1);
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
    IEnumerator GameOver()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        GameObject.Find("rocket_center").GetComponentInChildren<SpriteRenderer>().enabled = false;
        GameObject.Find("rocket_left").GetComponentInChildren<SpriteRenderer>().enabled = false;
        GameObject.Find("rocket_right").GetComponentInChildren<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds(2);
        Pause_Button pausebutton = GameObject.FindWithTag("Pause").GetComponent<Pause_Button>();
        pausebutton.TogglePause(0.95f);
        pausebutton.Deactivate();
        Instantiate(deathscreen);
    }
}
