using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Worldbuilder : MonoBehaviour
{
    private float spawnTime = 0f, spawnLength, spawnInterval;
    private Tile tile;
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
        tile = Resources.Load<Tile>("Temp_tile");
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
        if (spawnTime >= spawnInterval)
        {
            if (UnityEngine.Random.Range(0f, 2f) >= 1)
            {
                spawnSheep(40);
            } else
            {
                spawnSheep(-40);
            }
            spawnTime -= spawnInterval;
            spawnInterval = Math.Max(1.0f, spawnLength + UnityEngine.Random.Range(-6f, 6f));
        }
        if (Time.fixedTime / timeUnit - (Time.fixedTime - Time.deltaTime) / timeUnit == 1)
        {
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
        for (int i = -50; i <= 50; i++)
        {
            Vector3Int v = new Vector3Int(i, -6, 0);
            terrain.SetTile(v, tile);
        }
    }

    public void spawnSheep(float x)
    {
        float r = UnityEngine.Random.Range(0f, difficulty) % 5.0f;
        if (r <= 1.0f)
        {
            sheep.GetComponent<EnemyMove>().speed = UnityEngine.Random.Range(2, 4);
            Instantiate(sheep, new Vector3(x, -4.5f, 0), Quaternion.identity);
        } else if (r <= 2.0f)
        {
            flyingSheep.GetComponent<EnemyMove>().speed = UnityEngine.Random.Range(3, 5);
            Instantiate(flyingSheep, new Vector3(x, 0, 0), Quaternion.identity);
        } else if (r <= 3.0f)
        {
            Instantiate(bigSheep, new Vector3(x, -4.0f, 0), Quaternion.identity);
        } else if (r <= 4.0f) {
            Instantiate(astralSheep, new Vector3(x, 0, 0), Quaternion.identity);
        } else
        {
            sheep.GetComponent<EnemyMove>().speed = UnityEngine.Random.Range(2, 4);
            Instantiate(sheep, new Vector3(x, -4.5f, 0), Quaternion.identity);
            Instantiate(sheep, new Vector3(-x, -4.5f, 0), Quaternion.identity);
        }
    }
}
