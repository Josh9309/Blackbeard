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
    private bool active; //If the parrot is active
    BasePirate tPP; //The pirate
    private bool canChangeCharacter; //If the parrot can land or take off again

    private Rigidbody rBody;
    private bool rotateParrot = false;
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

    #region Magic Methods
    // Use this for initialization
    void Start () {
        rBody = GetComponent<Rigidbody>();
        cam = FindObjectOfType<PirateCamera>();
        active = true; //The parrot is active
        canChangeCharacter = true; //The parrot can be a parrotsite
	}
    
    private void Update() //Update is called once per frame
    {
        Takeoff(); //Let the parrot take off again
    }

    void FixedUpdate() //Physics updates
    {
        if (active)
        {
            ParrotMove();
        }
        else if (!active) //Stop the parrot if it is not active
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
        if (coll.tag == "Pirate" && Input.GetButton("Interact") && active && canChangeCharacter) //Landing on pirate
        {
            cam.Target = coll.gameObject.transform; //Set the target of the camera

            tPP = coll.GetComponent<BasePirate>(); //Get the script from the pirate
            tPP.enabled = true; //Enable the pirate

            active = false; //Disable the parrot

            StartCoroutine(ChangeTimer());
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

    internal IEnumerator ChangeTimer() //Coroutine to prevent immediate landing or takeoff from pirate
    {
        canChangeCharacter = false;

        yield return new WaitForSeconds(2);

        canChangeCharacter = true;
    }
    #endregion

    #region Methods
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
        if (boost && boostInput > 0) //if accelerate btn is pressed
        {
            rBody.velocity = transform.forward * (2 * speed); //parrot speed is doubled
        }
        else if(boost && boostInput < 0) //if Decelerate btn is pressed
        {
            rBody.velocity = transform.forward * (0.5f * speed); //parrot speed is halved
        }
        else //if niether the acclerate btn or decelerate btn is pressed 
        {
            rBody.velocity += transform.forward * speed; //parrot speed is normal
        }
        
        //Parrot fly up
        if(Mathf.Abs(verticalInput) > inputDelay) //make sure input is greater than deadzone range
        {
            if(verticalInput > 0 && transform.position.y < maxHeight) //if vertical input is positive and less than max height
            {
                rBody.velocity += new Vector3(0, speed, 0); //a upwards velocity is add to parrot's current speed
                parrotRotation += new Vector3(-15, 0, 0); //rotates parrot up
            }
            else if(transform.position.y > minHeight) //if vertical input is pointing down and greater than min height
            {
                rBody.velocity += new Vector3(0, -speed, 0); //a downwards velocity is added to parrot's current velocity
                parrotRotation += new Vector3(15, 0, 0);
            }
        }
        //code bellow does same thing as code above except using the triggers
        else if(Mathf.Abs(flyUpInput) > inputDelay && transform.position.y < maxHeight)
        {
            rBody.velocity += new Vector3(0, speed, 0);
            parrotRotation += new Vector3(-15, 0, 0);
        }
        else if (Mathf.Abs(flyDownInput) > inputDelay && transform.position.y > minHeight) //parrot fly down
        {
            rBody.velocity += new Vector3(0, -speed, 0);
            parrotRotation += new Vector3(15, 0, 0);
        }

        //controls parrot turning
        if (Mathf.Abs(horizontalInput) > inputDelay)
        {
            if(horizontalInput > 0) //turns parrot left
            {
                transform.Rotate(new Vector3(0, turnSpeed, 0)); //rotates the parrot left
                parrotRotation.z = -45; //angles parrot left
            }
            else if(horizontalInput < 0) //turns parrot right
            {
                transform.Rotate(new Vector3(0, -turnSpeed, 0)); //rotates parrot right
                parrotRotation.z = 45; //angles parrot right
            }
        }

        parrotRotation.y = transform.localEulerAngles.y;


        transform.localEulerAngles = parrotRotation; //update the parrot rotation
    }

    /// <summary>
    /// This method is used to allow the parrot to take off again
    /// </summary>
    private void Takeoff()
    {
        if (Input.GetButton("Interact") && !active && canChangeCharacter) //Taking off from pirate
        {
            cam.Target = gameObject.transform; //Set the target of the camera

            active = true; //Activate the parrot
            
            //TODO: update this with the AI
            tPP.RBody.velocity = Vector3.zero;
            tPP.transform.localEulerAngles = Vector3.zero;

            tPP.enabled = false; //Disable the pirate

            StartCoroutine(ChangeTimer());
        }
    }
    #endregion
}