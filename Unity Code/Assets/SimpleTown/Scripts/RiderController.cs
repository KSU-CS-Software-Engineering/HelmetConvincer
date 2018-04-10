using UnityEngine;
using System.Collections;

/// <summary>Determines how the rider interacts with the bike and the scene.</summary>
public class RiderController : MonoBehaviour
{
    Animator animator;
	/// <summary>The animator property represents the player.</summary>
    /// <value>The animator property represents the player's animator.</value>
	
    /// <summary>Start is a method called at the scene's start.</summary>
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    /// <summary>Update is a method called when the scene updates.</summary>
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