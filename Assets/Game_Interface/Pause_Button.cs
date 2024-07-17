using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pause_Button : MonoBehaviour
{
    public Sprite pause, play;
    public Image blackscreen;

    // Start is called before the first frame update
    void Start()
    {
        Activate();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void TogglePause(float opacity)
    {
        if (Time.timeScale == 1)
        {
            Time.timeScale = 0;
            GetComponent<Image>().sprite = play;
            blackscreen.color = new Color(0, 0, 0, opacity);
        } else if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
            GetComponent<Image>().sprite = pause;
            blackscreen.color = new Color(0, 0, 0, 0);
        }
    }

    public void NormalPause()
    {
        TogglePause(0.5f);
    }

    public void Deactivate()
    {
        GetComponent<Button>().enabled = false;
    }

    public void Activate()
    {
        GetComponent<Button>().enabled = true;
    }
}
