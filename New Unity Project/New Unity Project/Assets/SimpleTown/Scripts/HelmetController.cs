using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>Determines how the helmet interacts with the scene and the player.</summary>
public class HelmetController : MonoBehaviour
{
    [SerializeField]
    public Transform player;
    /// <summary>The player property represents the helmet's target for orientation.</summary>
    /// <value>The player property represents the value of the target object.</value>

    Animator animator;
	/// <summary>The animator property represents the player.</summary>
    /// <value>The animator property represents the player's animator.</value>
	
    public bool dead = false;
    /// <summary>The dead property determines if the player has died.</summary>
    /// <value>The dead property determines the state of the player's life</value>

    private Rigidbody rb;
    /// <summary>The rb property represents the helmet.</summary>
    /// <value>The rb property represents the helmet object.</value>


    public Text damageText;  // public if you want to drag your text object in there manually
    public Text speedText;
    public Image deathScreen;
    float damageTracker;

    Vector3 lastPosition = Vector3.zero;


        public Camera cam1;
        public Camera cam2;

    /// <summary>Start is a method called at the scene's start.</summary>
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = player.GetComponent<Animator>();
        damageTracker = 0;
        deathScreen.color = new Color(0, 0, 0, 0);
        cam1.gameObject.SetActive(true);
        cam2.gameObject.SetActive(false);
    }

    /// <summary>Update is a method called when the scene updates.</summary>
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
            cam1.gameObject.SetActive(true);
            cam2.gameObject.SetActive(false);
        }

		// Update animator
        animator.SetFloat("Speed_f", v);
        //animator.SetFloat("Strafe", h);
        animator.SetBool("Death_b", dead);
    }

    void FixedUpdate()
    {
        damageText.text = "DAMAGE: " + damageTracker.ToString();
        speedText.text = "SPEED: " + 30 * (transform.position - lastPosition).magnitude;
        lastPosition = transform.position;
    }

    /// <summary>OnCollisionEnter is a method called when the helmet collides with another rigidbody.</summary>
    void OnCollisionEnter(Collision col)
    {
        damageTracker += (transform.position - lastPosition).magnitude;
        print("Damage: " + damageTracker);
        if (damageTracker > 25)
            StartCoroutine(deathTrigger());
        
        
    }

    IEnumerator deathTrigger()
    {
        dead = true;
        cam1.gameObject.SetActive(false);
        cam2.gameObject.SetActive(true);

        yield return new WaitForSeconds(5);
        if(dead)
            deathScreen.color = new Color(0, 0, 0, 255);
    }
}