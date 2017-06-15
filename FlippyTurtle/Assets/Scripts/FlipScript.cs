﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FlipScript : MonoBehaviour {

    public Explosion explosionGroundHit;
    private Explosion tempExplosion;

    private float jumpforce = 65f;
    private float gravity = 250;
    private Vector3 moves = Vector3.zero;
    private Vector3 startingPos;
    private bool collided;
    private Collider platformCollision;
    private Vector3 collisionOffset;
    private float maxDist;
    private bool awesomeCollisionDetection; //detect for a perfect collision only once
    //private Animator CubeAnimation;
    private bool flipMode;
    private Quaternion startRotation;

    // Use this for initialization
    void Start () {
        //CubeAnimation = GetComponent<Animator>();
        this.startingPos = this.transform.position;
        this.collided = false;
        this.awesomeCollisionDetection = false;
        this.maxDist = this.transform.position.x - 20.0f;
        this.flipMode = false;
        this.startRotation = this.transform.rotation;
    }

    private void OnTriggerEnter(Collider other)
    {
        //this.collided = true;
        //this.platformCollision = other;
        //this.collisionOffset = this.transform.position - this.platformCollision.transform.position;

        //if (other.name.CompareTo("first_block") == 0)
        //{
        //    float distX = Math.Abs((this.transform.position.x + this.GetComponent<BoxCollider>().size.x / 2) - other.transform.position.x);

        //    if (distX < 0.5f)
        //    {
        //        //this.collisionOffset.y = other.GetComponent<BoxCollider>().size.y / 2; //+ this.transform.GetComponent<BoxCollider>().size.y;
        //        this.collisionOffset.x = -this.GetComponent<BoxCollider>().size.x / 2;
        //        Debug.Log("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
        //    }
        //}

        //if(other.name.CompareTo("Coin 1(Clone)") == 0)
        //{
        //    Debug.Log("COIN!");
        //    GameObject.Destroy(other);
        //}

        //Debug.Log("NAME---- " + other.name);
        if (other.name.CompareTo("PlatformPiece 1(Clone)") == 0 || other.name.CompareTo("first_block") == 0)
        {
            if(!this.collided)
            {
                //first collision, create some ground dust
                this.tempExplosion = (Instantiate(explosionGroundHit, this.transform.position, this.transform.rotation) as Explosion);

                this.transform.rotation = this.startRotation;
                //this.collided = true;
                this.collided = true;
                this.platformCollision = other;
                this.collisionOffset = this.transform.position - this.platformCollision.transform.position;
                this.collisionOffset.y = other.GetComponent<BoxCollider>().size.y / 2;

            }

            if (other.name.CompareTo("first_block") == 0 && !this.awesomeCollisionDetection)
            {
                float distX = Math.Abs((this.transform.position.x + this.GetComponent<BoxCollider>().size.x / 2) - other.transform.position.x);

                if (distX < 0.5f)
                {
                    this.platformCollision = other;
                    this.collisionOffset = this.transform.position - this.platformCollision.transform.position;
                    //this.collisionOffset.y = other.GetComponent<BoxCollider>().size.y / 2; //+ this.transform.GetComponent<BoxCollider>().size.y;
                    this.collisionOffset.x = -this.GetComponent<BoxCollider>().size.x / 2;
                    Debug.Log("--------------------!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                }

                this.awesomeCollisionDetection = true;
            }
            ////this.collided = true;
            //this.collided = true;
            //this.platformCollision = other;
            //this.collisionOffset = this.transform.position - this.platformCollision.transform.position;
            //this.collisionOffset.y = other.GetComponent<BoxCollider>().size.y / 2;

            //float distX = Math.Abs((this.transform.position.x + this.GetComponent<BoxCollider>().size.x / 2) - other.transform.position.x);

            //if (distX < 0.5f)
            //{
            //    //this.collisionOffset.y = other.GetComponent<BoxCollider>().size.y / 2; //+ this.transform.GetComponent<BoxCollider>().size.y;
            //    this.collisionOffset.x = -this.GetComponent<BoxCollider>().size.x / 2;
            //    Debug.Log("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
            //}
            //moves.x = PlatformPart.speedMove * 50;
            //this.platformCollision = other;
            //this.collisionOffset = this.transform.position - this.platformCollision.transform.position;


            //make the spacing in cubes of 10
            //int polarise = this.collisionOffset.x % spacing < spacing / 2.0f ? -1 : 1;
            //this.collisionOffset.x += this.collisionOffset.x % spacing * polarise;
            //this.collisionOffset.y -= this.collisionOffset.y % spacing;
            //this.collisionOffset.z -= this.collisionOffset.z % spacing;
            //this.collisionOffset.x = Blockify.Apply(this.collisionOffset.x);
            //this.collisionOffset.y = Blockify.Apply(this.collisionOffset.y);
            //this.collisionOffset.z = Blockify.Apply(this.collisionOffset.z);
            //float awesomeShotThreshold = 2.0f;

            //if (Math.Abs(this.collisionOffset.x) < awesomeShotThreshold)
            //{
            //    Debug.Log("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
            //    this.collisionOffset.x = 0;
            //}

            //Debug.Log("NO FORCE");
        }
    }

    private void OnDestroy()
    {
        //Debug.Log("CUBE IS DESTROYED");
    }

    // Update is called once per frame
    void Update () {

        if (this.collided)
        {
            //halt gravity and forward motion
            moves.z = 0f;
            moves.y = 0f;
            //we check that this platform has not been destroyed and follow it
            if(this.platformCollision != null)
            {
                this.transform.SetPositionAndRotation(this.platformCollision.transform.position + this.collisionOffset, this.transform.rotation);
            }
            //platform was destroyed, so destroy this cube
            else
            {
                //this.Reset();
                Destroy(this);
            }

            //move with conveyor belt
            //Vector3 speed = new Vector3(0.0f, 0.0f, PlatformPart.speedMove);
            //this.transform.Translate(speed);
        }
        //else
        //{

            CharacterController Controller = gameObject.GetComponent<CharacterController>();

            if (Input.GetKeyDown(KeyCode.Space) && !this.flipMode && !this.collided)
            {
                //CubeAnimation.SetBool("AnimState", true);
                moves.y = jumpforce;
                moves.z = 20f;
                //Debug.Log("JUMP!");
                this.flipMode = true;
            }
            moves.y -= gravity * Time.deltaTime;

            Controller.Move(moves * Time.deltaTime);
        //}

        if(this.transform.position.y < -20.0f || this.transform.position.x < this.maxDist)
        {
            Destroy(this);
        }

        if(this.flipMode && !this.collided)
        {
            this.transform.Rotate(12f, 0.0f, 0.0f);
        }

        //update explosion to move with player
        if(this.tempExplosion != null)
            this.tempExplosion.iterateBasePosition(new Vector3(-0.005f, 0, 0));

    }

    private void Reset()
    {
        this.moves = Vector3.zero;
        this.collided = false;  
        this.transform.SetPositionAndRotation(this.startingPos, this.transform.rotation);
        this.platformCollision = null;
        this.collisionOffset = Vector3.zero;
    }

    public bool getCollided() { return this.collided; }

}