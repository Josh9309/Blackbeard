using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PirateCamera : MonoBehaviour
{
    #region Attributes
    //Camera
    private Transform target;
    [SerializeField] private float smoothFollow = .15f;
    //[SerializeField] private Vector3 distanceFromTarget = new Vector3(0, 0, 0);
    //[SerializeField] private float tilt = 10; //Angle of the camera looking at the target
    private float rotationSpeed = 0;
    private Vector3 goalPosition; //The position the camera would like to move to
    //TestPiratePlayer targetScript; //To get the rotation of the target

    //Input attributes
    private float deadZone = 0.1f;
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
        target = FindObjectOfType<Parrot>().gameObject.transform; //Target the parrot from the start
        //transform.Rotate(tilt, transform.rotation.y, transform.rotation.z);
        //targetScript = target.GetComponent<TestPiratePlayer>(); //Get the target script from the target
    }

    void LateUpdate() //LateUpdate occurs after all other updates
    {
        float camXInput = Input.GetAxis("Cam_Horizontal");
        //float camYInput = Input.GetAxis("Cam_Vertical");

        transform.position = target.transform.position; //Set the position of the camera

        if (Mathf.Abs(camXInput) > deadZone)
            ManualCamera(camXInput);
        else
        {
            //FollowTarget();
            LookAtTarget();
        }
    }

    #region Methods
    ///// <summary>
    ///// Calculate the point behind the target from which to follow them
    ///// </summary>
    //void FollowTarget()
    //{
    //    goalPosition = targetScript.TargetRotation * Vector3.zero; //Rotate the goal position
    //    goalPosition += target.position; //Make the rotated goal position relative to the target
    //    transform.position = goalPosition; //Set the position of the camera
    //}

    /// <summary>
    /// Look at the target
    /// </summary>
    void LookAtTarget()
    {
        float eulerYAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, target.eulerAngles.y, ref rotationSpeed, smoothFollow); //Smooth lerping for the camera
        transform.rotation = Quaternion.Euler(transform.eulerAngles.x, eulerYAngle, 0); //Pass in the smooth rotation component to the camera's rotation
    }

    /// <summary>
    /// Let the player move the camera with the right stick
    /// </summary>
    void ManualCamera(float camX)
    {
        //transform.position = target.transform.position; //Center the position of the camera

        //if (Mathf.Abs(camX) > deadZone)
        //{
            //float eulerYAngle = Mathf.SmoothDampAngle(transform.eulerAngles.x, target.eulerAngles.x, ref rotationSpeed, smoothFollow); //Smooth lerping for the camera
            transform.Rotate(0, 100 * Mathf.Sign(camX) * Time.deltaTime, 0); //Pass in the smooth rotation component to the camera's rotation
            //transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);
            //transform.Rotate(0, 1 * Mathf.Sign(camX), 0);
        //}
        //else if (Mathf.Abs(camY) > deadZone)
        //{
        //    transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, 0, 0);
        //    transform.Rotate(1 * Mathf.Sign(camY), 0, 0);
        //}
    }
    #endregion
}