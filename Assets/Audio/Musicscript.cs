using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Musicscript : MonoBehaviour
{
    public AudioSource music;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale == 0)
        {
            music.volume = 0.25f;
        } else
        {
            music.volume = 1;
        }
    }
}
