using UnityEngine;
using System.Collections;
using UnityEngine.VR;

/// <summary>
/// Controls the way that a camera is positioned during runtime.
/// </summary>
public class CameraController : MonoBehaviour
{
    /// <summary>
    /// The target represents the way that a camera should be positioned.
    /// </summary>
    [SerializeField]
    private Transform target;

    /// <summary>
    /// offsetPosition represents the offset position of the camera.
    /// </summary>
    [SerializeField]
    private Vector3 offsetPosition;

    /// <summary>
    /// offsetPositionSpace represents the starting position of a camera.
    /// </summary>
    [SerializeField]
    private Space offsetPositionSpace = Space.Self;

    /// <summary>
    /// lookAt property determines whether the camera should follow the target or follow the target's orientation.
    /// </summary>
    [SerializeField]
    public bool lookAt;


    /// <summary>
    /// Update calls whenever the scene changes and reorients the camera accordingly.
    /// </summary>
    private void Update()
    {
        Refresh();
    }

    /// <summary>
    /// Refresh reorients the camera whenever a change in position needs to be made.
    /// </summary>
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
            UnityEngine.XR.XRSettings.enabled = !UnityEngine.XR.XRSettings.enabled;
            Debug.Log("Changed VRSettings.enabled to:" + UnityEngine.XR.XRSettings.enabled);
        }
        // Computes rotation
        if (lookAt)
        {
            transform.LookAt(target);
        }
        //Look around using VR Device
        else if (UnityEngine.XR.XRSettings.enabled)
        {
            float VRx = UnityEngine.XR.InputTracking.GetLocalRotation(UnityEngine.XR.XRNode.Head).eulerAngles.x;
            float VRy = UnityEngine.XR.InputTracking.GetLocalRotation(UnityEngine.XR.XRNode.Head).eulerAngles.y;
            float VRz = UnityEngine.XR.InputTracking.GetLocalRotation(UnityEngine.XR.XRNode.Head).eulerAngles.z;
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