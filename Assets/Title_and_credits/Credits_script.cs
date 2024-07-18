using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits_script : MonoBehaviour
{
    private GameControl t;
    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        t = GameObject.FindWithTag("GameController").GetComponent<GameControl>();
        player = GameObject.FindWithTag("Player");
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

    public void Continue()
    {
        Destroy(GameObject.FindWithTag("Respawn"));
        Pause_Button pausebutton = GameObject.FindWithTag("Pause").GetComponent<Pause_Button>();
        pausebutton.NormalPause();
        pausebutton.Activate();
        player.GetComponent<Combat>().health = player.GetComponent<Combat>().GetMaxHealth();
        player.GetComponent<Animator>().SetFloat("health", player.GetComponent<Combat>().GetMaxHealth());
        player.GetComponent<Rigidbody2D>().WakeUp();
        player.transform.position = new Vector3(0, 5, 0);
        player.GetComponent<Combat>().dead = false;
        GameObject.FindWithTag("MainCamera").GetComponent<Screenshake>().shake = 0;
    }
}
