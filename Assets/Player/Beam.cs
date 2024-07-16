using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beam : MonoBehaviour
{
    private Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        updateAngle();
    }

    // Update is called once per frame
    void Update()
    {
        updateAngle();
        transform.position = GameObject.FindWithTag("Player").transform.position;
    }

    void updateAngle()
    {
        Vector3 point = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = new Vector2(point.x - transform.position.x, point.y - transform.position.y);
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * 180 / Mathf.PI;
        transform.rotation = Quaternion.Euler(Vector3.forward * angle);
    }
}
