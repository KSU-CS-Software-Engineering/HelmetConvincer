using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalController : MonoBehaviour
{

    Vector3 start;

    Transform player;

    public bool flying;

    public int speed;

    bool chase = false;

    float time;

    // Use this for initialization
    void Start()
    {
        start = transform.position;
        time = Time.time;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //When chase is true the animal will chase the person.
        if (chase)
        {
            transform.LookAt(player);
            transform.Rotate(Vector3.up, -90);
            transform.Translate(speed * Time.deltaTime, 0, 0);
        }
    }

    //Method used from the World Controller class to tell animals to stop chasing the person
    //when the person is injured or the time runs out.
    void Halt()
    {
        chase = false;
    }

    //Method used to Initialize the animals at the begining of a round.
    void Respawn()
    {
        transform.position = start;
        chase = false;
        time = Time.time;
    }

    //Method called when a person enters a certain radius around the animal 
    //and tells the animal to chase them out of it
    private void OnTriggerEnter(Collider other)
    {
        string target = "Bycicle_02_Col_03";
        if (flying)
            target = "Cube";
        if (other.gameObject.name.Equals(target))
        {
            if (player == null)
                player = other.transform;
            chase = true;
        }
    }

    //Method called when a person leaves the radius of chasing to tell the animal to stop
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name.Equals("Bycicle_02_Col_03"))
        {
            chase = false;
        }
    }
}
