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
using UnityEngine.VR;
using UnityEngine.UI;
using System.Threading;
using System.IO.Ports;
using System.IO;

public class WorldController : MonoBehaviour
{
    /// <summary>
    /// The head property represents the player's head object.
    /// </summary>
    [SerializeField]
    private Transform head;

    /// <summary>
    /// The person property represents the player's person object. 
    /// </summary>
    [SerializeField]
    public Transform person;

    /// <summary>
    /// The user property represents the entirety of the user. 
    /// </summary>
    [SerializeField]
    public Transform user;

    /// <summary>
    /// deathCameraMan represents the camera box the holds the death camera.
    /// </summary>
    [SerializeField]
    public Transform deathCameraMan;

    /// <summary>
    /// The animator property represents the animator that controls the players movements.
    /// </summary>
    public Animator animator;

    /// <summary>
    /// The damageText property represents the text displaying the amount of damage the player has taken.
    /// </summary>
    public Text damageText;

    /// <summary>
    /// The speedText property represents the text that displays the speed at which the player is moving.
    /// </summary>
    public Text speedText;

    /// <summary>
    /// The deathScreen property is the screen that will be displayed whenever the user dies
    /// </summary>
    public Image deathScreen;

    /// <summary>
    /// The damageImage is a box that represents how much damage was taken by the user. It displays different colors depending on the amount of damage. 
    /// </summary>
    public Image damageImage;

    /// <summary>
    /// The damageTracker property represents the amount of damage that has been dealt to the player during the runtime. 
    /// </summary>
    float damageTracker;

    /// <summary>
    /// The lastPosition vector represents the last position on the playing field where the player has been. 
    /// </summary>
    Vector3 lastPosition = Vector3.zero;

    /// <summary>
    /// The cameras property keeps track of all of the cameras in the scene. 
    /// </summary>
    public Camera[] cameras;

    /// <summary>
    /// cameraOffsetPositions represents the positioning of each camera in the game. 
    /// </summary>
    public Vector3[] cameraOffsetPositions;

    /// <summary>
    /// activeCam property keeps track of which camera is currently displaying.
    /// </summary>
    public int activeCam;

    /// <summary>
    /// The TurnSpeed property represents the value by which the players rotation is scaled
    /// </summary>
    public float turnSpeed;

    /// <summary>
    /// The MoveSpeed property represents the value by which the players movement is scaled.
    /// </summary>
    public float moveSpeed;

    /// <summary>
    /// The Map property represents the value by which the player's current map is determined.
    /// </summary>
    public int map;

    /// <summary>
    /// The MapArray property represents each possible player starting postion.
    /// </summary>
    public Vector3[] mapArray;

    /// <summary>
    /// The playerLayer property takes out the player from the gravity script calculations.
    /// </summary>
    private LayerMask playerLayer = (1 << 8);

    /// <summary>
    /// SerialPort represents the port on which the game is currently operating
    /// </summary>
    private SerialPort myPort = new SerialPort("", 115200);

    /// <summary>
    /// speedSize property represents the magnitude of the speed of the bike
    /// </summary>
    private const int speedSize = 100;

    /// <summary>
    /// speeds property keeps track of various speed types, such as turning speed and the speed of forward movement
    /// </summary>
    private float[] speeds = new float[speedSize];

    /// <summary>
    /// turningMovement property represents how quickly the bike is turning
    /// </summary>
    private float turningMovement = 0;

    /// <summary>
    /// forwardMovement property keeps track of the magnitude of the bikes forward movement
    /// </summary>
    private float forwardMovement = 0;

    /// <summary>
    /// Time represents the amount of time left during the game.
    /// </summary>
    private float time = 0;

    /// <summary>
    /// animalController keeps track of the movements of animals in order to determine when they should move towards the player
    /// </summary>
    public GameObject animalController;

    /// <summary>
    /// Start is called at the beginning of the scene to initialize port values and repawn the player somewhere in the scene.
    /// </summary>
    void Start()
    {
        //animator = person.GetComponent<Animator>();

        string[] portNames = System.IO.Ports.SerialPort.GetPortNames();
        string ports = "";

        foreach(string portName in portNames)
        {
            myPort = new SerialPort(portName, 115200);
            if (myPort.IsOpen)
            {
                myPort.ReadTimeout = 250;
                myPort.Open();
                break;
            }
        }

        speedText.text = "TIME: " + ports;

        Respawn();
    }

    /// <summary>
    /// Respawn is called in order to have the player restart in one of the four possible locations. If called during the game, it will restart without the player dying.
    /// </summary>
    void Respawn()
    {
        ///Set possible locations where the player may respawn
        mapArray = new Vector3[4];
        mapArray[0] = new Vector3(-630, 0, 451);
        mapArray[1] = new Vector3(-469, 0, -171);
        mapArray[2] = new Vector3(9, 0, -305);
        mapArray[3] = new Vector3(193, 0, 421);
        /// Set the player's new position
        user.transform.localEulerAngles = new Vector3(0, 0, 0);
        user.transform.Translate(0, 0.1f, 0);
        person.position = user.transform.position + new Vector3(0, 0.5f, -0.3f);

        /// Select the map
        int mapSelection = Random.Range(0, 4);
        user.transform.position = mapArray[mapSelection];

        /// Reset values for reset
        damageTracker = 0;
        deathScreen.color = new Color(0, 0, 0, 0);
        damageImage.color = new Color(0, 0, 0, 0);
        animalController.BroadcastMessage("Respawn");
        for (int i = 0; i < speedSize; i++)
        {
            speeds[i] = 0;
        }
        activeCam = 0;
        cameras[0].gameObject.SetActive(true);
        cameras[1].gameObject.SetActive(false);
        time = Time.time;
    }

