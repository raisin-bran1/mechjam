using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : MonoBehaviour
{
    private enum Weapon {melee, ranged, laser}
    private Weapon weapon = Weapon.melee;
    public GameObject missile;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
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

        if (Input.GetMouseButtonDown(0) && weapon == Weapon.ranged)
        {
            spawnMissile();
        }
    }

    public void spawnMissile()
    {
        Instantiate(missile, transform.position, Quaternion.identity);
    }
}
