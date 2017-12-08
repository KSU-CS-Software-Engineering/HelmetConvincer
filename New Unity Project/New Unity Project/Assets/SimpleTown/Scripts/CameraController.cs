using UnityEngine;
using System.Collections;
using UnityEngine.VR;

/// <summary>Controls how a given camera is orientated</summary>
public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Transform target;
	/// <summary>The target property represents the camera's target for orientation.</summary>
    /// <value>The target property represents the value of the target object.</value>
	
    [SerializeField]
    private Vector3 offsetPosition;
	/// <summary>The offsetPosition property represents the camera's offest position.</summary>
    /// <value>The offsetPosition property represents the camera's offest position in relation to the target.</value>
	
    [SerializeField]
    private Space offsetPositionSpace = Space.Self;
	/// <summary>The offsetPositionSpace property represents the camera's starting position.</summary>
    /// <value>The offsetPositionSpace property represents the camera's starting position in relation to the target.</value>
	
    [SerializeField]
    public bool lookAt;
	/// <summary>The lookAt property tells how the camera should focus.</summary>
    /// <value>The lookAt property determines if the camera looks at the target or follows its orientation.</value>
	
	
	/// <summary>Update is a method called when the scene updates.</summary>
    private void Update()
    {
        Refresh();
    }

	/// <summary>Refresh is a method called to refresh variable values.</summary>
    public void Refresh()
    {
        if (target == null)
        {
            Debug.LogWarning("Missing target ref !", this);

            return;
        }

        // Computes position
        //transform.position = target.position + offsetPosition;

        if (lookAt)
        {
            transform.position = target.position + offsetPosition;
        }
        else
        {
            transform.position = target.TransformPoint(offsetPosition);
        }
        //If V is pressed, toggle VRSettings.enabled
        if (Input.GetKeyDown(KeyCode.V))
        {
            VRSettings.enabled = !VRSettings.enabled;
            Debug.Log("Changed VRSettings.enabled to:" + VRSettings.enabled);
        }
        // Computes rotation
        if (lookAt)
        {
            transform.LookAt(target);
        }
        //Look around using VR Device
        else if(VRSettings.enabled)
        {
            float VRx = InputTracking.GetLocalRotation(VRNode.Head).eulerAngles.x;
            float VRy = InputTracking.GetLocalRotation(VRNode.Head).eulerAngles.y;
            float VRz = InputTracking.GetLocalRotation(VRNode.Head).eulerAngles.z;
            transform.Rotate(Vector3.right, VRx);
            transform.Rotate(Vector3.up, VRy);
            transform.Rotate(Vector3.forward, VRz);
        }
        //Look around using mouse
        else
        {
            //float moveHorizontal = Input.GetAxis("Horizontal");
            //transform.Rotate(Vector3.up, moveHorizontal * 40 * Time.deltaTime);

			///Rotate based on mouse movement
            float mouseHorizontal = Input.GetAxis("Mouse X");
            float mouseVertical = Input.GetAxis("Mouse Y");
            transform.Rotate(Vector3.up, mouseHorizontal);
            transform.Rotate(Vector3.right, -mouseVertical);
        }
    }
}