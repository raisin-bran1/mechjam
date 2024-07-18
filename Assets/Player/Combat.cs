using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Combat : MonoBehaviour
{
    public GameObject missile, laser;
    private float cooldown = 0.0f;
    public float damage;
    public const float startingMaxEnergy = 10;
    public const float startingMaxHealth = 10;
    private float maxHealth = startingMaxHealth;
    public float health = startingMaxHealth;
    private float invincibility = 0;
    private static float damageTime = 0.5f;
    private float damageGradient;
    private bool beaming = false;
    private bool lasering = false;
    private bool recovering = false;
    private float speed;
    private float energy = 0;
    private float maxEnergy = startingMaxEnergy;
    public GameObject deathscreen;
    public bool dead = false;
    private int level = 0;
    public AudioClip ding;
    private float deathTime;

    public PlayerMovement move;
    public Upgrade upgrade;
    public GameObject explosion;

    Animator animator;
    PlayerMovement movement;
    SpriteRenderer spriteRenderer;
    BoxCollider2D col;
    GameControl t;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        movement = gameObject.GetComponent<PlayerMovement>();
        speed = movement.speed;
        animator = gameObject.GetComponent<Animator>();
        col = gameObject.GetComponent<BoxCollider2D>();
        GetComponent<Rigidbody2D>().WakeUp();
        animator.SetFloat("health", health);
        t = GameObject.FindWithTag("GameController").GetComponent<GameControl>();
    }

    // Update is called once per frame
    void Update()
    {
        if (level == 9)
        {
            t.LoadScene("Win");
        }

        if (!dead)
        {
            if (damageGradient > 0)
            {
                UpdateDamageColor();
            }
            cooldown -= Time.deltaTime;
            invincibility = Math.Max(invincibility - Time.deltaTime, 0);


            if (Input.GetMouseButton(0) && energy >= 0.5)
            {
                if (!lasering && !beaming && !recovering)
                {
                    if (cooldown <= 0)
                    {
                        SpawnMissile();
                        cooldown = 0.5f;
                        energy -= 0.5f;
                    }
                }
            }
            if (Input.GetMouseButton(1) && energy > 0)
            {
                if (!lasering && cooldown <= 0)
                {
                    lasering = true;
                    animator.SetBool("lasering", true);
                    movement.speed = 0;
                    cooldown = 0.85f;
                }
                else
                {
                    if (cooldown <= 0 && !beaming)
                    {
                        SpawnLaser();
                        beaming = true;
                        animator.SetBool("beaming", true);
                    }
                    else if (beaming)
                    {
                        energy -= Time.deltaTime;
                        energy = Math.Max(energy, 0);
                    }
                }
            }
            else
            {
                if (lasering && cooldown <= 0)
                {
                    lasering = false;
                    beaming = false;
                    animator.SetBool("lasering", false);
                    animator.SetBool("beaming", false);
                    recovering = true;
                    cooldown = 0.8f;
                    Destroy(GameObject.FindWithTag("Laser"));
                    Vector2 s = col.size;
                    s.x = 0.95f;
                    col.size = s;
                    Vector2 o = col.offset;
                    o.x = 0;
                    col.offset = o;
                }
            }
            if (recovering && cooldown <= 0)
            {
                recovering = false;
                movement.speed = speed;
            }
        } else
        {
            if (Time.fixedTime - deathTime < 1.0f && UnityEngine.Random.Range(0.0f, 1.0f) < 0.01f)
            {
                GameObject e = Instantiate(explosion, gameObject.transform.position + new Vector3(UnityEngine.Random.Range(-1.0f, 1.0f), UnityEngine.Random.Range(-1.0f, 1.0f), 0), Quaternion.identity);
                e.GetComponent<Animator>().Play("Explosion", -1, 0f);
                GameObject.FindWithTag("MainCamera").GetComponent<Screenshake>().shake = 0.1f;
                Destroy(e, 0.35f);
            }
        }

        if (health <= 0 && !dead)
        {
            dead = true;
            StartCoroutine(GameOver());
            deathTime = Time.fixedTime;
            move.speed = 0;
        }

        if (!dead && Input.GetKeyDown(KeyCode.F) && energy >= maxEnergy - 5)
        {
            energy -= maxEnergy - 5;
            level++;
            maxHealth += 5;
            health += 5;
            if (level % 3 == 0)
            {
                upgrade.AdvanceStage();
            }
            damage += 1;
            maxEnergy += 5;
            AudioSource.PlayClipAtPoint(ding, transform.position, 1);
        }
    }

    public float GetFlip()
    {
        return move.GetFlip();
    }

    public void SetFlip(float flip)
    {
        move.SetFlip(flip);
    }

    private void UpdateDamageColor()
    {
        damageGradient = Math.Max(damageGradient - Time.deltaTime, 0);
        Color c = gameObject.GetComponent<SpriteRenderer>().color;
        c.g = (damageTime - damageGradient) / damageTime;
        c.b = (damageTime - damageGradient) / damageTime;
        gameObject.GetComponent<SpriteRenderer>().color = c;
    }

    public void SpawnMissile()
    {
        Instantiate(missile, transform.position, Quaternion.identity);
    }

    public void SpawnLaser()
    {
        Vector3 pos = gameObject.transform.position;
        pos.y -= 0.5f;
        Vector2 o = col.offset;
        if (GetFlip() == 1)
        {
            o.x = 0.7f;
            pos.x += 5.4f;
        }
        else
        {
            o.x = -0.7f;
            pos.x -= 5.4f;
        }
        col.offset = o;
        Instantiate(laser, pos, Quaternion.identity);
        Vector2 s = col.size;
        s.x = 2.0f;
        col.size = s;
    }

    public int Damage(float d)
    {
        if (invincibility == 0) {
            health -= d;
            animator.SetFloat("health", health);
            invincibility = damageTime;
            damageGradient = damageTime;
            spriteRenderer.color = Color.red;
            if (health <= 0)
            {
                Debug.Log("You lose!");
                return -1;
            }
        }
        return 0;
    }

    public void AddEnergy(float e)
    {
        energy = Math.Min(maxEnergy, energy + e);
    }

    public float GetHealth()
    {
        return health;
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }

    public float GetEnergy()
    {
        return energy;
    }

    public float GetMaxEnergy()
    {
        return maxEnergy;
    }

    IEnumerator GameOver()
    {
        GetComponent<Rigidbody2D>().Sleep();
        yield return new WaitForSeconds(2);
        Pause_Button pausebutton = GameObject.FindWithTag("Pause").GetComponent<Pause_Button>();
        pausebutton.TogglePause(0.95f);
        pausebutton.Deactivate();
        Instantiate(deathscreen);
    }
    public int GetLevel()
    {
        return level;
    }

}
