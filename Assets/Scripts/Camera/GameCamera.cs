using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCamera : MonoBehaviour
{
    #region Attributes
    //Camera
    private Transform target;
    private GameObject cam;
    private Vector3 cameraTargetPosition;
    private Vector3 parrotCameraStartPosition;
    private Vector3 pirateCameraStartPosition;
    private float smoothFollow = 2;
    private float rotationSpeed = 100;

    //Parrot
    private int parrotMinAngle = 300;
    private int parrotMaxAngle = 35;

    //Pirate
    private int pirateMinAngle = 330;
    private int pirateMaxAngle = 10;

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

    #region InBuiltMethods
    void Start() //Use this for initialization
    {
        target = GameObject.FindGameObjectWithTag("ParrotCam").gameObject.transform; //Target the parrot from the start
        //reset = false; //Camera is not resetting

        cam = GameObject.FindGameObjectWithTag("MainCamera"); //Get the camera
        parrotCameraStartPosition = cam.transform.position; //Get the starting position of the camera
        pirateCameraStartPosition = new Vector3(0, 3, -5); //Set the starting position for the pirate camera
    }

    void LateUpdate() //LateUpdate occurs after all other updates
    {
        float camXInput = Input.GetAxis("Cam_Horizontal");
        float camYInput = Input.GetAxis("Cam_Vertical");

        transform.position = target.transform.position; //Adjust the camera

        //Right stick camera movement and reset
        if (Mathf.Abs(camXInput) > deadZone || Mathf.Abs(camYInput) > deadZone)
            ManualCamera(camXInput, camYInput);
        else
            AutoCamera();
    }
    #endregion

    #region Methods
    /// <summary>
    /// Look at the starget
    /// </summary>
    void AutoCamera()
    {
        if (target.tag == "ParrotCam")
            cameraTargetPosition = target.position + (target.up * parrotCameraStartPosition.y) + (target.forward * parrotCameraStartPosition.z); //The position of the camera behind the parrot
        else if (target.tag == "PirateCam")
            cameraTargetPosition = target.position + (target.up * pirateCameraStartPosition.y) + (target.forward * pirateCameraStartPosition.z); //The position of the camera behind the pirate

        cam.transform.position = Vector3.Slerp(cam.transform.position, cameraTargetPosition, Time.deltaTime * smoothFollow); //Move the camera into position

        cam.transform.LookAt(target); //Make the camera look at the target

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(Vector3.zero), Time.deltaTime * smoothFollow);
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

        if (target.tag == "ParrotCam")
        {
            //Clamp vertical space and set z rotation to 0
            if (transform.eulerAngles.x < 180)
                transform.rotation = Quaternion.Euler(Mathf.Clamp(transform.eulerAngles.x, 0, parrotMaxAngle), transform.eulerAngles.y, 0);
            else
                transform.rotation = Quaternion.Euler(Mathf.Clamp(transform.eulerAngles.x, parrotMinAngle, 360), transform.eulerAngles.y, 0);
        }
        else if (target.tag == "PirateCam")
        {
            //Clamp vertical space and set z rotation to 0
            if (transform.eulerAngles.x < 180)
                transform.rotation = Quaternion.Euler(Mathf.Clamp(transform.eulerAngles.x, 0, pirateMaxAngle), transform.eulerAngles.y, 0);
            else
                transform.rotation = Quaternion.Euler(Mathf.Clamp(transform.eulerAngles.x, pirateMinAngle, 360), transform.eulerAngles.y, 0);
        }
    }
    #endregion
}