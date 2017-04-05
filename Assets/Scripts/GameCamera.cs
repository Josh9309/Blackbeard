using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCamera : MonoBehaviour
{
    #region Attributes
    //Camera
    [SerializeField] private Transform target;
    private Vector3 lastPosition;
    private float cameraRotationSpeed;
    private float targetHeight;
    private float smoothFollow = 2;
    private Camera cam;

    //Parrot
    private int parrotMinAngle = 300;
    private int parrotMaxAngle = 35;

    //Pirate
    private int pirateMinAngle = 300;
    private int pirateMaxAngle = 30;

    //Input attributes
    private float deadZone = 0.1f;
    #endregion

    #region InBuiltMethods
    void Start() //Use this for initialization
    {
        cam = gameObject.GetComponentInChildren<Camera>(); //Get the camera
        targetHeight = target.GetComponent<Collider>().bounds.size.y;
        cameraRotationSpeed = 70;
    }

    void LateUpdate() //LateUpdate occurs after all other updates
    {
        transform.position = new Vector3(target.position.x, target.position.y + targetHeight, target.position.z); //Move the camera gameobject into position

        //Get camera input
        float camXInput = 0;
        float camYInput = 0;
        float movementXInput = 0;
        float movementYInput = 0;

        if (name.Contains("1")) //Player 1 input
        {
            camXInput = Input.GetAxis(GameManager.Instance.P1Input.CAM_HORIZONTAL_AXIS);
            camYInput = Input.GetAxis(GameManager.Instance.P1Input.CAM_VERTICAL_AXIS);
            movementXInput = Input.GetAxis(GameManager.Instance.P1Input.HORIZONTAL_AXIS);
            movementYInput = Input.GetAxis(GameManager.Instance.P1Input.VERTICAL_AXIS);
        }
        else if (name.Contains("2")) //Player 2 input
        {
            camXInput = Input.GetAxis(GameManager.Instance.P2Input.CAM_HORIZONTAL_AXIS);
            camYInput = Input.GetAxis(GameManager.Instance.P2Input.CAM_VERTICAL_AXIS);
            movementXInput = Input.GetAxis(GameManager.Instance.P2Input.HORIZONTAL_AXIS);
            movementYInput = Input.GetAxis(GameManager.Instance.P2Input.VERTICAL_AXIS);
        }

        //cam.transform.position = new Vector3(target.transform.position.x, target.transform.position.y + 2, target.transform.position.z - 3); //Adjust the camera

        //Right stick camera movement and reset
        if (Mathf.Abs(camXInput) > deadZone || Mathf.Abs(camYInput) > deadZone)
            ManualCamera(camXInput, camYInput);
        else if (target.transform.position != lastPosition)
            AutoCamera(movementXInput, movementYInput);

        lastPosition = target.transform.position;
    }
    #endregion

    #region Methods
    /// <summary>
    /// Look at the target
    /// </summary>
    void AutoCamera(float movementX, float movementY)
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(Vector3.zero), Time.deltaTime * smoothFollow); //Reset the rotation of the camera

        Vector3 cameraTargetPosition = target.position + (target.up * 2) - (target.forward * 3); //The target position for the camera
        cam.transform.position = Vector3.Slerp(cam.transform.position, cameraTargetPosition, Time.deltaTime * smoothFollow); //Move the camera into position

        cam.transform.LookAt(new Vector3(target.position.x, target.position.y + targetHeight, target.position.z)); //Make the camera look at the target
    }

    ///// <summary>
    ///// Look at the target
    ///// </summary>
    //void AutoCamera()
    //{
    //    //if (Vector3.Dot(cam.transform.forward, target.forward) < 0)
    //    //{ }
    //    //else
    //        cam.transform.position = new Vector3(target.transform.position.x, target.transform.position.y + 2, target.transform.position.z - 3); //Adjust the camera
    //
    //    //Vector2 camV2 = new Vector2(cam.transform.position.x, cam.transform.position.z);
    //    //Vector2 targetV2 = new Vector2(target.transform.position.x, target.transform.position.z);
    //    //if ((camV2 - targetV2).magnitude > 5.1)
    //    //{
    //    //    Vector3 forward = new Vector3(cam.transform.forward.x, 0, cam.transform.forward.z);
    //    //    cam.transform.position = Vector3.Slerp(cam.transform.position, cam.transform.position + (forward.normalized * 0.2f), Time.deltaTime);
    //    //}
    //    //else if ((camV2 - targetV2).magnitude < 4.9)
    //    //{
    //    //    Vector3 backward = new Vector3(-cam.transform.forward.x, 0, -cam.transform.forward.z);
    //    //    cam.transform.position = Vector3.Slerp(cam.transform.position, cam.transform.position + (backward.normalized * 0.2f), Time.deltaTime);
    //    //}
    //
    //    //Vector2 camVector2 = new Vector2(cam.transform.position.x, cam.transform.position.z);
    //    //Vector2 targetVector2 = new Vector2(target.transform.position.x, target.transform.position.z);
    //    //Debug.Log((camVector2 - targetVector2).magnitude);
    //    //if ((camVector2 - targetVector2).magnitude < 1)
    //    //    cam.transform.rotation = Quaternion.Slerp(cam.transform.rotation, Quaternion.Euler(Vector3.zero), Time.deltaTime);
    //    //else if ((camVector2 - targetVector2).magnitude > 5.1)
    //    //    cam.transform.position = Vector3.Slerp(new Vector3(target.position.x, target.position.y + targetHeight, target.position.z), new Vector3(cam.transform.position.x, target.position.y + targetHeight, cam.transform.position.z), Time.deltaTime);
    //
    //    cam.transform.LookAt(new Vector3(target.position.x, target.position.y + targetHeight, target.position.z)); //Make the camera look at the target
    //}

    /// <summary>
    /// Let the player move the camera with the right stick
    /// </summary>
    void ManualCamera(float camX, float camY)
    {
        float xRotation = 0, yRotation = 0;
        
        //Horizontal camera rotation
        if (Mathf.Abs(camX) > deadZone)
            yRotation = cameraRotationSpeed * -Mathf.Sign(camX) * Time.deltaTime;
        
        //Verical camera movement
        if (Mathf.Abs(camY) > deadZone)
            xRotation = cameraRotationSpeed * -Mathf.Sign(camY) * Time.deltaTime;

        //Rotate the camera's gameobject
        transform.Rotate(xRotation, yRotation, 0);

        //Unrotate the camera's Z rotations
        float zRotation = -transform.eulerAngles.z;
        transform.Rotate(0, 0, zRotation);


        //if (target.name.Contains("Parrot"))
        //{
        //    //Clamp vertical space and set z rotation to 0
        //    if (transform.eulerAngles.x < 180)
        //        transform.rotation = Quaternion.Euler(Mathf.Clamp(transform.eulerAngles.x, 0, parrotMaxAngle), transform.eulerAngles.y, 0);
        //    else
        //        transform.rotation = Quaternion.Euler(Mathf.Clamp(transform.eulerAngles.x, parrotMinAngle, 360), transform.eulerAngles.y, 0);
        //}
        //else if (target.name.Contains("Pirate"))
        //{
        //    //Clamp vertical space and set z rotation to 0
        //    if (transform.eulerAngles.x < 180)
        //        transform.rotation = Quaternion.Euler(Mathf.Clamp(transform.eulerAngles.x, 0, pirateMaxAngle), transform.eulerAngles.y, 0);
        //    else
        //        transform.rotation = Quaternion.Euler(Mathf.Clamp(transform.eulerAngles.x, pirateMinAngle, 360), transform.eulerAngles.y, 0);
        //}
    }
    #endregion

    //#region Attributes
    ////Camera
    //[SerializeField]
    //private Transform target;
    //private Transform cameraPosition;
    //private float targetHeight;
    //private Vector3 cameraDistance;
    //private Camera cam;
    //private Vector3 cameraStartPosition;
    //private float smoothFollow = 2;
    //private float rotationSpeed = 100;

    ////Parrot
    //private int parrotMinAngle = 300;
    //private int parrotMaxAngle = 35;

    ////Pirate
    //private int pirateMinAngle = 330;
    //private int pirateMaxAngle = 10;

    ////Input attributes
    //private float deadZone = 0.1f;
    //#endregion

    //#region Properties
    ////public Transform Target
    ////{
    ////    set
    ////    {
    ////        target = value;
    ////    }
    ////}
    //#endregion

    //#region InBuiltMethods
    //void Start() //Use this for initialization
    //{
    //    cam = gameObject.GetComponent<Camera>(); //Get the camera
    //    cameraPosition = target.FindChild("CameraLookAt");
    //    targetHeight = target.GetComponent<Collider>().bounds.size.y / 2;
    //    cameraDistance = cameraPosition.position;
    //    //cameraStartPosition = new Vector3(0, target.FindChild("CameraLookAt").position.y, -2); //Set the starting position for the pirate camera
    //}

    //void LateUpdate() //LateUpdate occurs after all other updates
    //{
    //    float camXInput = 0;
    //    float camYInput = 0;

    //    if (name.Contains("1")) //Player 1 input
    //    {
    //        camXInput = Input.GetAxis(GameManager.Instance.P1Input.CAM_HORIZONTAL_AXIS);
    //        camYInput = Input.GetAxis(GameManager.Instance.P1Input.CAM_VERTICAL_AXIS);
    //    }
    //    else if (name.Contains("2")) //Player 2 input
    //    {
    //        camXInput = Input.GetAxis(GameManager.Instance.P2Input.CAM_HORIZONTAL_AXIS);
    //        camYInput = Input.GetAxis(GameManager.Instance.P2Input.CAM_VERTICAL_AXIS);
    //    }

    //    cam.transform.position = cameraPosition.transform.position; //Adjust the camera

    //    //Right stick camera movement and reset
    //    if (Mathf.Abs(camXInput) > deadZone || Mathf.Abs(camYInput) > deadZone)
    //        ManualCamera(camXInput, camYInput);
    //    else
    //        AutoCamera();
    //}
    //#endregion

    //#region Methods
    ///// <summary>
    ///// Look at the target
    ///// </summary>
    //void AutoCamera()
    //{
    //    Vector3 cameraTargetPosition = target.position + (target.up * cameraDistance.y) - (target.forward * cameraDistance.z); //The target position for the camera

    //    Debug.DrawRay(cameraDistance, Vector3.up * cameraDistance.y, Color.red);
    //    Debug.DrawRay(cameraDistance, -(target.forward * cameraDistance.z), Color.blue);
    //    Debug.DrawRay(cameraDistance, cameraTargetPosition, Color.magenta);

    //    cam.transform.position = Vector3.Slerp(cam.transform.position, cameraTargetPosition, Time.deltaTime * smoothFollow); //Move the camera into position

    //    cam.transform.LookAt(new Vector3(target.position.x, target.position.y + targetHeight, target.position.z)); //Make the camera look at the target

    //    //cam.transform.rotation = Quaternion.Slerp(cam.transform.rotation, Quaternion.Euler(Vector3.zero), Time.deltaTime * smoothFollow); //Rotate the camera
    //}

    ///// <summary>
    ///// Let the player move the camera with the right stick
    ///// </summary>
    //void ManualCamera(float camX, float camY)
    //{
    //    float xRotation = 0, yRotation = 0;

    //    //Horizontal camera rotation
    //    if (Mathf.Abs(camX) > deadZone)
    //        yRotation = 100 * -Mathf.Sign(camX) * Time.deltaTime;

    //    //Verical camera movement
    //    if (Mathf.Abs(camY) > deadZone)
    //        xRotation = 100 * Mathf.Sign(camY) * Time.deltaTime;

    //    cam.transform.Rotate(xRotation, yRotation, 0);

    //    if (target.name.Contains("Parrot"))
    //    {
    //        //Clamp vertical space and set z rotation to 0
    //        if (transform.eulerAngles.x < 180)
    //            cam.transform.rotation = Quaternion.Euler(Mathf.Clamp(transform.eulerAngles.x, 0, parrotMaxAngle), transform.eulerAngles.y, 0);
    //        else
    //            cam.transform.rotation = Quaternion.Euler(Mathf.Clamp(transform.eulerAngles.x, parrotMinAngle, 360), transform.eulerAngles.y, 0);
    //    }
    //    else if (target.name.Contains("Pirate"))
    //    {
    //        //Clamp vertical space and set z rotation to 0
    //        if (transform.eulerAngles.x < 180)
    //            cam.transform.rotation = Quaternion.Euler(Mathf.Clamp(transform.eulerAngles.x, 0, pirateMaxAngle), transform.eulerAngles.y, 0);
    //        else
    //            cam.transform.rotation = Quaternion.Euler(Mathf.Clamp(transform.eulerAngles.x, pirateMinAngle, 360), transform.eulerAngles.y, 0);
    //    }
    //}
    //#endregion
}