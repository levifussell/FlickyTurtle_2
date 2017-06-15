using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour {

    public GameObject explosion;

	// Use this for initialization
	void Start () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        GameObject explosionTemp = (Instantiate(explosion, this.transform.position, this.transform.rotation) as GameObject);

        Destroy(this.gameObject);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
