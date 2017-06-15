using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpawnBelt : MonoBehaviour {

    private float time;
    public GameObject platform;
    //private List<Platform> platforms;

	// Use this for initialization
	void Start () {
        this.time = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		
        if(this.time < Time.time)
        {
            //random spawn time
            float randSpawn = UnityEngine.Random.Range(1.0f, 2.0f);
            this.time = Time.time + randSpawn;

           CreatePlatform();
        }
        //Debug.Log("TIME CREATED: " + Time.time);
    }

    void CreatePlatform()
    {
        GameObject platformTemp = (Instantiate(platform, this.transform.position, this.transform.rotation) as GameObject);
        //Debug.Log("NEW OBJECT CREATED: " + this.transform.position);
    }
}
