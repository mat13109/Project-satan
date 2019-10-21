﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombSpawn : MonoBehaviour
{

    public GameObject bombSpawn;
    public GameObject bombType;

    private float period = 0.0f;
    public int spawnTime = 1;


    // Start is called before the first frame update
    void Start()
    {


    }

    // Update is called once per frame


    void Update()
    {
        //shoot every spawnTime;
        if (period > spawnTime)
        {
            Shoot();
            period = 0;
        }
        period += UnityEngine.Time.deltaTime;
    }

    void Shoot()
    {
        //instantiate bomb
        Instantiate(bombType, bombSpawn.transform.position, transform.rotation);

        //destroy after n sec
        //Destroy(bombType, 2.0f);
    }
}
