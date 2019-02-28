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
using UnityEngine.UI;

/// <summary>
/// Determines how helmet movements will interact with the scene and with the player.
/// </summary>
public class HelmetController : MonoBehaviour
{

    /// <summary>
    /// The player property represents the helmet's target for orientation with the player object.
    /// </summary>
    [SerializeField]
    public Transform player;

    /// <summary>
    /// The animator property represents the player's animator device.
    /// </summary>
    Animator animator;

	/// <summary>
    /// The dead property determines whether the player has died or not.
    /// </summary>
    public bool dead = false;

    /// <summary>
    /// The rb property represents the helmet object within the game.
    /// </summary>
    private Rigidbody rb;

    /// <summary>
    /// damageText represents the text on the screen that represents how much damage the player has taken
    /// </summary>
    public Text damageText; 

    /// <summary>
    /// speedText represents the text on the screen that represents the speed at which the player is moving.
    /// </summary>
    public Text speedText;

    /// <summary>
    /// The death screen image represents the image shown once the player has died.
    /// </summary>
    public Image deathScreen;

    /// <summary>
    /// damageTracker property keeps track of the amount of damage that the player has been dealt.
    /// </summary>
    float damageTracker;

    /// <summary>
    /// lastPosition property keeps track of the position at which the player was last at in relation to the map.
    /// </summary>
    Vector3 lastPosition = Vector3.zero;

    /// <summary>
    /// The active camera property represents the camera that is available when the player is alive.
    /// </summary>
    public Camera activeCamera;

    /// <summary>
    /// The deathCamera property represents the camera that is shown when the player dies.
    /// </summary>
    public Camera deathCamera;

    /// <summary>
    /// Start is a method called at the beginning of the scene which initializes the properties for the HelmetController.
    /// </summary>
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = player.GetComponent<Animator>();
        damageTracker = 0;
        deathScreen.color = new Color(0, 0, 0, 0);
        activeCamera.gameObject.SetActive(true);
        deathCamera.gameObject.SetActive(false);
    }

    /// <summary>
    /// Update is called whenever the scene updates and determines user input in order repawn or move the character.
    /// </summary>
    void Update()
    {
		// Gets values for animator
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        bool space = Input.GetKeyDown("space");


        
        // Actions performed when the spacebar is pressed
        if (space)
        {
            dead = false;
            damageTracker = 0;
            deathScreen.color = new Color(0, 0, 0, 0);
            activeCamera.gameObject.SetActive(true);
            deathCamera.gameObject.SetActive(false);
        }

		// Update animator
        animator.SetFloat("Speed_f", v);
        //animator.SetFloat("Strafe", h);
        animator.SetBool("Death_b", dead);
    }

    /// <summary>
    /// FixedUpdate is called every frame and updates the text on the screen as well as moving the player to a current position.
    /// </summary>
    void FixedUpdate()
    {
        damageText.text = "DAMAGE: " + damageTracker.ToString();
        speedText.text = "SPEED: " + 30 * (transform.position - lastPosition).magnitude;
        lastPosition = transform.position;
    }

    /// <summary>
    /// OnCollisionEnter is called whenever the helmet makes a collision. It updates the player's damage and calls the death function if the player has taken too much damage.
    /// </summary>
    /// <param name="col">col represents the object that makes a collision.</param>
    void OnCollisionEnter(Collision col)
    {
        damageTracker += (transform.position - lastPosition).magnitude;
        print("Damage: " + damageTracker);
        if (damageTracker > 25)
            StartCoroutine(deathTrigger());  
    }

    /// <summary>
    /// deathTrigger is called whenever the player is set to die and sets the deathCamera and deathScreen accordingly.
    /// </summary>
    IEnumerator deathTrigger()
    {
        dead = true;
        activeCamera.gameObject.SetActive(false);
        deathCamera.gameObject.SetActive(true);

        yield return new WaitForSeconds(5);
        if(dead)
            deathScreen.color = new Color(0, 0, 0, 255);
    }
}