using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeController : MonoBehaviour {

    bool falling = false;
	// Use this for initialization
	void Start () {
        transform.localEulerAngles = new Vector3(0, 0, 0);
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (falling)
        {
            if(transform.localEulerAngles.z < 70)
                transform.Rotate(Vector3.forward, 50 * Time.deltaTime);
        }
    }

    void Respawn()
    {
        transform.localEulerAngles = new Vector3(0, 0, 0);
        falling = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Equals("Cube"))
            falling = true;
    }

}
