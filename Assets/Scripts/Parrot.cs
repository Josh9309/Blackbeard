using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parrot : MonoBehaviour {
    #region Attributes
    private int health = 10;
    [SerializeField] private float speed = 2.0f;
    [SerializeField] private float turnSpeed = 2.0f;
    private float currentHeight;
    
    private Rigidbody rBody;
    private bool rotateParrot = false;
    Vector3 parrotRotation = new Vector3(); //parrot euler angle rotation
    //input stuff
    private float inputDelay = 0.3f;
    private float horizontalInput = 0;
    private float verticalInput = 0;
    private float flyUpInput = 0;
    private float flyDownInput = 0;
    #endregion

    #region Properties
    #endregion

    // Use this for initialization
    void Start () {
        rBody = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        ParrotMove();

        //parrot Rotation
        //if (rotateParrot) //if parrot needs to be rotated
        //{
        //    if(Vector3.Distance(transform.localRotation.eulerAngles, parrotRotation) > 0.01f)
        //    {
        //        transform.eulerAngles = Vector3.Lerp(transform.localRotation.eulerAngles, parrotRotation, Time.deltaTime);
        //    }
        //    else
        //    {
        //        transform.eulerAngles = parrotRotation;
        //        rotateParrot = false;
        //    }
        //}
	}

    #region Methods
    private void ParrotMove()
    {

        //Get inputs for Parrot movement
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        
        flyUpInput = Input.GetAxis("FlyUp");
        flyDownInput = Input.GetAxis("FlyDown");

        //zero velocity
        rBody.velocity = Vector3.zero;

        //zero parrot rotation
        parrotRotation = Vector3.zero;

        
        
        //Parrot fly up
        if(Mathf.Abs(flyUpInput) > inputDelay)
        {
            rBody.velocity += new Vector3(0, speed, 0);
            parrotRotation += new Vector3(-15, 0, 0);
            rotateParrot = true;
        }
        else if (Mathf.Abs(flyDownInput) > inputDelay) //parrot fly down
        {
            rBody.velocity += new Vector3(0, -speed, 0);
            parrotRotation += new Vector3(15, 0, 0);
        }
        else
        {
            rotateParrot = true;
            parrotRotation = Vector3.zero;
        }

        if (Mathf.Abs(horizontalInput) > inputDelay)
        {
            //if input is greater than 0;
            if(horizontalInput > 0)
            {
                transform.Rotate(new Vector3(0, turnSpeed, 0));
                parrotRotation.y = transform.localEulerAngles.y;
               // parrotRotation.z = -45;
            }
            else if(horizontalInput < 0)
            {
                transform.Rotate(new Vector3(0, -turnSpeed, 0));
                parrotRotation.y = transform.localEulerAngles.y;
               // parrotRotation.z = 45;
            }
        }

        if(Mathf.Abs(verticalInput) > inputDelay)
        {
            if(verticalInput > 0)
            {
                //moves parrot constantly forward
                rBody.velocity += transform.forward * speed;
            }
            if (Mathf.Abs(horizontalInput) > inputDelay)
            {
                //if input is greater than 0;
                if (horizontalInput > 0)
                {
                    parrotRotation.z = -45;
                }
                else if (horizontalInput < 0)
                {
                    parrotRotation.z = 45;
                }
            }
        }

        print(rBody.velocity);

        parrotRotation.y = transform.localEulerAngles.y;


        transform.localEulerAngles = parrotRotation;
    }
    #endregion
}
