using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : MonoBehaviour
{
    private enum Weapon {melee, ranged, laser}
    private Weapon weapon = Weapon.melee;
    public GameObject missile, laser;
    private float cooldown = 0.0f;
    public float damage;
    public const float startingMaxEnergy = 10;
    public const float startingMaxHealth = 10;
    private float maxHealth = startingMaxHealth;
    private float health = startingMaxHealth;
    private float invincibility = 0;
    private static float damageTime = 0.5f;
    private float damageGradient;
    private bool lasering;
    private float speed;
    private float energy = 0;
    private float maxEnergy = startingMaxEnergy;

    PlayerMovement movement;
    SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        movement = gameObject.GetComponent<PlayerMovement>();
        speed = movement.speed;
    }

    // Update is called once per frame
    void Update()
    {
        if (lasering && cooldown <= 0)
        {
            lasering = false;
            movement.speed = speed;
            Destroy(GameObject.FindWithTag("Laser"));
        }

        if (damageGradient > 0)
        {
            UpdateDamageColor();
        }
        cooldown -= Time.deltaTime;
        invincibility = Math.Max(invincibility - Time.deltaTime, 0);
        if (Input.GetKeyDown(KeyCode.Alpha1) && cooldown <= 0)
        {
            weapon = Weapon.melee;
        } else if (Input.GetKeyDown(KeyCode.Alpha2) && cooldown <= 0)
        {
            weapon = Weapon.ranged;
        } else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            weapon = Weapon.laser;
        }

        if (Input.GetMouseButton(0))
        {
            if (weapon == Weapon.ranged)
            {
                if (cooldown <= 0)
                {
                    SpawnMissile();
                    cooldown = 0.5f;
                }
            } else if (weapon == Weapon.laser)
            {
                if (cooldown <= 0 && !lasering)
                {
                    lasering = true;
                    movement.speed = 0;
                    SpawnLaser();
                    cooldown = 3.0f;
                }
            }
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

    public void SpawnMissile()
    {
        Instantiate(missile, transform.position, Quaternion.identity);
    }

    public void SpawnLaser()
    {
        Instantiate(laser, transform.position, Quaternion.identity);
    }

    public int Damage(float d)
    {
        health -= d;
        invincibility = damageTime;
        damageGradient = damageTime;
        spriteRenderer.color = Color.red;
        if (health <= 0)
        {
            Debug.Log("You lose!");
            return -1;
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

}
