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
    [SerializeField] private float accelRate = .5f;
    private float currentHeight;
    public bool active; //If the parrot is active
    private Rigidbody rBody;
    private bool rotateParrot = false;
    private Vector3 parrotRotation; //parrot euler angle rotation
    private GameObject captain; // object of the captain this pirate is tied to
    private Coroutine signalCor; //coroutine that holds the signal 
    

    //Item pickup
    //private ItemPickup pickupScript;

    // Utility items & managment
    private GameObject itemSlot;
    [SerializeField]
    private List<GameObject> utilityItems;
    [SerializeField]
    private int numLanterns;
    private GameObject currentUtility; // represents the current utility NOT the one actually held
    private GameObject heldUtility; // is the actual utility the parrot is holding
    private int currentUtilityID = 0;
    private bool canDrop = true;
    private bool canSwitch = true;
    private float switchCooldown = 0.5f;
    private float dropCooldown; // represents the current utility's dropCooldown
    private bool switchButtonDown = false;
    private bool dropButtonDown = false;

    // cooldowns for utilities
    // match the index in the utilityCooldowns list to that in the utility list
    [SerializeField]
    private List<float> utilityCooldowns;

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

    public float DropCoolDown
    {
        get { return dropCooldown; }
    }

	public bool CanDrop
	{
		get { return canDrop; }
	}

    #endregion

    #region InBuiltMethods
    // Use this for initialization
    void Start()
    {
        rBody = GetComponent<Rigidbody>();

        gm = GameManager.Instance;

        //get input manager and captains
        switch (playerNum)
        {
            case 1:
                captain = gm.PirateP1.gameObject;
                itemSlot = GameObject.FindGameObjectWithTag("ItemSlot1");
                inputManager = gm.P1Input;
                break;

            case 2:
                captain = gm.PirateP2.gameObject;
                itemSlot = GameObject.FindGameObjectWithTag("ItemSlot2");
                inputManager = gm.P2Input;
                break;
        }

        ReturnToSpawn(gm.ParrotSpawn.transform.position);

        //pickupScript = GetComponent<ItemPickup>(); //Get the item pickup script
        trapScript = GetComponent<TrapInteraction>(); //Get the trap interaction script
        currentUtility = utilityItems[0]; // assign initial utility
        dropCooldown = utilityCooldowns[0]; // assign initial utility
        SpawnUtility();

        maxSpeed = speed * 3;
	}

    //Update is called once per frame
    private void Update() 
    {
        Signal(); //turns on the signal beams for pirates
        if (active)
        {
            //pickupScript.Pickup(active); //Let the parrot pickup treasure
            trapScript.Interact(active); //Let the parrot interact with traps
            SwitchUtility(); // allow parrot to switch current utility
            DropUtility(); // allows parrot to drop utility
        }
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
            ReturnToSpawn(gm.ParrotSpawn.transform.position);
            //StayWithCaptain();
        }
	}
    #endregion

    #region HelperMethods
    /// <summary>
    /// This will handle updating the currentUtility based on player input
    /// </summary>
    private void SwitchUtility()
    {
        if (Input.GetButton(inputManager.UTILITY_SWITCH) && !switchButtonDown)
        {
            currentUtilityID++;
            currentUtility = utilityItems[currentUtilityID % utilityItems.Count]; // wrap the utilities so that we don't go out of bounds
            dropCooldown = utilityCooldowns[currentUtilityID % utilityItems.Count];
            Debug.Log("current utility is " + currentUtility.name);
            switchButtonDown = true;
            // destroy old utility
            GameObject.Destroy(heldUtility);
            if (heldUtility != null)
                SpawnUtility();
            //StartCoroutine(SwitchUtilityCooldown());
        }

        else if (Input.GetButtonUp(inputManager.UTILITY_SWITCH))
        {
            switchButtonDown = false;
        }
    }

    /// <summary>
    /// will assign the currently selected utility to be held
    /// </summary>
    private void SpawnUtility()
    {
        heldUtility = GameObject.Instantiate(currentUtility, 
            new Vector3(itemSlot.transform.position.x, 
            itemSlot.transform.position.y, itemSlot.transform.position.z),
            Quaternion.identity);
        heldUtility.transform.position = new Vector3(itemSlot.transform.position.x,
            itemSlot.transform.position.y - (heldUtility.GetComponent<Collider>().bounds.size.y / 1.8f), itemSlot.transform.position.z);
        heldUtility.transform.parent = this.gameObject.transform;
        heldUtility.GetComponent<Item>().Active = false;
        heldUtility.GetComponent<Rigidbody>().useGravity = false;
        heldUtility.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        if (heldUtility.name.Contains("Bear_Trap"))
        {
            heldUtility.GetComponent<BearTrap>().enabled = false;
        }
    }

    /// <summary>
    /// drops the currently selected utility
    /// </summary>
    private void DropUtility()
    {
        if (Input.GetButton(inputManager.PARROT_PICKUP_AXIS) && canDrop && !dropButtonDown)
        {
            heldUtility.GetComponent<Rigidbody>().useGravity = true;
            heldUtility.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            heldUtility.transform.parent = null;
            heldUtility.GetComponent<Item>().Active = true;
            if (heldUtility.name.Contains("Bear_Trap"))
            {
                //heldUtility.GetComponent<Item>().enabled = false;
                heldUtility.GetComponent<BearTrap>().Activate();
            }
            heldUtility = null;
            Debug.Log(currentUtility.name + " has been dropped!");
            dropButtonDown = true;
            StartCoroutine(UtilityCooldown());
        }
        else if (Input.GetButtonUp(inputManager.PARROT_PICKUP_AXIS))
        {
            dropButtonDown = false;
        }
    }

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
            speed += accelRate;
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
            speed -= accelRate;
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

    public void ReturnToSpawn(Vector3 spawnPosition)
    {
        transform.position = spawnPosition;
        if(playerNum == 1)
        {
            transform.forward = gm.PirateP2.transform.position - transform.position;
        }
        else
        {
            transform.forward = gm.PirateP1.transform.position - transform.position;
        }
        
    }

    /// <summary>
    /// code for parrot staying with the captain without parenting
    /// </summary>
    private void StayWithCaptain()
    {
        transform.position = new Vector3(captain.transform.position.x, captain.transform.position.y + 2.5f, captain.transform.position.z);
        transform.rotation = captain.transform.rotation;
    }

    /// <summary>
    /// prevents players from spamming utilities
    /// </summary>
    /// <returns></returns>
    private IEnumerator UtilityCooldown()
    {
        canDrop = false;
        yield return new WaitForSeconds(dropCooldown);
        SpawnUtility();
        canDrop = true;
    }

    /// <summary>
    /// prevents reading in multiple swtich inputs per second
    /// </summary>
    /// <returns></returns>
    private IEnumerator SwitchUtilityCooldown()
    {
        canSwitch = false;
        yield return new WaitForSeconds(switchCooldown);
        canSwitch = true;
    }

    private void Signal()
    {
        if(active && !gm.SignalOn && Input.GetButtonDown(inputManager.SIGNAL_AXIS))
        {
            signalCor = StartCoroutine(gm.SignalBeam(name));
        }
        else if (!active && gm.SignalOn)
        {
            //gm.SignalOn = false;
            if (signalCor != null)
            {
                StopCoroutine(signalCor);
            }
            //GameManager.Instance.StopSignalBeam();
        }
    }
    #endregion
}