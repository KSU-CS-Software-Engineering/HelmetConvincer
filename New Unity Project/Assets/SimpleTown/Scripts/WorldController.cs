using UnityEngine;
using System.Collections;
using UnityEngine.VR;
using UnityEngine.UI;
using System.Threading;
using System.IO.Ports;
using System.IO;

public class WorldController : MonoBehaviour
{
    [SerializeField]
    private Transform head;

    [SerializeField]
    public Transform person;

    [SerializeField]
    public Transform user;

    Animator animator;
    /// <summary>The animator property represents the player.</summary>
    /// <value>The animator property represents the player's animator.</value>

    public Text damageText;
    public Text damageText2;
    public Text speedText;
    public Image deathScreen;
    float damageTracker;

    Vector3 lastPosition = Vector3.zero;

    public Camera[] cameras;
    public Vector3[] cameraOffsetPositions;
    public int activeCam;



    public float TurnSpeed;
    /// <summary>The TurnSpeed property determines the player's turn speed.</summary>
    /// <value>The TurnSpeed property represents the value by which the players rotation is scaled.</value>

    public float MoveSpeed;
    /// <summary>The MoveSpeed property determines the player's move speed.</summary>
    /// <value>The MoveSpeed property represents the value by which the players movement is scaled.</value>

    public int Map;

    public Vector3[] MapArray;

    public LayerMask playerLayer = (1 << 8);
	
	SerialPort myPort; 
	
    /// <summary>Start is a method called at the scene's start.</summary>
    void Start()
    {
        //animator = person.GetComponent<Animator>();
		myPort = new SerialPort("COM5", 9600);
		myPort.Open();
        Respawn();
    }
    void Respawn()
    {        
        user.transform.localEulerAngles = new Vector3(0, 0, 0);
        user.transform.Translate(0, 0.1f, 0);
        person.position = user.transform.position + new Vector3(0, 0.5f, -0.3f);

        int mapSelection = Random.Range(0, 3);
        Map++;
        if (Map > 3)
            Map = 0;
        user.transform.position = MapArray[(4 * Map) + mapSelection];

        damageTracker = 0;
        deathScreen.color = new Color(0, 0, 0, 0);

        activeCam = 0;
        cameras[0].gameObject.SetActive(true);
        cameras[1].gameObject.SetActive(false);
    }

    void FixedUpdate()
    {
        // Gets the value of the control axes
        float turningMovement = Input.GetAxis("Horizontal");
        float forwardMovement = Input.GetAxis("Vertical");
		
		String arduino = myPort.readStringUntil('\n');
		
		
        if (VRSettings.enabled)
        {
            Vector2 touchPosition = OVRInput.Get(OVRInput.Axis2D.PrimaryTouchpad);
            turningMovement = touchPosition.x;
            forwardMovement = touchPosition.y;
        }

        if(activeCam == 1)
        {
            turningMovement = 0;
            forwardMovement = 0;
        }

        user.transform.Rotate(Vector3.up, turningMovement * TurnSpeed * Time.deltaTime);

        RaycastHit hit;
        if (Physics.Raycast(user.transform.position + Vector3.up, -Vector3.up, out hit, playerLayer))
        {
            if (hit.distance < 1)
                hit.distance = 1;
            user.transform.Translate(0, 0, forwardMovement * (MoveSpeed / hit.distance) * Time.deltaTime);
        }

        // Actions performed when the reset button is pressed
        if (Input.GetKeyDown("space"))
        {
            Respawn();
        }

        damageText.text = "DAMAGE: " + damageTracker;
        damageText2.text = "DAMAGE: " + damageTracker;
		speedText.text = "SPEED: " + arduino;
		myPort.write(activeCam);
        //speedText.text = "SPEED: " + 30 * (transform.position - lastPosition).magnitude;
        lastPosition = transform.position;
    }

    /// <summary>Update is a method called when the scene updates.</summary>
    private void Update()
    {
        if (activeCam == 1)
        {
            cameras[activeCam].transform.position = user.position + cameraOffsetPositions[activeCam];
            cameras[activeCam].transform.LookAt(user);
        }
        // Computes rotation
        //Look around using VR Device
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
        //Look around using mouse
        else
        {
            cameras[activeCam].transform.position = head.TransformPoint(cameraOffsetPositions[activeCam]);
            ///Rotate based on mouse movement
            float mouseHorizontal = Input.GetAxis("Mouse X");
            float mouseVertical = Input.GetAxis("Mouse Y");
            cameras[activeCam].transform.Rotate(Vector3.up, mouseHorizontal);
            cameras[activeCam].transform.Rotate(Vector3.right, -mouseVertical);
        }
    }

    /// <summary>OnCollisionEnter is a method called when the helmet collides with another rigidbody.</summary>
    void OnCollisionEnter(Collision col)
    {
        damageTracker += 60 * (transform.position - lastPosition).magnitude;
        print("Found an object - distance: " + damageTracker);
        if (damageTracker > 25)
            StartCoroutine(deathTrigger());
    }

    IEnumerator deathTrigger()
    {
        activeCam = 1;
        cameras[0].gameObject.SetActive(false);
        cameras[1].gameObject.SetActive(true);

        yield return new WaitForSeconds(5);
        if (activeCam == 1)
            deathScreen.color = new Color(0, 0, 0, 255);
    }
}
