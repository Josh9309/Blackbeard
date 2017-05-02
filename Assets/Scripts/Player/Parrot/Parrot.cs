using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parrot : MonoBehaviour
{
    #region Attributes
    //parrot Stats
    [SerializeField] private int playerNum = 1;
    [SerializeField] private float baseSpeed = 10.0f;
    private float currentSpeed;
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

    // rotational attributes
    // angles to rotate to
    private float horizontalRot = 75.0f;
    private float verticalRot = 60.0f;
    // speed at which the parrot rotates between angles
    [SerializeField]
    private float rotRate = 1.5f;

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
    private int buttonHeldOnEntry;

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
    private float boostBrakeInput = 0;

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

    public int CurrentUtilityID
    {
        get { return currentUtilityID; }
    }

    public int NumLanterns
    {
        get { return numLanterns; }
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

        buttonHeldOnEntry = 0;

        currentSpeed = baseSpeed;
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
        else
            buttonHeldOnEntry = 0; //Reset to check held buttons again
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
            currentUtilityID = currentUtilityID % utilityItems.Count; //wrap utils so we dont go out of bounds
            currentUtility = utilityItems[currentUtilityID];
            dropCooldown = utilityCooldowns[currentUtilityID];
            Debug.Log("current utility is " + currentUtilityID + "/" + utilityItems.Count + currentUtility.name);
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
        heldUtility = Instantiate(currentUtility, 
            new Vector3(itemSlot.transform.position.x, 
            itemSlot.transform.position.y, itemSlot.transform.position.z),
            Quaternion.identity);

        Collider[] heldItemColliders = heldUtility.GetComponents<Collider>(); //Get the item's colliders

        heldUtility.transform.position = new Vector3(itemSlot.transform.position.x,
            itemSlot.transform.position.y - (heldItemColliders[0].bounds.size.y / 1.8f), itemSlot.transform.position.z);

        foreach (Collider c in heldItemColliders)
            c.enabled = false; //Disable the colliders so they don't hit pirates

        heldUtility.transform.parent = gameObject.transform;
        heldUtility.GetComponent<Item>().Active = false;

        Rigidbody heldUtilityRBody = heldUtility.GetComponent<Rigidbody>(); //Get the item's rigidbody
        heldUtilityRBody.useGravity = false;
        heldUtilityRBody.constraints = RigidbodyConstraints.FreezeAll;
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
        if (buttonHeldOnEntry == 0)
        {
            //Check if the jump button from the pirate was pressed
            if (Input.GetButton(inputManager.PARROT_PICKUP_AXIS))
                buttonHeldOnEntry = 1;
            else
                buttonHeldOnEntry = 2;
        }

        if (Input.GetButton(inputManager.PARROT_PICKUP_AXIS) && canDrop && !dropButtonDown && buttonHeldOnEntry == 2)
        {
            Debug.Log(Input.GetButton(inputManager.PARROT_PICKUP_AXIS) + "\n" + canDrop + "\n" + dropButtonDown + "\n" + buttonHeldOnEntry);

            Collider[] heldItemColliders = heldUtility.GetComponents<Collider>(); //Get the item's colliders
            foreach (Collider c in heldItemColliders)
                c.enabled = true; //Reenable the item's colliders

            Rigidbody heldUtilityRBody = heldUtility.GetComponent<Rigidbody>(); //Get the item's rigidbody
            heldUtilityRBody.useGravity = true;
            heldUtilityRBody.constraints = RigidbodyConstraints.None;
            heldUtilityRBody.velocity = new Vector3(rBody.velocity.x, 0, rBody.velocity.z);
            heldUtility.transform.parent = null;
            heldUtility.GetComponent<Item>().Active = true;
            if (heldUtility.name.Contains("Bear_Trap"))
            {
                //heldUtility.GetComponent<Item>().enabled = false;
                heldUtility.GetComponent<BearTrap>().Activate();
            }
            heldUtility = null;
            dropButtonDown = true;
            StartCoroutine(UtilityCooldown());
            numLanterns--; //added for UI test, just changes num lantern no actual functionality
        }
        else if (Input.GetButtonUp(inputManager.PARROT_PICKUP_AXIS))
        {
            dropButtonDown = false;
            buttonHeldOnEntry = 2;
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
        
        boostBrakeInput = Input.GetAxis(inputManager.BOOST_BRAKE_AXIS);
        //brakeInput = Input.GetAxis(inputManager.BRAKE_AXIS);

        float flyInput = Input.GetAxis(inputManager.FLY_AXIS);
        //Debug.Log("BOOST:" + boostInput + "Brake:" + brakeInput);
        
        bool fly = Input.GetButton(inputManager.FLY_AXIS);

        //zero velocity
        rBody.velocity = Vector3.zero;

        //zero parrot rotation
        Quaternion t = Quaternion.Euler(0, transform.eulerAngles.y, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, t, Time.deltaTime * rotRate);

        //parrot move forwards
        //if accelerate btn is pressed
        if (boostBrakeInput < 0) 
        {
            //parrot speed is doubled
            currentSpeed += accelRate;
            if(currentSpeed > baseSpeed * 3)
            {
                currentSpeed = baseSpeed * 3;
            }
        }
        //if Decelerate btn is pressed
        else if (boostBrakeInput > 0) 
        {
            //parrot speed is halved
            //rBody.velocity = transform.forward * (0.5f * speed); 

            //decrease speed
            currentSpeed -= baseSpeed/16;
            if(currentSpeed < baseSpeed/2)
            {
                currentSpeed = baseSpeed/2;
            }
        }
        else
        {
            currentSpeed = baseSpeed;
        }
        //if neither the accelerate btn or decelerate btn is pressed 
        
        //parrot speed is added to velocity
        rBody.velocity += transform.forward * currentSpeed;

        //Parrot fly up
        //make sure input is greater than deadzone range
        if (Mathf.Abs(verticalInput) > inputDelay) 
        {
            //if vertical input is positive and less than max height
            if (verticalInput > 0 && transform.position.y < maxHeight) 
            {
                //a upwards velocity is add to parrot's current speed
                rBody.velocity += new Vector3(0, currentSpeed, 0);

                //rotates parrot up
                Quaternion target = Quaternion.Euler(-verticalRot, transform.eulerAngles.y, 0);
                transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * rotRate);
            }
            //if vertical input is pointing down and greater than min height
            else if (verticalInput < 0 && transform.position.y > minHeight)
            {
                //a downwards velocity is added to parrot's current velocity
                rBody.velocity += new Vector3(0, -currentSpeed, 0);

                //angle the parrot down
                Quaternion target = Quaternion.Euler(verticalRot, transform.eulerAngles.y, 0);
                transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * rotRate);
            }
        }
        //code below does same thing as code above except using the triggers
        else if(fly && flyInput > 0)
        {
            rBody.velocity += new Vector3(0, currentSpeed, 0);
            //parrotRotation += new Vector3(-45, 0, 0);

            //rotates parrot up
            Quaternion target = Quaternion.Euler(-verticalRot, transform.eulerAngles.y, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * rotRate);
        }
        //parrot fly down
        else if (fly && flyInput < 0) 
        {
            rBody.velocity += new Vector3(0, -currentSpeed, 0);
            //parrotRotation += new Vector3(45, 0, 0);

            //angle the parrot down
            Quaternion target = Quaternion.Euler(verticalRot, transform.eulerAngles.y, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * rotRate);
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
                Quaternion target = Quaternion.Euler(0, transform.eulerAngles.y, -horizontalRot);
                transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * rotRate);
            }
            //turns parrot right
            else if (horizontalInput < 0) 
            {
                //rotates parrot right
                transform.Rotate(new Vector3(0, -turnSpeed, 0));

                //angles parrot right
                Quaternion target = Quaternion.Euler(0, transform.eulerAngles.y, horizontalRot);
                transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * rotRate);
            }
        }
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