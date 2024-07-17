using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameControl : MonoBehaviour
{
    public GameObject transitionObject, transitionObject1;
    // Start is called before the first frame update
    void Start()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("GameController");

        if (objs.Length > 1)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameObject u = Instantiate(transitionObject1);
        u.GetComponent<Transition>().WaitTransition();
    }

    public void LoadScene(string scene)
    {
        GameObject t = Instantiate(transitionObject);
        Transition script = t.GetComponent<Transition>();
        script.LoadScene(scene);
    }
}
