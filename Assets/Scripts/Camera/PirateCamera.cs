using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PirateCamera : MonoBehaviour
{
    #region Attributes
    //Camera
    private Transform target;
    private Camera parrotCam, pirateCam;
    [SerializeField] private float smoothFollow = .15f;
    //[SerializeField] private Vector3 distanceFromTarget = new Vector3(0, 0, 0);
    //[SerializeField] private float tilt = 10; //Angle of the camera looking at the target
    private float rotationSpeed = 0;
    //private Vector3 goalPosition; //The position the camera would like to move to
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

    #region InBuildMethods
    void Start() //Use this for initialization
    {
        //goalPosition = Vector3.zero;
        target = FindObjectOfType<Parrot>().gameObject.transform; //Target the parrot from the start

        //Get and assign all cameras
        Camera []camList = GetComponentsInChildren<Camera>();     
        foreach (Camera c in camList)
        {
            if (c.name.Contains("Parrot"))
                parrotCam = c;
            else if (c.name.Contains("Pirate"))
                pirateCam = c;
        }

        //transform.Rotate(tilt, transform.rotation.y, transform.rotation.z);
        //targetScript = target.GetComponent<TestPiratePlayer>(); //Get the target script from the target
    }

    void LateUpdate() //LateUpdate occurs after all other updates
    {
        //Choose a camera based on the target object
        if (target.gameObject.tag == "Parrot")
        {
            pirateCam.enabled = false;
            parrotCam.enabled = true;
        }
        else if (target.gameObject.tag == "Pirate")
        {
            parrotCam.enabled = false;
            pirateCam.enabled = true;
        }

        float camXInput = Input.GetAxis("Cam_Horizontal");
        float camYInput = Input.GetAxis("Cam_Vertical");

        transform.position = target.transform.position; //Adjust the camera

        if (Mathf.Abs(camXInput) > deadZone || Mathf.Abs(camYInput) > deadZone)
            ManualCamera(camXInput, camYInput);
        //else if (target.rBody.velocity > 3)
        //{
        //    //FollowTarget();
        //    LookAtTarget();
        //}
    }
    #endregion

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
    void ManualCamera(float camX, float camY)
    {
        float xRotation = 0, yRotation = 0;

        //Horizontal camera rotation
        if (Mathf.Abs(camX) > deadZone)
            yRotation = 100 * -Mathf.Sign(camX) * Time.deltaTime;

        //Verical camera movement
        if (Mathf.Abs(camY) > deadZone)
            xRotation = 100 * Mathf.Sign(camY) * Time.deltaTime;

        transform.Rotate(xRotation, yRotation, 0);

        //Clamp vertical space and set z rotation to 0
        if (transform.eulerAngles.x < 180)
            transform.rotation = Quaternion.Euler(Mathf.Clamp(transform.eulerAngles.x, 0, 20), transform.eulerAngles.y, 0);
        else
            transform.rotation = Quaternion.Euler(Mathf.Clamp(transform.eulerAngles.x, 340, 360), transform.eulerAngles.y, 0);
    }
    #endregion
}