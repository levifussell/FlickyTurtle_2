using System.Collections;
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

	// Use this for initialization
	void Start () {

        this.collided = false;
        this.size = UnityEngine.Random.Range(minSize, maxSize);
        this.dampenHit = 0.5f / this.size;
        this.blocks = new List<PlatformPart>();
        this.offset = new List<Vector3>();
        //GetComponent<Collider>().bounds.size.x
        //this.bitify();
        Vector3 posOffset = new Vector3(Blockify.spacing + 0.1f, 0.0f, 0.0f);
        for(int i = 0; i < this.size; ++i)
        {
            Vector3 offsetTemp = Vector3.zero;
            PlatformPart blockTemp = (Instantiate(block, this.transform.position + posOffset * i, this.transform.rotation) as PlatformPart);
            //blockTemp.GetComponent<Renderer>().material.color = Color.black;
            blockTemp.setParentPos(this.transform.position);
            if (i == 0)
            {
                blockTemp.name = "first_block";
                //offsetTemp.z += 1.0f;
            }

            //float randScale = Random.Range(blockTemp.transform.localScale.z * 3, blockTemp.transform.localScale.z * 7);
            //blockTemp.transform.localScale = new Vector3(blockTemp.transform.localScale.x, blockTemp.transform.localScale.y, randScale);
            this.blocks.Add(blockTemp);
            this.offset.Add(posOffset * i + offsetTemp);
        }

        startBobPos = this.transform.position.y;
        bobRate = 0.0f + (float)Math.Sin(UnityEngine.Random.Range(0.1f, 4.0f)) / 100.0f;
        bobChange = 0.08f;

        this.maxDist = this.transform.position.x - 50.0f;

        if(this.size % 2 == 0)
            coinOffset = new Vector3((size + 0.5f) * Blockify.spacing, 0.5f, -4.5f);
        else
            coinOffset = new Vector3((size + 1) * Blockify.spacing, 0.5f, -4.5f);

        //if(UnityEngine.Random.Range(0, 5) == 1)
          //  createCoin();
    }

    private void OnDestroy()
    {
        for(int i = 0; i < this.blocks.Count; ++i)
        {
            Destroy(this.blocks[i]);
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

        Vector3 speed = new Vector3(0.0f, 0.0f, PlatformPart.speedMove);

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

        if (this.transform.position.x < this.maxDist)
        {
            //Debug.Log("PLATFORM GONEWRWRWQARQWRQ");
            Destroy(this);
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
