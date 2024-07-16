using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Worldbuilder : MonoBehaviour
{
    private float spawn_time = 0f, spawn_length = 3, spawn_interval;
    private Tile tile;
    private Tilemap terrain;
    public GameObject sheep;

    // Start is called before the first frame update
    void Start()
    {
        tile = Resources.Load<Tile>("Temp_tile");
        terrain = GameObject.Find("Terrain").GetComponent<Tilemap>();
        placeGround();
        spawnSheep(20);
        spawn_interval = spawn_length + Random.Range(-1f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        spawn_time += Time.deltaTime;
        if (spawn_time >= spawn_interval)
        {
            if (Random.Range(0f, 2f) >= 1)
            {
                spawnSheep(20);
            } else
            {
                spawnSheep(-20);
            }
            spawn_time -= spawn_interval;
            spawn_interval = spawn_length + Random.Range(-1f, 1f);
        }
    }
    public void placeGround()
    {
        for (int i = -25; i <= 25; i++)
        {
            Vector3Int v = new Vector3Int(i, -6, 0);
            terrain.SetTile(v, tile);
        }
    }

    public void spawnSheep(float x)
    {
        Instantiate(sheep, new Vector3(x, -4.5f, 0), Quaternion.identity);
    }
}
