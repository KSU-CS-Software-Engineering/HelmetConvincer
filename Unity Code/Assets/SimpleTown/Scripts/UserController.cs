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

using UnityEngine;
using System.Collections;

/// <summary>
/// Determines the player's interaction with the scene that the player is in
/// </summary>
public class UserController : MonoBehaviour
{

    /// <summary>
    /// The TurnSpeed property represents the value by which the players rotation is scaled.
    /// </summary>
    public float TurnSpeed;

    /// <summary>
    /// The MoveSpeed property represents the value by which the players movement is scaled.
    /// </summary>
    public float MoveSpeed;

    /// <summary>
    /// The rb property represents the player object.
    /// </summary>
    private Rigidbody rb;

    /// <summary>
    /// rider represents how the rider's movement will be controlled when the player moves.
    /// </summary>
    public Transform rider;

    /// <summary>
    /// riderOffsetPosition represents the rider's position on the map.
    /// </summary>
    private Vector3 riderOffsetPosition;

    /// <summary>
    /// Map represents the current map that is being used
    /// </summary>
    public int Map;

    /// <summary>
    /// MapArray keeps track of the four maps in which the player may respawn.
    /// </summary>
    public Vector3[] MapArray;

    /// <summary>
    /// distToGround determines the position of the bike relative to the plane representing the ground.
    /// </summary>
    float distToGround;

    /// <summary>
    /// playerLayer represents the of the playing field on which the player is located.
    /// </summary>
    public LayerMask playerLayer = (1 << 8);



    /// <summary>
    /// Start is a method called at the scene's start, which gets the player component and sets its position on the map.
    /// </summary>
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        riderOffsetPosition = transform.position + rider.transform.position;
        Respawn();
        //playerLayer = ~playerLayer;
        distToGround = GetComponent<Collider>().bounds.extents.y;
    }

    /// <summary>
    /// Repawn sets the players position on the map at one of the set respawn points
    /// </summary>
    void Respawn()
    {
        transform.localEulerAngles = new Vector3(0, 0, 0);
        transform.Translate(0, 0.1f, 0);
        rider.position = transform.position + new Vector3(0, 0.5f, -0.3f);
        int mapSelection = Random.Range(0, 3);
        Map++;
        if (Map > 3)
            Map = 0;
        transform.position = MapArray[(4 * Map) + mapSelection];
    }

    /// <summary>
    /// FixedUpdate is a method called on a regular basis whenever the scene needs to be updated.
    /// </summary>
    void FixedUpdate()
    {
        // Gets the value of the control axes
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        transform.Rotate(Vector3.up, moveHorizontal * TurnSpeed * Time.deltaTime);
        //transform.Rotate(new Vector3(-1, 0, 0));

        RaycastHit hit;
        ///Controlls how the player object will move during a collision
        if (Physics.Raycast(transform.position + Vector3.up, -Vector3.up, out hit, playerLayer))
        {
            //print("Found an object - distance: " + hit.distance);
            if (hit.distance < 1)
                hit.distance = 1;
            transform.Translate(0, 0, moveVertical * (MoveSpeed / hit.distance) * Time.deltaTime);
            //print("Found an object - distance: " + hit.distance);
        }//        rb.AddTorque(transform.up * 10);

        // Actions performed when the spacebar is pressed
        if (Input.GetKeyDown("space"))
        {
            Respawn();
        }
    }

}