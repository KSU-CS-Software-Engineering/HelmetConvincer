//MIT License

//Copyright (c) 2019 KSU-CS-Software-Engineering

//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:

//The above copyright notice and this permission notice shall be included in all
//copies or substantial portions of the Software.

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//SOFTWARE.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalController : MonoBehaviour
{
    /// <summary>
    /// start represents the starting position for an animal.
    /// </summary>
    Vector3 start;

    /// <summary>
    /// player represents where the player is in relation to an animal.
    /// </summary>
    Transform player;

    /// <summary>
    /// flying determines whether or not an animal is flying (applies to bird animals).
    /// </summary>
    public bool flying;

    /// <summary>
    /// speed determines the speed at which an animal should move.
    /// </summary>
    public int speed;

    /// <summary>
    /// chase determines whether or not an animal should begin moving towards the player.
    /// </summary>
    bool chase = false;

    /// <summary>
    /// Time represents the current amount of time in the game.
    /// </summary>
    float time;

    /// <summary>
    /// Start is called at the beginning of the scene and determines the position for an animal.
    /// </summary>
    void Start()
    {
        start = transform.position;
        time = Time.time;
    }

    /// <summary>
    /// FixedUpdate is called once per frame and determines whether or not an animal should be moving towards the player.
    /// </summary>
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

    /// <summary>
    /// Halt is implemented within the WorldController and causes animals to halt when they are not in range of the player.
    /// </summary>
    void Halt()
    {
        chase = false;
    }

    /// <summary>
    /// Respawn initializes animals at the beginning of a round.
    /// </summary>
    void Respawn()
    {
        transform.position = start;
        chase = false;
        time = Time.time;
    }

    /// <summary>
    /// OnTriggerEnter is called whenever the player enters a radus of the animal
    /// </summary>
    /// <param name="other">Represents the player entering the radius of the animal.</param>
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

    /// <summary>
    /// OnTriggerExit is called whenever the player exits the radius of the animal and causes the animal to stop moving.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name.Equals("Bycicle_02_Col_03"))
        {
            chase = false;
        }
    }
}
