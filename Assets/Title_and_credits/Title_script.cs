using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Start_button : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BeginGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void OpenCredits()
    {
        SceneManager.LoadScene("Credits");
    }
}
