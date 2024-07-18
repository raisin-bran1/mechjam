using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : MonoBehaviour
{
    public GameObject missile, laser;
    private float cooldown = 0.0f;
    public float damage;
    public const float startingMaxEnergy = 5;
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

    public PlayerMovement move;
    public Upgrade upgrade;

    Animator animator;
    PlayerMovement movement;
    SpriteRenderer spriteRenderer;
    BoxCollider2D col;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        movement = gameObject.GetComponent<PlayerMovement>();
        speed = movement.speed;
        animator = gameObject.GetComponent<Animator>();
        col = gameObject.GetComponent<BoxCollider2D>();
        GetComponent<Rigidbody2D>().WakeUp();
    }

    // Update is called once per frame
    void Update()
    {

        if (damageGradient > 0)
        {
            UpdateDamageColor();
        }
        cooldown -= Time.deltaTime;
        invincibility = Math.Max(invincibility - Time.deltaTime, 0);

        if (Input.GetMouseButton(0) && energy >= 1)
        {
            if (!lasering && !beaming && !recovering)
            {
                if (cooldown <= 0)
                {
                    SpawnMissile();
                    cooldown = 0.5f;
                    energy -= 1;
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
                } else if (beaming)
                {
                    energy -= Time.deltaTime * 5;
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

        if (health <= 0 && !dead)
        {
            dead = true;
            StartCoroutine(GameOver());
        if (Input.GetKeyDown(KeyCode.P) && energy == maxEnergy)
        {
            energy = 0;
            level++;
            maxHealth += 5;
            health += 5;
            if (level % 3 == 0)
            {
                upgrade.AdvanceStage();
            }
            damage += 1f;
            maxEnergy += 5;
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
