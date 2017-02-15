using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParrotCopter : MonoBehaviour
{
    #region Attributes
    //Parrot
    private int health = 10;
    [SerializeField] private float speed = 2.0f;
    [SerializeField] private float maxSpeed = 5.0f;
    private float savedMaxSpeed;
    [SerializeField] private float turnSpeed = 5.0f;
    [SerializeField] private float minHeight = 0;
    [SerializeField] private float maxHeight = 15;
    private float currentHeight;    
    private Rigidbody rBody;
    private float flapCycleTime;

    //Input
    private float deadZone = .1f;

    //input stuff
    //private float inputDelay = 0.3f;
    //private float horizontalInput = 0;
    //private float verticalInput = 0;
    //private float camXInput = 0;
    //private float camYInput = 0;
    #endregion

    #region Properties
    #endregion

    void Start() //Use this for initialization
    {
        rBody = GetComponent<Rigidbody>();
        savedMaxSpeed = maxSpeed;
	}
	
	void Update() //Update is called once per frame
    {
        ParrotControl();
    }

    private void FixedUpdate() //Physics updates
    {
        ParrotRigidbodyControl();
    }

    /// <summary>
    /// This method controls parrot movement through the rigidbody, and should be called in FixedUpdate
    /// </summary>
    private void ParrotRigidbodyControl()
    {
        //Make the parrot fly forward and backwards
        if (Input.GetAxis("Vertical") > deadZone)
        {
            rBody.velocity += transform.forward * (speed / 2);
            rBody.velocity = Vector3.ClampMagnitude(rBody.velocity, maxSpeed);
        }
        else if (Input.GetAxis("Vertical") < -deadZone)
        {
            rBody.velocity = -transform.forward;
        }
        else
        {
            rBody.velocity = Vector3.zero;
        }

        //Make the parrot fly upwards
        if (Input.GetAxis("FlyUp") > deadZone && transform.position.y < maxHeight)
        {
            rBody.velocity += transform.up * Mathf.Lerp(maxSpeed, speed, flapCycleTime);

            flapCycleTime += 2.5f * Time.deltaTime;

            if (flapCycleTime > 1f)
            {
                flapCycleTime = 0f;
            }
        }

        //Make the parrot fly downwards
        if (Input.GetAxis("FlyDown") > deadZone && transform.position.y > minHeight)
        {
            rBody.velocity -= transform.up * Mathf.Lerp(maxSpeed, speed, flapCycleTime);

            flapCycleTime += 2.5f * Time.deltaTime;

            if (flapCycleTime > 1f)
            {
                flapCycleTime = 0f;
            }
        }
    }

    /// <summary>
    /// This method controls parrot movement without the rigidbody, an should be called in Update
    /// </summary>
    private void ParrotControl()
    {
        //Make the parrot bank left and right
        if (Input.GetAxis("Horizontal") > deadZone)
        {
            transform.Rotate(0, turnSpeed, 0);
        }
        else if (Input.GetAxis("Horizontal") < -deadZone)
        {
            transform.Rotate(0, -turnSpeed, 0);
        }
        else
        {
            transform.Rotate(new Vector3(0, 0, 0));
        }

        //BOOST
        if (Input.GetButton("BoostFly"))
        {
            maxSpeed = savedMaxSpeed * 2;
        }
        else
        {
            maxSpeed = savedMaxSpeed;
        }
    }
}