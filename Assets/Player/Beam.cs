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

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        UpdateAngleInstant();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateAngle();
        UpdateDamage();
        transform.position = GameObject.FindWithTag("Player").transform.position;
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
        float angle2 = transform.eulerAngles[2];
        if (Math.Abs(angle - angle2) % 360 < 360 - Math.Abs(angle - angle2) % 360)
        {
            angle = Math.Max(angle, (angle2 - Time.deltaTime * rotationSpeed));
        } else
        {
            angle = Math.Min(angle, (angle2 + Time.deltaTime * rotationSpeed));
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
                collision.gameObject.GetComponent<EnemyCombat>().Damage(damage * Time.deltaTime);
            }
        }
    }

}
