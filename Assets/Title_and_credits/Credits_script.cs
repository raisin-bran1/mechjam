using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits_script : MonoBehaviour
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

    public void OpenTitle()
    {
        Time.timeScale = 1;
        t.LoadScene("Title");
    }
}
