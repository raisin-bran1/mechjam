using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Hover : MonoBehaviour
{
    TMP_Text text;
    // Start is called before the first frame update
    void Start()
    {
        text = gameObject.GetComponentInChildren<TMP_Text>();
        if (text == null)
        {
            Debug.Log("HI");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void IncreaseFont()
    {
        text.fontSize = 100;
    }

    public void DecreaseFont()
    {
        text.fontSize = 72;
    }
}
