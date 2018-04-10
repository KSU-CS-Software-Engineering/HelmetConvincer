using UnityEngine;
using System.Collections;

/// <summary>Determines how the player interacts with the scene.</summary>
public class UserController : MonoBehaviour
{
    public float TurnSpeed;
	/// <summary>The TurnSpeed property determines the player's turn speed.</summary>
    /// <value>The TurnSpeed property represents the value by which the players rotation is scaled.</value>
	
    public float MoveSpeed;
	/// <summary>The MoveSpeed property determines the player's move speed.</summary>
    /// <value>The MoveSpeed property represents the value by which the players movement is scaled.</value>
	
//    public float torque;

    private Rigidbody rb;
    /// <summary>The rb property represents the player.</summary>
    /// <value>The rb property represents the player object.</value>

    public Transform rider;

    private Vector3 riderOffsetPosition;

    public int Map;

    public Vector3[] MapArray;

    float distToGround;

    public LayerMask playerLayer = (1 << 8);
    


    /// <summary>Start is a method called at the scene's start.</summary>
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        riderOffsetPosition = transform.position + rider.transform.position;
        Respawn();
        //playerLayer = ~playerLayer;
        distToGround = GetComponent<Collider>().bounds.extents.y;
    }
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
    /// <summary>FixedUpdate is a method called when the scene updates.</summary>
    void FixedUpdate()
    {
        // Gets the value of the control axes
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        transform.Rotate(Vector3.up, moveHorizontal * TurnSpeed * Time.deltaTime);
        //transform.Rotate(new Vector3(-1, 0, 0));

        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up, -Vector3.up, out hit, playerLayer))
        {
            //print("Found an object - distance: " + hit.distance);
            if(hit.distance < 1)
                hit.distance = 1;
            transform.Translate(0, 0, moveVertical * (MoveSpeed/hit.distance) * Time.deltaTime);
            //print("Found an object - distance: " + hit.distance);
        }//        rb.AddTorque(transform.up * 10);

        // Actions performed when the spacebar is pressed
        if (Input.GetKeyDown("space"))
        {
            Respawn();
        }
    }
    
}