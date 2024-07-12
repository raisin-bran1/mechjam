using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Worldbuilder : MonoBehaviour
{
    private Tile tile;
    private Tilemap terrain;

    // Start is called before the first frame update
    void Start()
    {
        tile = Resources.Load<Tile>("Temp_tile");
        terrain = GameObject.Find("Terrain").GetComponent<Tilemap>();
        placeGround();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void placeGround()
    {
        for (int i = -10; i <= 10; i++)
        {
            Vector3Int v = new Vector3Int(i, -3, 0);
            terrain.SetTile(v, tile);
        }
    }
}
