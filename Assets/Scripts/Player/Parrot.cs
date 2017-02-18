using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parrot : MonoBehaviour
{
    #region Attributes
    private int health = 10;
    [SerializeField] private float speed = 2.0f;
    [SerializeField] private float turnSpeed = 2.0f;
    [SerializeField] private float minHeight = 0;
    [SerializeField] private float maxHeight = 15;
    private float currentHeight;
    //If the parrot is active
    private bool active;
    //The pirate
    BasePirate tPP;
    //If the parrot can land or take off again
    private bool canChangeCharacter; 

    private Rigidbody rBody;
    private bool rotateParrot = false;
    //parrot euler angle rotation
    Vector3 parrotRotation = new Vector3(); 

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

    #region InBuiltMethods
    // Use this for initialization
    void Start()
    {
        rBody = GetComponent<Rigidbody>();
        cam = FindObjectOfType<PirateCamera>();
        //The parrot is active
        active = true;
        //The parrot can be a parrotsite
        canChangeCharacter = true; 
	}

    //Update is called once per frame
    private void Update() 
    {
        //Let the parrot take off again
        Takeoff(); 
    }

    //Physics updates
    void FixedUpdate() 
    {
        if (active)
        {
            ParrotMove();
        }
        //Stop the parrot if it is not active
        else if (!active) 
        {
            //Make sure the parrot stays with the pirate
            rBody.velocity = Vector3.zero;
            transform.position = new Vector3(tPP.transform.position.x, transform.position.y, tPP.transform.position.z);
            transform.rotation = tPP.transform.rotation;

            //TODO: make parrot invisable. Best done when rough assets arrive
        }
	}

    private void OnTriggerStay(Collider coll)
    {
        //Landing on pirate
        if (coll.tag == "Pirate" && Input.GetButton("Interact") && active && canChangeCharacter) 
        {
            //Set the target of the camera
            cam.Target = coll.gameObject.transform;
            //Get the script from the pirate
            tPP = coll.GetComponent<BasePirate>();
            //Enable the pirate
            tPP.PirateActive = true;
            //Disable the parrot
            active = false; 

            StartCoroutine(ChangeTimer());
        }
    }

    //Coroutine to prevent immediate landing or takeoff from pirate
    internal IEnumerator ChangeTimer() 
    {
        canChangeCharacter = false;

        yield return new WaitForSeconds(2);

        canChangeCharacter = true;
    }
    #endregion

    #region HelperMethods
    /// <summary>
    /// The ParrotMove Method controls the parrot's flight and movement. It is what turns the bird, boosts, decellerates, what angles the parrot during flight.
    /// </summary>
    private void ParrotMove()
    {
        //Get inputs for Parrot movement
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        
        flyUpInput = Input.GetAxis("FlyUp");
        flyDownInput = Input.GetAxis("FlyDown");

        float boostInput = Input.GetAxis("BoostFly");
        bool boost = Input.GetButton("BoostFly");

        //zero velocity
        rBody.velocity = Vector3.zero;

        //zero parrot rotation
        parrotRotation = Vector3.zero;

        //parrot move forwards
        //if accelerate btn is pressed
        if (boost && boostInput > 0) 
        {
            //parrot speed is doubled
            rBody.velocity = transform.forward * (2 * speed); 
        }
        //if Decelerate btn is pressed
        else if (boost && boostInput < 0) 
        {
            //parrot speed is halved
            rBody.velocity = transform.forward * (0.5f * speed); 
        }
        //if niether the acclerate btn or decelerate btn is pressed 
        else
        {
            //parrot speed is normal
            rBody.velocity += transform.forward * speed; 
        }

        //Parrot fly up
        //make sure input is greater than deadzone range
        if (Mathf.Abs(verticalInput) > inputDelay) 
        {
            //if vertical input is positive and less than max height
            if (verticalInput > 0 && transform.position.y < maxHeight) 
            {
                //a upwards velocity is add to parrot's current speed
                rBody.velocity += new Vector3(0, speed, 0);
                //rotates parrot up
                parrotRotation += new Vector3(-15, 0, 0); 
            }
            //if vertical input is pointing down and greater than min height
            else if (transform.position.y > minHeight)
            {
                //a downwards velocity is added to parrot's current velocity
                rBody.velocity += new Vector3(0, -speed, 0); 
                parrotRotation += new Vector3(15, 0, 0);
            }
        }
        //code bellow does same thing as code above except using the triggers
        else if(Mathf.Abs(flyUpInput) > inputDelay && transform.position.y < maxHeight)
        {
            rBody.velocity += new Vector3(0, speed, 0);
            parrotRotation += new Vector3(-15, 0, 0);
        }
        //parrot fly down
        else if (Mathf.Abs(flyDownInput) > inputDelay && transform.position.y > minHeight) 
        {
            rBody.velocity += new Vector3(0, -speed, 0);
            parrotRotation += new Vector3(15, 0, 0);
        }

        //controls parrot turning
        if (Mathf.Abs(horizontalInput) > inputDelay)
        {
            //turns parrot left
            if (horizontalInput > 0) 
            {
                //rotates the parrot left
                transform.Rotate(new Vector3(0, turnSpeed, 0));
                //angles parrot left
                parrotRotation.z = -45; 
            }
            //turns parrot right
            else if (horizontalInput < 0) 
            {
                //rotates parrot right
                transform.Rotate(new Vector3(0, -turnSpeed, 0));
                //angles parrot right
                parrotRotation.z = 45; 
            }
        }

        parrotRotation.y = transform.localEulerAngles.y;

        //update the parrot rotation
        transform.localEulerAngles = parrotRotation; 
    }

    /// <summary>
    /// This method is used to allow the parrot to take off again
    /// </summary>
    private void Takeoff()
    {
        //Taking off from pirate
        if (Input.GetButton("Interact") && !active && canChangeCharacter) 
        {
            //Set the target of the camera
            cam.Target = gameObject.transform;
            //Activate the parrot
            active = true; 
            
            //TODO: update this with the AI
            tPP.RBody.velocity = Vector3.zero;
            tPP.transform.localEulerAngles = Vector3.zero;
            //Disable the pirate
            tPP.PirateActive = false; 

            StartCoroutine(ChangeTimer());
        }
    }
    #endregion
}