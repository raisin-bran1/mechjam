using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgrade : MonoBehaviour
{

    public int stage;

    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        animator.SetInteger("stage", stage);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AdvanceStage()
    {
        stage++;
        animator.SetInteger("stage", stage);
    }

}
