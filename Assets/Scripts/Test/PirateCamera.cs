using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PirateCamera : MonoBehaviour
{
    #region Attributes
    [SerializeField] private Transform target;
    [SerializeField] private float smoothFollow = .15f;
    [SerializeField] private Vector3 distanceFromTarget = new Vector3(0, 0, 0);
    [SerializeField] private float tilt = 10; //Angle of the camera looking at the target
    private float rotationSpeed = 0;
    private Vector3 goalPosition; //The position the camera would like to move to
    TestPiratePlayer targetScript; //To get the rotation of the target
    #endregion

    #region Properties
    public Transform Target
    {
        set
        {
            target = value;
        }
    }
    #endregion

    
    void Start() //Use this for initialization
    {
        goalPosition = Vector3.zero;
        transform.Rotate(tilt, transform.rotation.y, transform.rotation.z);
        targetScript = target.GetComponent<TestPiratePlayer>(); //Get the target script from the target
    }

    void LateUpdate() //LateUpdate occurs after all other updates
    {
        FollowTarget();
        LookAtTarget();
    }

    /// <summary>
    /// Calculate the point behind the target from which to follow them
    /// </summary>
    void FollowTarget()
    {
        goalPosition = targetScript.TargetRotation * distanceFromTarget; //Rotate the goal position
        goalPosition += target.position; //Make the rotated goal position relative to the target
        transform.position = goalPosition; //Set the position of the camera
    }

    /// <summary>
    /// Look at the target
    /// </summary>
    void LookAtTarget()
    {
        float eulerYAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, target.eulerAngles.y, ref rotationSpeed, smoothFollow); //Smooth lerping for the camera
        transform.rotation = Quaternion.Euler(transform.eulerAngles.x, eulerYAngle, 0); //Pass in the smooth rotation component to the camera's rotation
    }
}
