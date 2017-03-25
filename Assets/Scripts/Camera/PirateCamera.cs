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
    private float rotationSpeed = 100;
    private bool reset;

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
        target = FindObjectOfType<Parrot>().gameObject.transform; //Target the parrot from the start
        reset = false; //Camera is not resetting

        //Get and assign all cameras
        Camera []camList = GetComponentsInChildren<Camera>();     
        foreach (Camera c in camList)
        {
            if (c.name.Contains("Parrot"))
                parrotCam = c;
            else if (c.name.Contains("Pirate"))
                pirateCam = c;
        }
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

        //Right stick camera movement and reset
        if (Mathf.Abs(camXInput) > deadZone || Mathf.Abs(camYInput) > deadZone)
            ManualCamera(camXInput, camYInput);
        else if (Input.GetButton("R3"))
            reset = true;

        ResetCamera(reset);
    }
    #endregion

    #region Methods
    /// <summary>
    /// Look at the starget
    /// </summary>
    void ResetCamera(bool buttonPressed)
    {
        //If the camera has to be reset and isn't yet in the correct position
        if (buttonPressed && (Mathf.Abs(transform.eulerAngles.y) - Mathf.Abs(target.eulerAngles.y) >= 1 || transform.eulerAngles.x != target.eulerAngles.x))
        {
            float eulerYAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, target.eulerAngles.y, ref rotationSpeed, smoothFollow); //Smooth lerping for the camera

            transform.rotation = Quaternion.Euler(target.eulerAngles.x, eulerYAngle, 0); //Pass in the smooth rotation component to the camera's rotation
        }
        else
            reset = false;
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

        if (parrotCam.enabled)
        {
            //Clamp vertical space and set z rotation to 0
            if (transform.eulerAngles.x < 180)
                transform.rotation = Quaternion.Euler(Mathf.Clamp(transform.eulerAngles.x, 0, parrotMaxAngle), transform.eulerAngles.y, 0);
            else
                transform.rotation = Quaternion.Euler(Mathf.Clamp(transform.eulerAngles.x, parrotMinAngle, 360), transform.eulerAngles.y, 0);
        }
        else if (pirateCam.enabled)
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