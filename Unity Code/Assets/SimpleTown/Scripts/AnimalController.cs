using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalController : MonoBehaviour {

    Vector3 start;

    Transform player;

    public bool flying;

    public int speed;

    bool chase = false;

    float time;

	// Use this for initialization
	void Start () {
        start = transform.position;
        time = Time.time;

	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (chase)
        {
            transform.LookAt(player);
            transform.Rotate(Vector3.up, -90);
            transform.Translate(speed * Time.deltaTime, 0, 0);
        }
	}
    void Halt()
    {
        chase = false;
    }
    void Respawn()
    {
        transform.position = start;
        chase = false;
        time = Time.time;
    }
    private void OnTriggerEnter(Collider other)
    {
        string target = "Bycicle_02_Col_03";
        if (flying)
            target = "Cube";
        if (other.gameObject.name.Equals(target))
        {
            if ((Time.time - time) > Random.Range(0, 100))
            {
                if (player == null)
                    player = other.transform;
                chase = true;
            }
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
