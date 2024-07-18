using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Screenshake : MonoBehaviour
{
    // Start is called before the first frame update
    private Camera cam;
    public float shake = 0;
    public float shakeAmount;
    public float decreaseFactor;
 
    void Start()
    {
        cam = GetComponent<Camera>();    
    }
    void Update()
    {
        //cam.transform.localPosition = new Vector3(cam.transform.position.x, cam.transform.position.y, 0);
        if (shake > 0)
        {
            Vector2 shakeposition = Random.insideUnitCircle;
            shakeposition *= shake;
            cam.transform.localPosition = new Vector3(shakeposition.x, shakeposition.y, -10);
            shake -= Time.deltaTime * decreaseFactor;
        }
        else
        {
            shake = 0;
            cam.transform.localPosition = new Vector3(0, 0, -10);
        }
    }
}
