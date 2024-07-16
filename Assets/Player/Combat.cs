using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : MonoBehaviour
{
    private enum Weapon {melee, ranged, laser}
    private Weapon weapon = Weapon.melee;
    public GameObject missile, laser;
    private float cooldown = 1000;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        cooldown += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            weapon = Weapon.melee;
        } else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            weapon = Weapon.ranged;
        } else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            weapon = Weapon.laser;
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (weapon == Weapon.ranged)
            {
                spawnMissile();
            } else if (weapon == Weapon.laser)
            {
                if (cooldown >= 3)
                {
                    spawnLaser();
                    cooldown = 0;
                }
            }
        }

        if (cooldown >= 3)
        {
            Destroy(GameObject.FindWithTag("Laser"));
        }
    }

    public void spawnMissile()
    {
        Instantiate(missile, transform.position, Quaternion.identity);
    }

    public void spawnLaser()
    {
        Instantiate(laser, transform.position, Quaternion.identity);
    }
}
