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