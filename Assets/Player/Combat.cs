using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Combat : MonoBehaviour
{
    private enum Weapon {melee, ranged, laser}
    private Weapon weapon = Weapon.melee;
    public GameObject missile, laser;
    public Text hpDisplay;
    private float cooldown = 1000;
    public float health, maxHealth, damage;
    private float invincibility = 0;
    private static float damageTime = 0.5f;
    private float damageGradient;

    SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (damageGradient > 0)
        {
            UpdateDamageColor();
        }
        cooldown += Time.deltaTime;
        invincibility = Math.Max(invincibility - Time.deltaTime, 0);
        if (Input.GetKeyDown(KeyCode.Alpha1) && cooldown >= 3)
        {
            weapon = Weapon.melee;
        } else if (Input.GetKeyDown(KeyCode.Alpha2) && cooldown >= 3)
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
                if (cooldown >= 0.5)
                {
                    SpawnMissile();
                    cooldown = 0;
                }
            } else if (weapon == Weapon.laser)
            {
                if (cooldown >= 3)
                {
                    SpawnLaser();
                    cooldown = 0;
                }
            }
        }

        if (cooldown >= 3)
        {
            Destroy(GameObject.FindWithTag("Laser"));
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
        hpDisplay.text = "HP: " + ((float) (int) (health*10) / 10).ToString();
        if (health <= 0)
        {
            Debug.Log("You lose!");
            return -1;
        }
        return 0;
    }

}
