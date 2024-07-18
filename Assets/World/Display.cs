using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Display : MonoBehaviour
{

    public Combat combat;
    public Base_interaction interaction;
    public TMP_Text displayText;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        displayText.text = "HP: " + ((float)(int)(combat.GetHealth() * 10) / 10.0f).ToString() + "/" + ((float)(int)(combat.GetMaxHealth() * 10) / 10.0f).ToString() + "\n" +
            "ENERGY: " + ((float)(int)(combat.GetEnergy() * 10) / 10.0f).ToString() + "/" + ((float)(int)(combat.GetMaxEnergy() * 10) / 10.0f).ToString() + "\n" +
            "BASE HP: " + ((float)(int)(interaction.GetHealth() * 10) / 10.0f).ToString() + "/" + ((float)(int)(interaction.GetMaxHealth() * 10) / 10.0f).ToString() + "\n" + 
            "BASE STAGE: " + combat.GetLevel().ToString() + "/9";
    }
}
