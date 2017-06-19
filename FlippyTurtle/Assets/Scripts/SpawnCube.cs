using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpawnCube : MonoBehaviour {

    public enum CubeTypes
    {
        Normal,
        Big
    };

    public FlipScript cubeNormal;
    public FlipScript cubeBig;
    public Vector3 spawnPoint;
    public int maxQueue;
    //private FlipScript currentCube;
    //private FlipScript nextCube;
    private List<FlipScript> nextCubes;
    private Queue<CubeTypes> next_lineup;
    private static Queue<CubeTypes> next_platform_lineup;
    private static int lineup_size = 10;

	// Use this for initialization
	void Start () {

        this.restart();
	}

    void restart()
    {
        this.nextCubes = new List<FlipScript>();
        //this.currentCube = null;
        //this.nextCube = null;

        //set up the initial lineup
        next_lineup = new Queue<CubeTypes>();
        for (int i = 0; i < lineup_size; ++i)
            next_lineup.Enqueue(this.randomNextCube());

        //line up for turtles that have spawned and their required platforms
        next_platform_lineup = new Queue<CubeTypes>();
    }
	
	// Update is called once per frame
	void Update () {

        //if (PlayButton.GAME_OVER)
        //    return;

        //if(this.currentCube == null || this.currentCube.getCollided() == true)
        if(nextCubes.Count < maxQueue && PlayButton.GAME_MODE != PlayButton.GameMode.GAME_OVER)
        {
            this.CreateCube();
        }

        if (nextCubes.Count == 0 && PlayButton.GAME_MODE == PlayButton.GameMode.GAME_OVER)
            this.restart();

        for(int i = 0; i < nextCubes.Count; ++i)
        {
            if (nextCubes[i] == null || nextCubes[i].getFlipMode())
            {
                nextCubes.RemoveAt(i);
                break;
            }
            else if (nextCubes[i].waitingToJump && i == 0)
                nextCubes[i].waitingToJump = false;

            if(nextCubes[i] != null && !nextCubes[i].getFlipMode())
            {
                Vector3 offset = this.spawnPointFromOrder(i);
                Vector3 target = this.spawnPoint + offset;
                Vector3 diffToTarget = target - nextCubes[i].transform.position;
                Vector3 dampenMove = diffToTarget / 4.0f;
                dampenMove.y = -0.1f;
                //nextCubes[i].transform.position = target; //+= dampenMove;
                nextCubes[i].gameObject.GetComponent<CharacterController>().Move(dampenMove);
            }
        }

	}

    private void CreateCube()
    {
        //remove an item from the queue and enqueue a new item
        CubeTypes nextType = next_lineup.Dequeue();
        next_platform_lineup.Enqueue(nextType);
        next_lineup.Enqueue(randomNextCube());

        int position = this.nextCubes.Count + 1;
        Vector3 offset = this.spawnPointFromOrder(position);
        FlipScript platformTemp;

        //create random turtle types
        if(nextType == CubeTypes.Normal)
            platformTemp = Instantiate(cubeNormal, this.spawnPoint + offset, this.transform.rotation);
        else if(nextType == CubeTypes.Big)
            platformTemp = Instantiate(cubeBig, this.spawnPoint + offset, this.transform.rotation);
        else
            platformTemp = Instantiate(cubeNormal, this.spawnPoint + offset, this.transform.rotation);

        platformTemp.setCubeType(nextType);

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

    private CubeTypes randomNextCube()
    {
        int randVal = UnityEngine.Random.Range(0, 2);
        if (randVal < 1)
            return CubeTypes.Normal;
        else if (randVal <= 2)
            return CubeTypes.Big;
        else
            return CubeTypes.Normal;
    }

    public static Queue<CubeTypes> getNextPlatformLineup()
    {
        return next_platform_lineup;
    }
}
