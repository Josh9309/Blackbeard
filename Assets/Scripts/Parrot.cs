using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parrot : MonoBehaviour {
    #region Attributes
    private int health = 10;
    [SerializeField] private float speed = 2.0f;
    [SerializeField] private float turnSpeed = 2.0f;
    [SerializeField] private float minHeight = 0;
    [SerializeField] private float maxHeight = 15;
    private float currentHeight;
    
    private Rigidbody rBody;
    private bool rotateParrot = false;
    [SerializeField] private bool copter = false; //switches parrot to kevin's copter control
    Vector3 parrotRotation = new Vector3(); //parrot euler angle rotation
    //input stuff
    private float inputDelay = 0.3f;
    private float horizontalInput = 0;
    private float verticalInput = 0;
    private float flyUpInput = 0;
    private float flyDownInput = 0;

    private PirateCamera cam;
    #endregion

    #region Properties
    #endregion

    // Use this for initialization
    void Start () {
        rBody = GetComponent<Rigidbody>();
        cam = FindObjectOfType<PirateCamera>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (!copter)
        {
            ParrotMove();
        }
        else
        {
            ParrotCopterControl();
        }
	}

    private void OnTriggerStay(Collider coll)
    {
        if (coll.tag == "Pirate" && Input.GetButton("Fire1"))
        {
            TestPiratePlayer tPP = coll.GetComponent<TestPiratePlayer>(); //Get the script from the pirate
            tPP.enabled = true; //Enable the pirate
            cam.Target = coll.gameObject.transform; //Set the target of the camera

            this.enabled = false; //Disable the parrot
        }

        //if () //If the parrot has landed

        //if (coll.tag == "Pirate")
        //{
        //
        //}
        //else if(coll.tag == "Perch")
        //{
        //    
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

        //parrot move forwards
        if (Input.GetButton("BoostFly") && Input.GetAxis("BoostFly") > 0)
        {
            rBody.velocity += transform.forward * (2 * speed);
        }
        else if(Input.GetButton("BoostFly") && Input.GetAxis("BoostFly") < 0)
        {
            rBody.velocity = Vector3.zero;
        }
        else
        {
            rBody.velocity += transform.forward * speed;
        }
        
        //Parrot fly up
        if(Mathf.Abs(verticalInput) > inputDelay)
        {
            if(verticalInput > 0 && transform.position.y < maxHeight)
            {
                rBody.velocity += new Vector3(0, speed, 0);
            }
            else if(transform.position.y > minHeight)
            {
                rBody.velocity += new Vector3(0, -speed, 0);
            }
        }
        else if(Mathf.Abs(flyUpInput) > inputDelay && transform.position.y < maxHeight)
        {
            rBody.velocity += new Vector3(0, speed, 0);
        }
        else if (Mathf.Abs(flyDownInput) > inputDelay && transform.position.y > minHeight) //parrot fly down
        {
            rBody.velocity += new Vector3(0, -speed, 0);
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

        if(!(Input.GetButton("BoostFly") && Input.GetAxis("BoostFly") < 0))
        {
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

            if(Mathf.Abs(verticalInput) > inputDelay)
            {
                if(verticalInput > 0)
                {
                    parrotRotation += new Vector3(-15, 0, 0);
                    rotateParrot = true;
                }
                else if (verticalInput < 0)
                {
                    parrotRotation += new Vector3(15, 0, 0);
                    rotateParrot = true;
                }
            }
            else if (Mathf.Abs(flyUpInput) > inputDelay)
            {
                parrotRotation += new Vector3(-15, 0, 0);
                rotateParrot = true;
            }
            else if (Mathf.Abs(flyDownInput) > inputDelay) //parrot fly down
            {
                parrotRotation += new Vector3(15, 0, 0);
                rotateParrot = true;
            }
        }

        //print(rBody.velocity);

        parrotRotation.y = transform.localEulerAngles.y;


        transform.localEulerAngles = parrotRotation;
    }

    private void ParrotCopterControl()
    {
        rBody.velocity = Vector3.zero;

        //parrot move forwards
        if (Input.GetAxis("BoostFly") > 0)
        {
            rBody.velocity += transform.forward * (2 * speed);
        }
        //Make the parrot fly forward and backwards
        else if (Input.GetAxis("Vertical") > inputDelay)
        {
            rBody.velocity += transform.forward * speed;
        }
        else if (Input.GetAxis("Vertical") < -inputDelay)
        {
            rBody.velocity = -transform.forward;
        }
        else
        {
            rBody.velocity = Vector3.zero;
        }

        //Make the parrot fly upwards
        if (Input.GetAxis("FlyUp") > inputDelay && transform.position.y < maxHeight)
        {
            rBody.velocity += transform.up * speed;
        }

        //Make the parrot fly downwards
        if (Input.GetAxis("FlyDown") > inputDelay && transform.position.y > minHeight)
        {
            rBody.velocity -= transform.up * speed;
        }

        //Make the parrot bank left and right
        if (Input.GetAxis("Horizontal") > inputDelay)
        {
            transform.Rotate(0, turnSpeed, 0);
        }
        else if (Input.GetAxis("Horizontal") < -inputDelay)
        {
            transform.Rotate(0, -turnSpeed, 0);
        }
        else
        {
            transform.Rotate(new Vector3(0, 0, 0));
        }
    }
    #endregion
}
