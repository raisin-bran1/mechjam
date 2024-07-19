using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beam : MonoBehaviour
{
    public float damage = 20;
    private Camera cam;
    private Collider2D[] collisions = new Collider2D[20];
    public float rotationSpeed;

    GameObject player;
    Combat combat;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        UpdateAngleInstant();
        
        player = GameObject.Find("Player");
        combat = player.GetComponent<Combat>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePosition();
        UpdateAngle();
        UpdateDamage();
    }

    void UpdatePosition()
    {
        transform.position = player.transform.position;
        Vector3 pos = player.transform.position;
        pos.y -= 0.5f;
        if (combat.GetFlip() == 1)
        {
            pos.x += 5.4f;
        }
        else
        {
            pos.x -= 5.4f;
        }
        transform.position = pos;
    }

    void UpdateAngleInstant()
    {
        Vector3 point = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = new Vector2(point.x - transform.position.x, point.y - transform.position.y);
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(Vector3.forward * angle);
    }

    void UpdateAngle()
    {
        Vector3 point = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = new Vector2(point.x - transform.position.x, point.y - transform.position.y);
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        float angle2 = transform.rotation.eulerAngles.z;
        angle = angle % 360;
        angle2 = angle2 % 360;
        if (angle < 0)
        {
            angle += 360;
        }
        if (angle < angle2)
        {
            angle += 360;
        }
        if (angle - angle2 >= 180)
        {
            angle -= 360;
            angle = Math.Max(angle, angle2 - rotationSpeed * Time.deltaTime);
        } else
        {
            angle = Math.Min(angle, angle2 + rotationSpeed * Time.deltaTime);
        }
        angle = angle % 360;
        if (combat.GetFlip() == 1)
        {
            if (angle <= 180)
            {
                angle += 360;
            }
            if (angle < 300)
            {
                angle = 300;
            }
            if (angle > 420)
            {
                angle = 420;
            }
        } else
        {
            if (angle < 120)
            {
                angle = 120;
            }
            if (angle > 240)
            {
                angle = 240;
            }
        }
        transform.rotation = Quaternion.Euler(Vector3.forward * angle);
    }

    void UpdateDamage()
    {
        int cols = gameObject.GetComponent<Collider2D>().OverlapCollider(new ContactFilter2D().NoFilter(), collisions);
        for (int i = 0; i < cols; i++)
        {
            Collider2D collision = collisions[i];
            if (collision.gameObject.tag == "Enemy")
            {
                combat.AddEnergy(collision.gameObject.GetComponent<EnemyCombat>().Damage(3 * damage * Time.deltaTime, 2));
            }
        }
    }

}
