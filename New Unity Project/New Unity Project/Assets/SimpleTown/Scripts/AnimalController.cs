using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalController : MonoBehaviour {

    Vector3 start;

    Transform player;

    bool chase = false;

	// Use this for initialization
	void Start () {
        start = transform.position;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (chase)
        {
            transform.LookAt(player);
            transform.Rotate(Vector3.up, -90);
            transform.Translate(10 * Time.deltaTime, 0, 0);
        }
	}
    void Respawn()
    {
        transform.position = start;
        chase = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Equals("Bycicle_02_Col_03"))
        {
            if (player == null)
                player = other.transform;
            chase = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name.Equals("Bycicle_02_Col_03"))
        { 
            chase = false;
        }
    }
}
