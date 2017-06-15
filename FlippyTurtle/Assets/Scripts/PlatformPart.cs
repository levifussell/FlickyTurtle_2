using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlatformPart : MonoBehaviour {

    public static float speedMove = -0.08f;
    private Vector3 parentPos;
    public bool collided;

	// Use this for initialization
	void Start () {
        this.collided = false;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        this.collided = true;
        this.GetComponent<Renderer>().material.color = Color.red;
        //this.blocks[i].GetComponent<Renderer>().material.SetColor("_Color", Color.red);
        //Debug.Log("COLLISION with " + other.name);

        //float awesomeShotThreshold = 1.0f;

        //if (Math.Abs(other.transform.position.x - this.parentPos.x) < awesomeShotThreshold)
        //{
        //    Debug.Log("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
        //}
    }

    //public float getSpeedMove() { return this.speedMove; }
    public void setParentPos(Vector3 parentPos) { this.parentPos = parentPos;  }
}
