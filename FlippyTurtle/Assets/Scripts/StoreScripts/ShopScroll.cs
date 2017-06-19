using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopScroll : MonoBehaviour {

    private float movementStop = 0;
    private float movementTime = .1f;
    private bool isMovingLeft = false;
    private bool isMovingRight = false;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
        if(Input.GetKeyDown(KeyCode.LeftArrow) && isMovingRight == false)
        {
            movementStop = Time.time + movementTime;
            isMovingLeft = true;
        }

        if (isMovingLeft == true)
        {
            if (movementStop > Time.time)
            {
                transform.Translate(Vector2.left * 200 * Time.deltaTime);
            }
            else
            {
                isMovingLeft = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.RightArrow) && isMovingLeft == false)
        {
            movementStop = Time.time + movementTime;
            isMovingRight = true;
        }

        if (isMovingRight == true)
        {
            if(movementStop > Time.time)
            {
                transform.Translate(Vector2.right * 200 * Time.deltaTime);
            }
            else
            {
                isMovingRight = false;
            }

        }


    }
}
