using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpawnBelt : MonoBehaviour {

    private float time;
    public GameObject platform;
    public static float PLATFORM_SPEED = -0.08f;
    public static float PLATFORM_SPEED_DECREASE = -0.001f;
    //private List<Platform> platforms;

    public static int bounceyCount;
    public static int bounceySize;

    // Use this for initialization
    void Start () {
        this.time = Time.time;

        bounceyCount = 0;

    }
	
	// Update is called once per frame
	void Update () {
		
        if(this.time < Time.time && SpawnCube.getNextPlatformLineup().Count > 0)
        {
            //random spawn time
            float randSpawn = UnityEngine.Random.Range(1.0f, 1.8f);

            //if we are in bouncey block mode, add consecutive platforms at even time-points
            if (bounceyCount > 0)
                randSpawn = 1.0f;

            this.time = Time.time + randSpawn;

            CreatePlatform();

            PLATFORM_SPEED += PLATFORM_SPEED_DECREASE;
        }
        //Debug.Log("TIME CREATED: " + Time.time);
    }

    void CreatePlatform()
    {
        GameObject platformTemp = (Instantiate(platform, this.transform.position, this.transform.rotation) as GameObject);
        //Debug.Log("NEW OBJECT CREATED: " + this.transform.position);
    }
}
