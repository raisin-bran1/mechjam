using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public float speed;
    public float jump;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector2 v = rb.velocity;
        v.x = 0;
        if (Input.GetKey(KeyCode.A))
        {
            v.x -= speed;
        }
        if (Input.GetKey(KeyCode.D))
        {
            v.x += speed;
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            v.y = jump;
        }
        rb.velocity = v;
    }
}
