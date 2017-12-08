using UnityEngine;
using System.Collections;
using UnityEngine.VR;
using UnityEngine.UI;
//using System.Threading;
//using System.IO.Ports;
//using System.IO;
public class WorldController : MonoBehaviour
{
    [SerializeField]
    private Transform head;
	/// <summary>The head property represents the player's head.</summary>
    /// <value>The head transformation represents the player's head object.</value>
	
    [SerializeField]
    public Transform person;
	/// <summary>The person property represents the player's person.</summary>
    /// <value>The person transformation represents the player's model object.</value>
	
    [SerializeField]
    public Transform user;
	/// <summary>The user property represents the player.</summary>
    /// <value>The user transformation represents the player's entire object.</value>
	
    [SerializeField]
    public Transform deathCameraMan;
	/// <summary>The deathCameraMan property represents the box holding the deathcam.</summary>
    /// <value>The deathCameraMan transformation represents the object which holds the deathcam.</value>
	
    public Animator animator;
    /// <summary>The animator property represents the player.</summary>
    /// <value>The animator property represents the player's animator.</value>

    public Text damageText;
	/// <summary>The damageText property represents the player's damageText.</summary>
    /// <value>The damageText property represents the damageText that will be displayed to the player.</value>
	
    public Text speedText;
	/// <summary>The speedText property represents the player's speedText.</summary>
    /// <value>The speedText property represents the speedText that will be displayed to the player.</value>
	
    public Image deathScreen;
	/// <summary>The deathScreen property represents the player's deathScreen.</summary>
    /// <value>The deathScreen property represents the deathScreen that will be displayed upon player death.</value>
	
    float damageTracker;
	/// <summary>The damageTracker property tracks the player's damage.</summary>
    /// <value>The damageTracker property represents the total damage that has been dealth to the player.</value>

    Vector3 lastPosition = Vector3.zero;
	/// <summary>The lastPosition property tracks the player's postition.</summary>
    /// <value>The lastPosition property represents the lastPosition of the player object.</value>

    public Camera[] cameras;
	/// <summary>The cameras array contains all used cameras.</summary>
    /// <value>The cameras property represents all the cameras in the scene.</value>

    public Vector3[] cameraOffsetPositions;
	/// <summary>The cameraOffsetPositions array contains all used cameraOffsetPositions.</summary>
    /// <value>The cameraOffsetPositions property represents all the cameras in the scene offest positions.</value>
	
    public int activeCam;
	/// <summary>The activeCam integer stores the active camera's array location.</summary>
    /// <value>The activeCam property represents which camera is currently active in the scene.</value>
	
    public float TurnSpeed;
    /// <summary>The TurnSpeed property determines the player's turn speed.</summary>
    /// <value>The TurnSpeed property represents the value by which the players rotation is scaled.</value>

    public float MoveSpeed;
    /// <summary>The MoveSpeed property determines the player's move speed.</summary>
    /// <value>The MoveSpeed property represents the value by which the players movement is scaled.</value>

    public int Map;
	/// <summary>The Map property determines which map to use.</summary>
    /// <value>The Map property represents the value by which the player's current map is determined.</value>
	
    public Vector3[] MapArray;
	/// <summary>The MapArray array determines the player's starting postions.</summary>
    /// <value>The MapArray property represents each possible player starting postion.</value>

    private LayerMask playerLayer = (1 << 8);
	/// <summary>The playerLayer layer represents all player objects.</summary>
    /// <value>The playerLayer property takes out the player from the gravity script calculations.</value>

    //SerialPort myPort;

    /// <summary>Start is a method called at the scene's start.</summary>
    void Start()
    {
        //animator = person.GetComponent<Animator>();
        //myPort = new SerialPort("COM5", 9600);
        //myPort.Open();

		/// Set up the cameras
        float[] distances = new float[32];
        distances[9] = 1000;
        cameras[0].layerCullDistances = distances;
        cameras[1].layerCullDistances = distances;

        Respawn();
    }
	
	/// <summary>Respawn is a method called at the player's respawn.</summary>
    void Respawn()
    {        
		/// Set the player's new position
        user.transform.localEulerAngles = new Vector3(0, 0, 0);
        user.transform.Translate(0, 0.1f, 0);
        person.position = user.transform.position + new Vector3(0, 0.5f, -0.3f);

		/// Select the map
        int mapSelection = Random.Range(0, 3);
        Map++;
        if (Map > 3)
            Map = 0;
        user.transform.position = MapArray[(4 * Map) + mapSelection];

		/// Reset values for reset
        damageTracker = 0;
        deathScreen.color = new Color(0, 0, 0, 0);

        activeCam = 0;
        cameras[0].gameObject.SetActive(true);
        cameras[1].gameObject.SetActive(false);
    }

