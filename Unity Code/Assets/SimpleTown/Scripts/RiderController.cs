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

using UnityEngine;
using System.Collections;

/// <summary>
/// Determines how the rider object interacts with it's bike and with the rest of the scene.
/// </summary>
public class RiderController : MonoBehaviour
{
    /// <summary>
    /// The animator property represents the player's animator, and will determine how the player moves.
    /// </summary>
    Animator animator;
	
    /// <summary>
    /// Start is called at the beginning of the scene to acquire an animator for the rider.
    /// </summary>
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    /// <summary>
    /// Update is called every time the scene is updated and retrieves input from the user to determine the rider's movement.
    /// </summary>
    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        bool space = Input.GetKeyDown("space");

        animator.SetFloat("Speed_f", v);
        //animator.SetFloat("Strafe", h);
        animator.SetBool("Death_b", space);
    }
}