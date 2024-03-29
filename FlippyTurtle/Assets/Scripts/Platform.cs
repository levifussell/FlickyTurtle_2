﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Platform : MonoBehaviour {

    private float maxDist;

    static int minSize = 3;
    static int maxSize = 7;
    private int size;
    //public GameObject spawnPoint;
    public PlatformPart block;
    public PlatformPart frontBlock;
    public PlatformPart backBlock;

    public PlatformPart blockBouncey;
    public PlatformPart frontBlockBouncey;
    public PlatformPart backBlockBouncey;

    private List<PlatformPart> blocks;
    private List<Vector3> offset;

    public GameObject coin;
    private GameObject coinAttach;
    private Vector3 coinOffset;

    private float startBobPos;
    private float bobRate;
    private float bobChange;
    private bool collided;
    private float dampenHit;

    public static string NAME_EXTEND_BOUNCEY = "_bouncey";

	// Use this for initialization
	void Start () {

        this.collided = false;

        if (SpawnBelt.bounceyCount > 0 && SpawnBelt.bounceyCount < SpawnBelt.maxBounceSequence)
        {
            this.size = SpawnBelt.bounceySize;
        }
        else
        {
            //get the type of this platform correlating with the turtle type
            SpawnCube.CubeTypes nextType = SpawnCube.getNextPlatformLineup().Dequeue();

            if (nextType == SpawnCube.CubeTypes.Normal)
                this.size = 3; //UnityEngine.Random.Range(minSize, maxSize);
            else if (nextType == SpawnCube.CubeTypes.Big)
                this.size = 4;
        }

        this.dampenHit = 0.5f / this.size;
        this.blocks = new List<PlatformPart>();
        this.offset = new List<Vector3>();
        //GetComponent<Collider>().bounds.size.x
        //this.bitify();

        //determine platform type
        string name_extend = "";
        PlatformPart frontPart = frontBlock;
        PlatformPart backPart = backBlock;
        PlatformPart middlePart = block;


        //int randPlatType = UnityEngine.Random.Range(0, 4);
        if (SpawnBelt.bounceyCount > 0)
        {
            SpawnBelt.bounceySize = this.size;
            SpawnBelt.bounceyCount--;

            //if (SpawnBelt.bounceyCount == -1)
            //    SpawnBelt.bounceyCount = 3;

            Debug.Log("BOUNCEY COUNT: " + SpawnBelt.bounceyCount);

            //the final platform is not bouncey
            if(SpawnBelt.bounceyCount > 0)
            {
                name_extend = NAME_EXTEND_BOUNCEY;
                frontPart = frontBlockBouncey;
                backPart = backBlockBouncey;
                middlePart = blockBouncey;
            }
        }

        Vector3 posOffset = new Vector3(Blockify.spacing + 0.1f, 0.0f, 0.0f);
        for(int i = 0; i < this.size; ++i)
        {
            Vector3 offsetTemp = Vector3.zero;
            PlatformPart blockTemp;
            if(i == 0)
            {
                blockTemp = (Instantiate(backPart, this.transform.position + posOffset * (i + 1), this.transform.rotation) as PlatformPart);
                offsetTemp = posOffset * (i + 1) - new Vector3(0.0f, 0.0f, 0.0f);
            }
            else if(i == this.size - 1)
            {
                blockTemp = (Instantiate(frontPart, this.transform.position + posOffset * (i - 1), this.transform.rotation) as PlatformPart);
                offsetTemp = posOffset * (i - 1) + new Vector3(0.0f, 0.0f, 0.0f);
            }
            else
            {
                blockTemp = (Instantiate(middlePart, this.transform.position + posOffset * i, this.transform.rotation) as PlatformPart);
                offsetTemp = posOffset * i;

            }

            blockTemp.setParentPos(this.transform.position);
            if (i == 0)
            {
                blockTemp.name = "first_block" + name_extend;
                //offsetTemp.z += 1.0f;
            }
            else
            {
                blockTemp.name = "middle_block" + name_extend;
            }
            //float randScale = Random.Range(blockTemp.transform.localScale.z * 3, blockTemp.transform.localScale.z * 7);
            //blockTemp.transform.localScale = new Vector3(blockTemp.transform.localScale.x, blockTemp.transform.localScale.y, randScale);
            this.blocks.Add(blockTemp);
            this.offset.Add(offsetTemp);
        }

        startBobPos = this.transform.position.y;
        bobRate = 0.0f + (float)Math.Sin(UnityEngine.Random.Range(0.1f, 4.0f)) / 100.0f;
        bobChange = 0.08f;

        this.maxDist = this.transform.position.x - 50.0f;

        if(this.size % 2 == 0)
            coinOffset = new Vector3((size + -3f) * Blockify.spacing, 2f, 0f);
        else
            coinOffset = new Vector3((size + -2.5f) * Blockify.spacing, 2f, 0f);

        if(UnityEngine.Random.Range(0, 5) == 1)
            createCoin();
    }

    private void OnDestroy()
    {
        for(int i = 0; i < this.blocks.Count; ++i)
        {
            if(this.blocks[i] != null)
                Destroy(this.blocks[i].gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //foreach (ContactPoint contact in collision.contacts)
        //{
        //    Debug.DrawRay(contact.point, contact.normal, Color.white);
        //}
        //Debug.Log("COLLISION!");
    }

    // Update is called once per frame
    void Update () {

        Vector3 speed = new Vector3(0.0f, 0.0f, SpawnBelt.PLATFORM_SPEED);

        //Debug.Log(this.transform.position.x);
        //Vector3 posBit = this.transform.position + this.offset[i];
        
        this.transform.Translate(speed);

        //make the platform bob
        Vector3 bobPos = this.transform.position;
        bobPos.y = (float)(this.startBobPos + Math.Sin(this.bobRate) / 10.0f);
        this.bobRate += bobChange;
        this.transform.position = bobPos;

        //Vector3 bittifiedPos = this.transform.position;
        //bittifiedPos.z -= bittifiedPos.z % FlipScript.spacing;
        //this.transform.SetPositionAndRotation(bittifiedPos, this.transform.rotation);
        //this.bitify();
        //spawnPoint.transform.Translate(speed);

        for (int i = 0; i < this.blocks.Count; ++i)
        {
            //this.blocks[i].GetComponent<Renderer>().material.color = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
            //this.blocks[i].GetComponent<Renderer>().material.SetColor("_Color", Color.red);
            Vector3 posBit = this.transform.position + this.offset[i];
            //posBit.x -= posBit.x % FlipScript.spacing;
            //posBit.y -= posBit.y % FlipScript.spacing;
            //posBit.z -= posBit.z % FlipScript.spacing;
            this.blocks[i].transform.SetPositionAndRotation(posBit, this.blocks[i].transform.rotation);
            this.blocks[i].setParentPos(this.transform.position);

            if(this.blocks[i].collided && !this.collided)
            {
                this.collided = true;
            }
                
        }

        if(this.collided)
        {
            //this.transform.position = this.transform.position + new Vector3(-this.dampenHit, 0, 0);
            this.startBobPos -= this.dampenHit;
            this.dampenHit -= 0.008f;
        }

        if (PlayButton.GAME_MODE == PlayButton.GameMode.GAME_OVER)
        {
            this.startBobPos -= this.dampenHit;
            this.dampenHit += 0.01f * this.transform.position.x;
        }
        else if (PlayButton.GAME_MODE == PlayButton.GameMode.PAUSE_START)
            Destroy(this.gameObject);

        if (this.transform.position.x < -15.0f ||
            this.transform.position.y < -20.0f)
        {
            //if the platform leaves the screen and it
            // hasn't been collided with, the player loses
            if (!this.collided)
            {
                Debug.Log("PLATFORM GAMEOVER!!");
                PlayButton.GameOverCall();
            }

            Destroy(this.gameObject);
        }

        //coin update
        if(this.coinAttach != null)
        {
            this.coinAttach.transform.position = this.transform.position + coinOffset;
            this.coinAttach.transform.Rotate(new Vector3(-1.0f, 0, 0));
        }

	}

    void bitify()
    {
        Vector3 bittifiedPos = this.transform.position;
        bittifiedPos.z -= bittifiedPos.z % Blockify.spacing;
        //bittifiedPos.z = Blockify.Apply(bittifiedPos.z);
        this.transform.SetPositionAndRotation(bittifiedPos, this.transform.rotation);
    }

    void createCoin()
    {
        this.coinAttach = (Instantiate(coin, this.transform.position + coinOffset, this.transform.rotation) as GameObject);
        this.coinAttach.transform.Rotate(new Vector3(0, 0, 90));
    }
}
