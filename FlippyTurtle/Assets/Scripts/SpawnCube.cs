using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCube : MonoBehaviour {

    public FlipScript cube;
    public Vector3 spawnPoint;
    public int maxQueue;
    //private FlipScript currentCube;
    //private FlipScript nextCube;
    private List<FlipScript> nextCubes;

	// Use this for initialization
	void Start () {
        this.nextCubes = new List<FlipScript>();
        //this.currentCube = null;
        //this.nextCube = null;
	}
	
	// Update is called once per frame
	void Update () {
		
        //if(this.currentCube == null || this.currentCube.getCollided() == true)
        if(nextCubes.Count < maxQueue)
        {
            this.CreateCube();
        }

        for(int i = 0; i < nextCubes.Count; ++i)
        {
            if (nextCubes[i] == null || nextCubes[i].getCollided())
                nextCubes.RemoveAt(i);
        }

	}

    private void CreateCube()
    {
        int position = nextCubes.Count;
        Vector3 offset = new Vector3(2.0f * position, 0, 0);
        FlipScript platformTemp = Instantiate(cube, this.spawnPoint + offset, this.transform.rotation);
        //this.currentCube = platformTemp;
        this.nextCubes.Add(platformTemp);
        //Debug.Log("NEW CUBE CREATED: " + this.transform.position);
    }
}