	/// <summary>FixedUpdate is a method called at the during the scenes fixed update, used for movement calculations.</summary>
    void FixedUpdate()
    {
        /// Gets the value of the control axes
        float turningMovement = Input.GetAxis("Horizontal");
        float forwardMovement = Input.GetAxis("Vertical");

        //String arduino = myPort.readStringUntil('\n');

		/// Modifies the player movement based on source
        if (VRSettings.enabled)
        {
            Vector2 touchPosition = OVRInput.Get(OVRInput.Axis2D.PrimaryTouchpad);
            turningMovement = touchPosition.x;
            forwardMovement = touchPosition.y;
        }
        if (activeCam == 1)
        {
            turningMovement = 0;
            forwardMovement = 0;
        }

		/// Modify the player's postion based on inputs and speed		
        user.transform.Rotate(Vector3.up, turningMovement * TurnSpeed * Time.deltaTime);
        RaycastHit hit;
        if (Physics.Raycast(user.transform.position + Vector3.up, -Vector3.up, out hit, playerLayer))
        {
            if (hit.distance < 1)
                hit.distance = 1;
            user.transform.Translate(0, 0, forwardMovement * (MoveSpeed / hit.distance) * Time.deltaTime);
        }

        /// Actions performed when the reset button is pressed
        if (Input.GetKeyDown("space") || OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
        {
            Respawn();
        }

        //myPort.write(activeCam);

		/// Update varaibles needed between loops
        damageText.text = "DAMAGE: " + damageTracker;
        //speedText.text = "SPEED: " + arduino;
        lastPosition = transform.position;
    }

    /// <summary>Update is a method called when the scene updates, used for visual calculations.</summary>
    private void Update()
    {
		/// UPDATE VR DEVICE INPUTS
        OVRInput.Update();
		/// DeathCam positioning
        if (activeCam == 1)
        {
            cameras[activeCam].transform.position = user.position + cameraOffsetPositions[activeCam];
            cameras[activeCam].transform.LookAt(user);
            deathCameraMan.transform.position = user.position + cameraOffsetPositions[activeCam];
            deathCameraMan.transform.LookAt(user);
        }
        /// Look around using VR Device
        else if (VRSettings.enabled)
        {
            cameras[activeCam].transform.position = head.TransformPoint(cameraOffsetPositions[activeCam]);
            float VRx = InputTracking.GetLocalRotation(VRNode.Head).eulerAngles.x;
            float VRy = InputTracking.GetLocalRotation(VRNode.Head).eulerAngles.y;
            float VRz = InputTracking.GetLocalRotation(VRNode.Head).eulerAngles.z;
            cameras[activeCam].transform.Rotate(Vector3.right, VRx);
            cameras[activeCam].transform.Rotate(Vector3.up, VRy);
            cameras[activeCam].transform.Rotate(Vector3.forward, VRz);
        }
        /// Look around using mouse
        else
        {
            cameras[activeCam].transform.position = head.TransformPoint(cameraOffsetPositions[activeCam]);
            float mouseHorizontal = Input.GetAxis("Mouse X");
            float mouseVertical = Input.GetAxis("Mouse Y");
            cameras[activeCam].transform.Rotate(Vector3.up, mouseHorizontal);
            cameras[activeCam].transform.Rotate(Vector3.right, -mouseVertical);
        }
    }

    /// <summary>OnCollisionEnter is a method called when the helmet collides with another rigidbody.</summary>
    void OnCollisionEnter(Collision col)
    {
		/// Add damage during collision instance to damageTracker
        damageTracker += 60 * (transform.position - lastPosition).magnitude;
        print("Found an object - distance: " + damageTracker);
        if (damageTracker > 25)
            StartCoroutine(deathTrigger());
    }

	/// <summary>DeathTrigger is a method called whenever damage passes the death threshold.</summary>
    IEnumerator deathTrigger()
    {
		/// Set deathCam as active camera
        activeCam = 1;
        cameras[0].gameObject.SetActive(false);
        cameras[1].gameObject.SetActive(true);

		/// Black out after 5 seconds if still on deathCam
        yield return new WaitForSeconds(5);
        if (activeCam == 1)
            deathScreen.color = new Color(0, 0, 0, 100);
    }
}