    /// <summary>
    /// Fixed update is called consistenly over a period of time in order to calculate the player's movement.
    /// </summary>
    void FixedUpdate()
    {
        user.transform.Rotate(Vector3.up, turningMovement * turnSpeed * Time.deltaTime);
        RaycastHit hit;
        ///Calculates the movement of the player if player collides with another object.
        if (Physics.Raycast(user.transform.position + Vector3.up, -Vector3.up, out hit, playerLayer))
        {
            if (hit.distance < 1)
            {
                hit.distance = 1;
            }

            user.transform.Translate(0, 0, forwardMovement * (moveSpeed / hit.distance) * Time.deltaTime);
        }
        // After 90 seconds the session ends.
        if (((Time.time - time) > 90) && (activeCam == 0))
        {
            StartCoroutine(deathTrigger());
        }


        /// Actions performed when the reset button is pressed
        if (Input.GetKeyDown("space") || OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
        {
            Respawn();
        }

        /// Update varaibles needed between loops
        damageText.text = "";
        speedText.text = "TIME: " + (90 - (Time.time - time)).ToString("F2");
        lastPosition = transform.position;
    }

    /// <summary>
    /// Update is called whenever the scene needs to update and it calculates visual aspects of the scene.
    /// </summary>
    private void Update()
    {
        string serialIn = "NOPE";
        ///Calculates arduino input but does we will not have any functional need until arduino is acquired.
        try
        {
            if (myPort.IsOpen)
            {
                serialIn = myPort.ReadLine();
            }
        }
        catch (System.Exception e)
        {
            serialIn = e.Message;
        }


        string[] serialValues = serialIn.Split(',');
        if (serialValues.Length == 2)
        {
            turningMovement = System.Convert.ToSingle(serialValues[1]);
            forwardMovement = System.Convert.ToSingle(serialValues[0]);

            if (turningMovement < 10)
            {
                if (forwardMovement > 0)
                {
                    forwardMovement = 1;
                }
            }
            else
            {
                forwardMovement = forwardMovement / turningMovement;
            }
            if (forwardMovement > 1)
            {
                forwardMovement = 1;
            }
            turningMovement -= 450;
            turningMovement = turningMovement / 450;
            if (turningMovement < .1 && turningMovement > -.1)
            {
                turningMovement = 0;
            }
            float total = 0;
            for (int i = 1; i < speedSize; i++)
            {
                speeds[i] = speeds[i - 1];
                total += speeds[i - 1];
            }
            speeds[0] = forwardMovement;
            total += speeds[0];
            forwardMovement = total / speedSize;
            if (forwardMovement > .5)
                forwardMovement = 0;
        }
        else
        {
            if (UnityEngine.XR.XRSettings.enabled)
            {
                Vector2 touchPosition = OVRInput.Get(OVRInput.Axis2D.PrimaryTouchpad);
                turningMovement = touchPosition.x;
                forwardMovement = touchPosition.y / 4;
            }
            else
            {
                forwardMovement = Input.GetAxis("Vertical") / 5;
                turningMovement = Input.GetAxis("Horizontal");
            }
        }

        if (activeCam == 1)
        {
            forwardMovement = 0;
            turningMovement = 0;
        }




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
        else if (UnityEngine.XR.XRSettings.enabled)
        {
            cameras[activeCam].transform.position = head.TransformPoint(cameraOffsetPositions[activeCam]);
            float VRx = UnityEngine.XR.InputTracking.GetLocalRotation(UnityEngine.XR.XRNode.Head).eulerAngles.x;
            float VRy = UnityEngine.XR.InputTracking.GetLocalRotation(UnityEngine.XR.XRNode.Head).eulerAngles.y;
            float VRz = UnityEngine.XR.InputTracking.GetLocalRotation(UnityEngine.XR.XRNode.Head).eulerAngles.z;
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

    /// <summary>
    /// OnCollisionEnter is a method called when the helmet collides with another rigidbody, attempts to determine where colision is made and will then results in particular death animation
    /// once implemented.
    /// </summary>
    /// <param name="col">Col is the part of the bike that collides with a rigidbody</param>
    void OnCollisionEnter(Collision col)
    {
        //if (col.gameObject.name.Contains("grass") || col.gameObject.name.Contains("road") || col.gameObject.name.Contains("animal"))
        //{
        //}
        ///// Add damage during collision instance to damageTracker
        //damageTracker = 100 * (transform.position - lastPosition).magnitude * Time.deltaTime;
        //if (damageTracker >= 1)
        //{
        //    StartCoroutine(deathTrigger());
        //}

        ///If back wheel makes contact with rigidbody, results in death
        if (col.Equals("Back_Wheel1"))
        {
            StartCoroutine(deathTrigger());
        }
        if (col.Equals("Back_Wheel"))
        {
            StartCoroutine(deathTrigger());
        }
        
    }

    /// <summary>
    /// DeathTrigger is a method called whenever the player has crashed and lost the game
    /// </summary>
    IEnumerator deathTrigger()
    {
        /// Set deathCam as active camera
        animalController.BroadcastMessage("Halt");
        activeCam = 1;
        cameras[0].gameObject.SetActive(false);
        cameras[1].gameObject.SetActive(true);

        /// Black out after 5 seconds if still on deathCam
        yield return new WaitForSeconds(1);
        if (activeCam == 1)
        {
            deathScreen.color = new Color(0, 0, 0, 100);
        }
    }
    void OnApplicationQuit()
    {
        if (myPort.IsOpen)
            myPort.Close();
    }
}
