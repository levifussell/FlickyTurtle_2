using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blockify : MonoBehaviour {

    public static float spacing = 0.5f;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public static float Apply(float position)
    {
        int polarise = position % spacing < spacing / 2.0f ? -1 : 1;
        return position + position % spacing * polarise;
    }
}
