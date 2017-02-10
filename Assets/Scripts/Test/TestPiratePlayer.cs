using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPiratePlayer : MonoBehaviour
{
    #region Attributes
    //Pirate stats
    private int health = 25;
    private int attackPower;

    //Pirate movement
    private float movementSpeed = 5.0f;
    private float rotationSpeed = 100;
    private Rigidbody rBody;
    private Quaternion targetRotation; //Next rotation to turn to

    //Input attributes
    private float deadZone = 0.1f;
    #endregion

    #region Properties
    public Quaternion TargetRotation
    {
        get
        {
            return targetRotation;
        }
    }

    public Rigidbody RBody
    {
        get
        {
            return rBody;
        }
        set
        {
            rBody = value;
        }
    }
    #endregion

    void Start() //Use this for initialization
    {
        rBody = GetComponent<Rigidbody>();
        targetRotation = transform.rotation; //Set the target rotation as the initial rotation of this object
    }

    void Update() //Update is called once per frame
    {
        PirateTurn();
    }

    void FixedUpdate() //Physics updates
    {
        PirateMove();
    }

    /// <summary>
    /// Move the pirate through the rigidbody
    /// </summary>
    private void PirateMove()
    {
        float movementInput = Input.GetAxis("Vertical"); //Get the movement input from the vertical axis

        if (Mathf.Abs(movementInput) > deadZone)
            rBody.velocity = transform.forward * movementInput * movementSpeed; //MovementInput determines direction
        else
            rBody.velocity = Vector3.zero;
    }

    #region Methods
    /// <summary>
    /// Turn the pirate
    /// </summary>
    private void PirateTurn()
    {
        float rotationInput = Input.GetAxis("Horizontal"); //Get the rotation input from the horizontal axis

        if (Mathf.Abs(rotationInput) > deadZone)
        {
            targetRotation *= Quaternion.AngleAxis(rotationSpeed * rotationInput * Time.deltaTime, Vector3.up); //RotationInput determines direction
            transform.rotation = targetRotation; //Rotate the object
        }
    }
    #endregion
}