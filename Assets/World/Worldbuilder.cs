using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Worldbuilder : MonoBehaviour
{
    private float spawnTime = 0f, spawnLength, spawnInterval;
    private float updateTime = 0f;
    private Tile[] tiles;
    private Tilemap terrain;
    private float difficulty;
    //rate in enemies per t, acceleration in enemies per t per t, chance to accelerate rate rather than increment difficulty; difficulty starts at .5 and each next stronger sheep starts appearing at each integer
    public float startRate, rateAcc, rateChance, diffIncrement;
    public float timeUnit;
    public GameObject player;
    public GameObject sheep;
    public GameObject bigSheep;
    public GameObject flyingSheep;
    public GameObject astralSheep;

    // Start is called before the first frame update
    void Start()
    {
        tiles = Resources.LoadAll<Tile>("Tiles");
        terrain = GameObject.Find("Terrain").GetComponent<Tilemap>();
        placeGround();
        spawnSheep(40);
        spawnLength = timeUnit / startRate;
        difficulty = 0.5f;
        spawnInterval = Math.Max(1.0f, spawnLength + UnityEngine.Random.Range(-6f, 6f));
    }

    // Update is called once per frame
    void Update()
    {
        spawnTime += Time.deltaTime;
        updateTime += Time.deltaTime;
        if (spawnTime >= spawnInterval)
        {
            if (UnityEngine.Random.Range(0f, 2f) >= 1)
            {
                spawnSheep(Math.Min(player.transform.position.x + 50, 95));
            } else
            {
                spawnSheep(Math.Max(player.transform.position.x - 50, -95));
            }
            spawnTime -= spawnInterval;
            spawnInterval = Math.Max(1.0f, spawnLength + UnityEngine.Random.Range(-6f, 6f));
        }
        if (updateTime >= timeUnit)
        {
            updateTime -= timeUnit;
            if (UnityEngine.Random.Range(0.0f, 1.0f) < rateChance)
            {
                startRate += rateAcc;
                spawnLength = timeUnit / startRate;
            } else
            {
                difficulty += diffIncrement;
            }
        }
    }
    public void placeGround()
    {
        for (int i = -130; i <= 130; i++)
        {
            Vector3Int v = new Vector3Int(i, -6, 0);
            terrain.SetTile(v, tiles[1]);
            for (int j = -7; j >= -12; j--)
            {
                Vector3Int w = new Vector3Int(i, j, 0);
                terrain.SetTile(w, tiles[4]);
            }
        }
    }

    public void spawnSheep(float x)
    {
        float y = difficulty;
        while (y >= 0)
        {
            float r = UnityEngine.Random.Range(0f, y) % 4.0f;
            if (r <= 1.0f)
            {
                sheep.GetComponent<EnemyMove>().speed = UnityEngine.Random.Range(2, 4);
                Instantiate(sheep, new Vector3(x, -4.5f, 0), Quaternion.identity);
                y -= 1.0f;
            }
            else if (r <= 2.0f)
            {
                flyingSheep.GetComponent<EnemyMove>().speed = UnityEngine.Random.Range(3, 5);
                flyingSheep.GetComponent<FlyingSheepMovement>().height = UnityEngine.Random.Range(3, 7);
                Instantiate(flyingSheep, new Vector3(x, 0, 0), Quaternion.identity);
                y -= 2.0f;
            }
            else if (r <= 3.0f)
            {
                Instantiate(bigSheep, new Vector3(x, -4.0f, 0), Quaternion.identity);
                y -= 3.0f;
            }
            else if (r <= 4.0f)
            {
                Instantiate(astralSheep, new Vector3(x, 0, 0), Quaternion.identity);
                y -= 4.0f;
            }
        }
    }
}
