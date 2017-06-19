using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FlipScript : MonoBehaviour {

    public bool waitingToJump; //true if the turtle is in line, false otherwise

    public Explosion explosionGroundHit;
    public Explosion explosionAwesomeHit;  
    private Explosion tempExplosion;

    private float jumpforce = 65f;
    private float gravity = 250;
    public Vector3 moves = Vector3.zero;
    private Vector3 startingPos;
    private bool collided;
    private Collider platformCollision;
    private Vector3 collisionOffset;
    private float maxDist;
    private bool awesomeCollisionDetection; //detect for a perfect collision only once
    //private Animator CubeAnimation;
    public bool flipMode;
    private float flipDirection;
    private bool flipAndDie;
    private Quaternion startRotation;

    private SpawnCube.CubeTypes type;

    // Use this for initialization
    void Start () {
        //CubeAnimation = GetComponent<Animator>();
        this.startingPos = this.transform.position;
        this.collided = false;
        this.awesomeCollisionDetection = false;
        this.maxDist = this.transform.position.x - 100.0f;
        this.flipMode = false;
        this.flipAndDie = false;
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

        //if we are flipping and dying we don't care about collisions
        if (this.flipAndDie)
            return;

        if (other.name.CompareTo("middle_block" + Platform.NAME_EXTEND_BOUNCEY) == 0
            || other.name.CompareTo("first_block" + Platform.NAME_EXTEND_BOUNCEY) == 0)
        {
            moves.y = jumpforce * 1.0f;
            moves.z = 0.0f;
            moves.x = -SpawnBelt.PLATFORM_SPEED * 60.0f;
        }

        //Debug.Log("NAME---- " + other.name);
        if (other.name.CompareTo("middle_block") == 0 || other.name.CompareTo("first_block") == 0)
            //|| other.name.CompareTo("middle_block") == 0)
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
                this.collisionOffset.y = other.GetComponent<BoxCollider>().size.y * 1.5f;

            }

            if ((other.name.CompareTo("first_block") == 0)
                && !this.awesomeCollisionDetection)
            {
                float distX = (this.transform.position.x + this.GetComponent<BoxCollider>().size.x / 2) - other.transform.position.x;
                Debug.Log("dist to awesome: " + distX);
                //7.5 > , < 7.38
                if ((this.type == SpawnCube.CubeTypes.Normal && distX < 3.9f && distX > 3.45f)
                    || (this.type == SpawnCube.CubeTypes.Big && distX < 7.55f && distX > 7.15f))
                {
                    this.platformCollision = other;
                    this.collisionOffset = this.transform.position - this.platformCollision.transform.position;
                    this.collisionOffset.y = other.GetComponent<BoxCollider>().size.y * 1.5f; //+ this.transform.GetComponent<BoxCollider>().size.y;
                    //this.collisionOffset.x = -this.GetComponent<BoxCollider>().size.x / 2;
                    this.tempExplosion = (Instantiate(explosionAwesomeHit, this.transform.position, this.transform.rotation) as Explosion);
                    Debug.Log("--------------------!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                }
                //here we check for the case to make the turtle fly to the right and die
                else if ((this.type == SpawnCube.CubeTypes.Normal && distX < 2.55f)
                    || (this.type == SpawnCube.CubeTypes.Big && distX < 6.15f))
                {
                    moves.y = 3.0f;
                    moves.x = -10.0f;
                    this.collided = false;
                    this.flipAndDie = true;
                    Debug.Log("FLY AnD DIE!!");
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
                Destroy(this.gameObject);
            }

            //move with conveyor belt
            //Vector3 speed = new Vector3(0.0f, 0.0f, PlatformPart.speedMove);
            //this.transform.Translate(speed);
        }
        //else
        //{

        if (PlayButton.GAME_MODE == PlayButton.GameMode.PAUSE_START && (this.flipMode || this.collided))
            Destroy(this.gameObject);

        if (!this.waitingToJump)
        {
            CharacterController Controller = gameObject.GetComponent<CharacterController>();

            if ((Input.GetKeyDown(KeyCode.Space) || PlayButton.GAME_MODE == PlayButton.GameMode.GAME_OVER)
                && !this.flipMode && !this.collided && !this.waitingToJump)
            {
                //CubeAnimation.SetBool("AnimState", true);
                moves.y = jumpforce;

                if (PlayButton.GAME_MODE == PlayButton.GameMode.GAME_OVER)
                    moves.x = 30.0f * UnityEngine.Random.Range(-1.0f, 1.0f);

                moves.z = PlayButton.GAME_MODE == PlayButton.GameMode.GAME_OVER ? 40f : 20f;
                //Debug.Log("JUMP!");
                this.flipMode = true;
                //this.flipDirection = UnityEngine.Random.Range(0.0f, 1.0f) > 0.5f ? 1.0f : -1.0f;
                this.flipDirection = 1.0f;
}
            moves.y -= gravity * Time.deltaTime;

            Controller.Move(moves * Time.deltaTime);
        }
        //}

        //game over scenario because turtle mised platform
        if (this.transform.position.y < -20.0f)
        {
            PlayButton.GameOverCall();
            Destroy(this.gameObject);
        }

        if(this.transform.position.x < this.maxDist)
        {
            Destroy(this.gameObject);
        }

        if(this.flipMode && !this.collided)
        {
            float zRoll = this.flipAndDie ? 12.0f : 0.0f; //if we are dying, roll to the left
            this.transform.Rotate(12f * this.flipDirection, 0.0f, zRoll);
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
    public bool getFlipMode() { return this.flipMode; }

    public void setCubeType(SpawnCube.CubeTypes type) { this.type = type; }

}
