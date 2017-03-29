using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parrot : MonoBehaviour
{
    #region Attributes
    //parrot Stats
    [SerializeField] private int playerNum = 1;
    [SerializeField] private float speed = 2.0f;
    private float maxSpeed;
    [SerializeField] private float turnSpeed = 2.0f;
    [SerializeField] private float minHeight = 0;
    [SerializeField] private float maxHeight = 15;
    private float currentHeight;
    public bool active; //If the parrot is active
    private Rigidbody rBody;
    private bool rotateParrot = false;
    private Vector3 parrotRotation; //parrot euler angle rotation
    private GameObject captain; // object of the captain this pirate is tied to

    //Item pickup
    private ItemPickup pickupScript;

    //Trap interaction
    private TrapInteraction trapScript;

    //input stuff
    private PlayerInput inputManager;
    private float inputDelay = 0.3f;
    private float horizontalInput = 0;
    private float verticalInput = 0;
    private float flyUpInput = 0;
    private float flyDownInput = 0;

    //Game Manager
    private GameManager gm;

    #endregion

    #region Properties
    public PlayerInput InputManager
    {
        get { return inputManager; }
    }
    #endregion

    #region InBuiltMethods
    // Use this for initialization
    void Start()
    {
        rBody = GetComponent<Rigidbody>();

        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        //get input manager and captains
        switch (playerNum)
        {
            case 1:
                captain = gm.GetComponent<GameManager>().PirateP1.gameObject;
                inputManager = gm.P1Input;
                break;

            case 2:
                captain = gm.GetComponent<GameManager>().PirateP2.gameObject;
                inputManager = gm.P2Input;
                break;
        }

        //The parrot is active
        active = false;

        pickupScript = GetComponent<ItemPickup>(); //Get the item pickup script
        trapScript = GetComponent<TrapInteraction>(); //Get the trap interaction script

        maxSpeed = speed * 3;
	}

    //Update is called once per frame
    private void Update() 
    {
        pickupScript.Pickup(active); //Let the parrot pickup treasure
        trapScript.Interact(active); //Let the parrot interact with traps
    }

    //Physics updates
    void FixedUpdate() 
    {
        if (active)
        {
            ParrotMove();
        }
        else if (!active) //Stop the parrot if it is not active
        {
            rBody.velocity = Vector3.zero;
            StayWithCaptain();
        }
	}
    #endregion

    #region HelperMethods
    /// <summary>
    /// The ParrotMove Method controls the parrot's flight and movement. It is what turns the bird, boosts, decellerates, what angles the parrot during flight.
    /// </summary>
    private void ParrotMove()
    {
        //Get inputs for Parrot movement
        horizontalInput = Input.GetAxis(inputManager.HORIZONTAL_AXIS);
        verticalInput = Input.GetAxis(inputManager.VERTICAL_AXIS);
        
        flyUpInput = Input.GetAxis(inputManager.FLY_UP_AXIS);
        flyDownInput = Input.GetAxis(inputManager.FLY_DOWN_AXIS);

        float boostInput = Input.GetAxis(inputManager.BOOST_AXIS);
        bool boost = Input.GetButton(inputManager.BOOST_AXIS);

        //zero velocity
        rBody.velocity = Vector3.zero;

        //zero parrot rotation
        parrotRotation = Vector3.zero;

        //parrot move forwards
        //if accelerate btn is pressed
        if (boost && boostInput > 0) 
        {
            //---old back up ----
            //parrot speed is doubled
            //rBody.velocity = transform.forward * (2 * speed); 

            //increase speed
            speed += 0.1f;
            if(speed > maxSpeed)
            {
                speed = maxSpeed;
            }
        }
        //if Decelerate btn is pressed
        else if (boost && boostInput < 0) 
        {
            //parrot speed is halved
            //rBody.velocity = transform.forward * (0.5f * speed); 

            //decrease speed
            speed -= 0.1f;
            if(speed < 0)
            {
                speed = 0;
            }
        }
        //if neither the accelerate btn or decelerate btn is pressed 
        
        //parrot speed is added to velocity
        rBody.velocity += transform.forward * speed; 

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
                parrotRotation += new Vector3(-45, 0, 0); 
            }
            //if vertical input is pointing down and greater than min height
            else if (transform.position.y > minHeight)
            {
                //a downwards velocity is added to parrot's current velocity
                rBody.velocity += new Vector3(0, -speed, 0); 
                parrotRotation += new Vector3(45, 0, 0);
            }
        }
        //code below does same thing as code above except using the triggers
        else if(Mathf.Abs(flyUpInput) > inputDelay && transform.position.y < maxHeight)
        {
            rBody.velocity += new Vector3(0, speed, 0);
            parrotRotation += new Vector3(-45, 0, 0);
        }
        //parrot fly down
        else if (Mathf.Abs(flyDownInput) > inputDelay && transform.position.y > minHeight) 
        {
            rBody.velocity += new Vector3(0, -speed, 0);
            parrotRotation += new Vector3(45, 0, 0);
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

    public void ReturnToCaptain(Transform captainTransform)
    {
        // TODO: for use when we have parrot
    }

    /// <summary>
    /// code for parrot staying with the captain without parenting
    /// </summary>
    private void StayWithCaptain()
    {
        transform.position = new Vector3(captain.transform.position.x, captain.transform.position.y + 2.5f, captain.transform.position.z);
        transform.rotation = captain.transform.rotation;
    }
    #endregion
}