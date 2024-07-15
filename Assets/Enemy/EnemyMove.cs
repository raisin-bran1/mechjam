using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    public Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 v = rb.velocity;
        v.x = 0;
        if (rb.position.x > 0)
        {
            v.x -= 3;
        } else
        {
            v.x += 3;
        }
        rb.velocity = v;
    }
}
