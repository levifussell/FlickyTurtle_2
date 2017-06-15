using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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
            {
                nextCubes.RemoveAt(i);
                //break;
            }
            else if (nextCubes[i].waitingToJump && i == 0)
                nextCubes[i].waitingToJump = false;

            if(nextCubes[i] != null && !nextCubes[i].getFlipMode())
            {
                Vector3 offset = this.spawnPointFromOrder(i);
                Vector3 target = this.spawnPoint + offset;
                Vector3 diffToTarget = target - nextCubes[i].transform.position;
                Vector3 dampenMove = diffToTarget / 20.0f;
                dampenMove.y = -0.1f;
                //nextCubes[i].transform.position = target; //+= dampenMove;
                nextCubes[i].gameObject.GetComponent<CharacterController>().Move(dampenMove);
            }
        }

	}

    private void CreateCube()
    {
        int position = this.nextCubes.Count + 1;
        Vector3 offset = this.spawnPointFromOrder(position);
        FlipScript platformTemp = Instantiate(cube, this.spawnPoint + offset, this.transform.rotation);
        platformTemp.transform.Rotate(new Vector3(0, 180, 0));
        if (this.nextCubes.Count == 0)
            platformTemp.waitingToJump = false;
        else
            platformTemp.waitingToJump = true;
        //this.currentCube = platformTemp;
        this.nextCubes.Add(platformTemp);
        //Debug.Log("NEW CUBE CREATED: " + this.transform.position);
    }

    private Vector3 spawnPointFromOrder(int order)
    {
        return new Vector3(order * 2, 0.0f, (float)Math.Cos(order) * 2 - 2);
    }
}
