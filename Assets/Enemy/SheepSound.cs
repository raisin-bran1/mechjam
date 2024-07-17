using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SheepSound : MonoBehaviour
{
    private float cooldown = 0;
    [SerializeField] private AudioClip baa;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        cooldown += Time.deltaTime;
        if (cooldown >= 3)
        {
            if (Random.Range(0, 10000) == 666)
            {
                AudioSource.PlayClipAtPoint(baa, transform.position, 1f);
                cooldown = 0;
            }
        }
    }
}
