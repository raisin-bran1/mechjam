using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingSheepMovement : EnemyMove
{
    public float height;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

    public override void FixedUpdate()
    {

        base.FixedUpdate();
        if (!dead && rb.transform.position.y < height && Math.Abs(rb.transform.position.x) > 5)
        {
            Vector2 v = rb.velocity;
            v.y = 5;
            rb.velocity = v;
        }

    }

}
