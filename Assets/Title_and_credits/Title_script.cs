using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Start_button : MonoBehaviour
{
    private GameControl t;
    // Start is called before the first frame update
    void Start()
    {
        t = GameObject.FindWithTag("GameController").GetComponent<GameControl>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BeginGame()
    {
        t.LoadScene("Game");
    }

    public void OpenCredits()
    {
        t.LoadScene("Credits");
    }
}
