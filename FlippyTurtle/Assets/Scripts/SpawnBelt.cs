using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpawnBelt : MonoBehaviour {

    private float time;
    public GameObject platform;
    public static float PLATFORM_SPEED = -0.08f;
    public static float PLATFORM_SPEED_DECREASE = -0.001f;
    private float minSpawnTime;
    private float maxSpawnTime;
    //private List<Platform> platforms;

    public static int bounceyCount;
    public static int bounceySize;
    public static int maxBounceSequence = 3;

    // Use this for initialization
    void Start () {
        this.restart();
    }

    void restart()
    {
        this.time = Time.time + 1;

        this.minSpawnTime = 1.0f;
        this.maxSpawnTime = 1.8f;
        bounceyCount = 0;
    }
	
	// Update is called once per frame
	void Update () {

        if (PlayButton.GAME_MODE == PlayButton.GameMode.GAME_OVER)
            this.restart();

        if (this.time < Time.time && (SpawnCube.getNextPlatformLineup().Count > 0 || bounceyCount > 0) && 
            PlayButton.GAME_MODE != PlayButton.GameMode.GAME_OVER)
        {
            //random spawn time
            float randSpawn = UnityEngine.Random.Range(this.minSpawnTime, this.maxSpawnTime);

            //maybe create a random bouncey platform
            int randPlatType = UnityEngine.Random.Range(0, 2);
            if (randPlatType < 1 && bounceyCount == 0)
                bounceyCount = maxBounceSequence;

            //if we are in bouncey block mode, add consecutive platforms at even time-points
            if (bounceyCount > 0)
                randSpawn = -0.08f/PLATFORM_SPEED;

            this.time = Time.time + randSpawn;

            CreatePlatform();

           // PLATFORM_SPEED += PLATFORM_SPEED_DECREASE;
           // this.minSpawnTime += PLATFORM_SPEED * 0.0001f;
           // this.maxSpawnTime += PLATFORM_SPEED * 0.01f;
        }
        //Debug.Log("TIME CREATED: " + Time.time);
    }

    void CreatePlatform()
    {
        GameObject platformTemp = (Instantiate(platform, this.transform.position, this.transform.rotation) as GameObject);
        //Debug.Log("NEW OBJECT CREATED: " + this.transform.position);
    }
}
