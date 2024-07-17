using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderHurt : MonoBehaviour
{

    GameObject player;
    SpriteRenderer spriteRenderer;
    SpriteRenderer playerRenderer;

    // Start is called before the first frame update
    void Start()
    {
        player = gameObject.transform.parent.gameObject.transform.parent.gameObject;
        playerRenderer = player.GetComponent<SpriteRenderer>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        spriteRenderer.color = playerRenderer.color;
    }
}
