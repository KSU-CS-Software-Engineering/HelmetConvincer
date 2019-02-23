MIT License

Copyright (c) 2019 KSU-CS-Software-Engineering

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// TreeController controls the behavior of trees to determine when and how they should begin to fall during the scene.
/// </summary>
public class TreeController : MonoBehaviour
{

    /// <summary>
    /// falling indicates whether the tress should begin falling or not
    /// </summary>
    bool falling = false;

    /// <summary>
    /// time property represents the time left during gameplay
    /// </summary>
    float time;

    /// <summary>
    /// Start initializes the behavior of trees at the beginning of the scene.
    /// </summary>
    void Start()
    {
        transform.localEulerAngles = new Vector3(0, 0, 0);
        time = Time.time;
    }

    /// <summary>
    /// FixedUpdate determines each frame whether the trees should be falling or not, and if they should, it controlls the movement of the falling trees.
    /// </summary>
    void FixedUpdate()
    {
        if (falling)
        {
            if (transform.localEulerAngles.z < 70)
                transform.Rotate(Vector3.forward, 50 * Time.deltaTime);
        }
    }

    /// <summary>
    /// Respawn resets the trees to their inital values so that they do not begin falling at the beginning of the scene.
    /// </summary>
    void Respawn()
    {
        transform.localEulerAngles = new Vector3(0, 0, 0);
        falling = false;
        time = Time.time;
    }

    /// <summary>
    /// OnTriggerEnter determines a time for the trees to begin falling and sets 'falling' to true so that they will begin to move.
    /// </summary>
    /// <param name="other">Other represents the game object that controls the trees.</param>
    private void OnTriggerEnter(Collider other)
    {
        if ((Time.time - time) > Random.Range(0, 100))
        {
            if (other.gameObject.name.Equals("Cube"))
                falling = true;
        }
    }

}
