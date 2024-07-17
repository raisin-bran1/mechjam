using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstralSheepMovement : EnemyMove
{

    public float t = 0;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        base.SetRand(UnityEngine.Random.Range(5, 15));
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

    public override void FixedUpdate()
    {
        if (Time.fixedTime > t)
        {
            t = Time.fixedTime + UnityEngine.Random.Range(5, 10);
            base.SetBool("jumping", false);
            Vector2 v = rb.velocity;
            v.x = 0;
            v.y = 10;
            if (rb.position.x > 0)
            {
                v.x -= speed;
            }
            else
            {
                v.x += speed;
            }
            rb.velocity = v;
        } else if (Time.fixedTime > t - 0.3f)
        {
            base.SetBool("jumping", true);
        }

        if (Math.Abs(rb.velocity.x - 0) < epsilon)
        {
            base.SetFloat("xVelocity", 0);
        }
        else
        {
            base.SetFloat("xVelocity", 1);
        }
        base.SetFloat("yVelocity", Math.Abs(rb.velocity.y));
        if (rb.velocity.x > epsilon)
        {
            base.SetFlipX(false);
        }
        else if (rb.velocity.x < -epsilon)
        {
            base.SetFlipX(true);
        }
    }
}
