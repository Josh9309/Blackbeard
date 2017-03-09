using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parrot : MonoBehaviour
{
    #region Attributes
    //Health and flight
    private int health = 10;
    [SerializeField] private float speed = 2.0f;
    [SerializeField] private float turnSpeed = 2.0f;
    [SerializeField] private float minHeight = 0;
    [SerializeField] private float maxHeight = 15;
    private float currentHeight;
    private bool active; //If the parrot is active
    private Rigidbody rBody;
    private bool rotateParrot = false;
    private Vector3 parrotRotation; //parrot euler angle rotation

    //Switching
    private BasePirate basePirateScript; //The pirate
    private Buccaneer buccScript; //Buccaneer script
    private TreasureHunter treasureHScript; //Treasure hunter script
    private NPC npcScript;
    private bool canChangeCharacter; //If the parrot can land or take off again

    //Item pickup
    private List<GameObject> items;
    private List<Rigidbody> itemsRB;
    private List<Item> itemsScripts;
    private GameObject carriedItem;
    private Rigidbody carriedItemRB;
    private Item carriedItemScript;
    private Transform itemSlot;
    private RaycastHit hit;
    private int visionAngle;
    private bool buttonDown;

    //input stuff
    private float inputDelay = 0.3f;
    private float horizontalInput = 0;
    private float verticalInput = 0;
    private float flyUpInput = 0;
    private float flyDownInput = 0;

    private PirateCamera cam;
    private GameManager gm;
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
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        //Item pickup
        items = new List<GameObject>();
        itemsRB = new List<Rigidbody>();
        itemsScripts = new List<Item>();
        GameObject[] foundItems = GameObject.FindGameObjectsWithTag("Item");
        for (int i = 0; i < foundItems.Length; i++) //Get all of the needed item componenets
        {
            items.Add(foundItems[i]);
            itemsRB.Add(foundItems[i].GetComponent<Rigidbody>());
            itemsScripts.Add(foundItems[i].GetComponent<Item>());
        }
        carriedItem = null;
        carriedItemRB = null;
        itemSlot = GameObject.FindGameObjectWithTag("ItemSlot").transform;
        visionAngle = 45;
        buttonDown = false;
	}

    //Update is called once per frame
    private void Update() 
    {
        //Let the parrot take off again
        if (!active)
            Takeoff();

        if (active)
            Pickup();
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
            //Make sure the parrot stays with the pirate
            rBody.velocity = Vector3.zero;
            transform.position = new Vector3(basePirateScript.transform.position.x, transform.position.y, basePirateScript.transform.position.z);
            transform.rotation = basePirateScript.transform.rotation;

            //TODO: make parrot invisable. Best done when rough assets arrive
        }
	}

    private void OnTriggerStay(Collider coll)
    {
        //Landing on pirate
        if (coll.tag == "Pirate" && Input.GetButton("Interact") && active && canChangeCharacter && carriedItem == null) 
        {
            //Set the target of the camera
            cam.Target = coll.gameObject.transform;

            //Get scripts from the pirate
            basePirateScript = coll.GetComponent<BasePirate>();
            npcScript = coll.GetComponent<NPC>();
            npcScript.Active = false;

            if (basePirateScript is Buccaneer)
            {
                buccScript = coll.GetComponent<Buccaneer>();
                gm.CurrentPlayerState = GameManager.PlayerState.BUCCANEER;
            }
            else if (basePirateScript is TreasureHunter)
            {
                treasureHScript = coll.GetComponent<TreasureHunter>();
                gm.CurrentPlayerState = GameManager.PlayerState.HUNTER;
            }

            gm.Player = coll.gameObject;
            

            //Enable the pirate
            basePirateScript.PirateActive = true;
            //Disable the parrot
            active = false; 

            StartCoroutine(ChangeTimer(2));
        }
    }

    //Coroutine to prevent immediate landing or takeoff from pirate
    internal IEnumerator ChangeTimer(int time) 
    {
        canChangeCharacter = false;

        yield return new WaitForSeconds(time);

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

    /// <summary>
    /// This method is used to allow the parrot to take off again
    /// </summary>
    private void Takeoff()
    {
        bool doingRelevantAction = false;

        //If either pirate type is perfoming an action
        if (buccScript != null && buccScript.AttState != Buccaneer.AttackState.Idle)
            doingRelevantAction = true;
        else if (treasureHScript != null && treasureHScript.PickingUp)
            doingRelevantAction = true;

        //Taking off from pirate
        //Timer for switching must be completed
        //Pirate must not be jumping, picking up treasure, or attacking
        if (basePirateScript.Health <= 0 || (Input.GetButton("Interact") && !active && canChangeCharacter && basePirateScript.Grounded && !doingRelevantAction)) 
        {
            //Set the target of the camera
            cam.Target = gameObject.transform;
            //Activate the parrot
            active = true;

            gm.Player = this.gameObject;
            gm.CurrentPlayerState = GameManager.PlayerState.PARROT;
            
            //TODO: update this with the AI
            basePirateScript.RBody.velocity = Vector3.zero;
            basePirateScript.transform.localEulerAngles = Vector3.zero;

            //Disable the pirate scripts
            basePirateScript.PirateActive = false;
            buccScript = null;
            treasureHScript = null;
            basePirateScript = null;

            npcScript.Active = true;
            npcScript = null;

            StartCoroutine(ChangeTimer(2));
        }
    }

    /// <summary>
    /// Let the parrot pick up treasure
    /// </summary>
    private void Pickup()
    {
        //Checking if the items can be picked up or put down
        if (carriedItem == null) //If no item is currently carried, search for an item for the player to pick up
        {
            for (int i = 0; i < items.Count; i++)
            {
                //If an item will been destroyed outside of this script
                if (items[i] == null)
                {
                    items.Remove(items[i]);
                    itemsRB.Remove(itemsRB[i]);
                    itemsScripts.Remove(itemsScripts[i]);
                }

                Vector3 direction = (items[i].transform.position + new Vector3(0, items[i].transform.localScale.y / 2, 0)) - transform.position;

                //Raycast to pick up the treasure
                Physics.Raycast(transform.position, direction, out hit);

                //TODO: update this with UI cues
                if (direction.magnitude < 1.5f && Vector3.Dot(direction, transform.forward) > -.1f)
                {
                    if (Input.GetButton("Attack"))
                    {
                        carriedItem = items[i];
                        carriedItemRB = itemsRB[i];
                        carriedItemScript = itemsScripts[i];
                        buttonDown = true; //Prevents immediate release of items
                        break;
                    }
                }
            }
        }
        else if (Input.GetButtonUp("Attack")) //Lets items be released
            buttonDown = false;

        //Picking up and putting down items
        if (Input.GetButton("Attack") && !buttonDown && carriedItem != null) //Putting down items
        {
            carriedItemScript.Active = true; //Activate the items
            carriedItem.transform.rotation = Quaternion.AngleAxis(0, Vector3.zero); //Rotate the item for the drop

            //Determine the properties of the item being dropped and drop it
            if (carriedItem.name.Contains("Coconut") || carriedItem.name.Contains("Bomb"))
            {
                carriedItemRB.useGravity = true;
                carriedItem.transform.parent = null;
            }
            else if (carriedItem.name.Contains("Rum") && health < 10) //Restore health if it can be restored
            {
                health = 10;

                Destroy(carriedItem); //Destroy the rum

                //Remove the items from the list and set them to null
                items.Remove(carriedItem);
                itemsRB.Remove(carriedItemRB);
                itemsScripts.Remove(carriedItemScript);
            }
            else if (carriedItem.name.Contains("Rum") && health >= 10) //Drop rum
            {
                carriedItemRB.useGravity = true;
                carriedItem.transform.parent = null;
            }

            //Remove the items from the list and set them to null
            items.Remove(carriedItem);
            itemsRB.Remove(carriedItemRB);
            itemsScripts.Remove(carriedItemScript);

            //Set all current items to null
            carriedItem = null;
            carriedItemRB = null;
            carriedItemScript = null;
        }
        else if (carriedItem != null) //When trying to pick something up, make sure nothing is currently held
        {
            carriedItemRB.useGravity = false;
            
            //Set the treasure for when it is picked up
            carriedItem.transform.parent = itemSlot;
            carriedItem.transform.localPosition = Vector3.zero;
            carriedItem.transform.rotation = itemSlot.transform.rotation;
        }
    }
    #endregion
}