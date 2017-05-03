using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]

public class CaptainPirate: MonoBehaviour
{
    #region Attributes
    //Player Id
    [SerializeField] private int playerNum;

    //pirate Stats
    private bool invincible = false;
    private bool stunned = false;
    private float speed = 5.0f;
    private bool pirateActive; //The pirate will only recieve input if it is active

    private Vector3 respawnLocation;
    private Vector3 backUpRespawn;

    //pirate animation attributes
    private Animator pirateAnim;

    //pirate movement attributes
    private Vector3 movement;
    private Vector3 camForwards;
    private Vector3 groundPlaneNormal;
    [SerializeField] private float groundedDist = 0.3f;
    [SerializeField] private float idleTurnSpeed = 360;
    [SerializeField] private float movingTurnSpeed = 180;
    [SerializeField] private float jumpForce = 50;
    [Range(1f, 4f)][SerializeField] private float gravityMultiplyer = 1.5f;
    private float turnAmount;
    private float forwardAmount;
    private bool grounded;
    public  LayerMask groundMask;
    private bool jumpInput;
    private bool canJump = true; //this bool is used by outside elements to prevent the player from jumping
    private bool canDoubleJump;
    private bool airControl = true;
    private bool onMoving; //pirate is on a surface that moves
    private bool onRotating; //pirate is on a surface that rotates
    private Vector3 movingPlatformVel = Vector3.zero; // the velocity of the moving platform that the pirate is on

    //input attributes
    private PlayerInput inputManager;
    [SerializeField] private float inputDelay = 0.3f;
    private float horizontalInput = 0;
    private float verticalInput = 0;

    //game camera variables
    private Transform gameCamera;
    private Rigidbody rBody;

    //Game Manager
    //private GameManager gm;

    //Camera access
    //SplitScreenCamera ssCamera;

    private Coroutine signalCor; //coroutine that holds the signal 
    #endregion

    #region Properties
    public bool Invincible
    {
        get { return invincible; }
    }

    public float Speed
    {
        get
        {
            return speed;
        }
        set
        {
            speed = value;
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

    public bool PirateActive
    {
        get
        {
            return pirateActive;
        }
        set
        {
            pirateActive = value;
        }
    }

    public bool Grounded
    {
        get
        {
            return grounded;
        }
    }

    public int PlayerNum
    {
        get { return playerNum; }
    }

    public bool CanJump
    {
        set { canJump = value; }
    }
    #endregion

    #region InBuiltMethods
    // Use this for initialization
    private void Start()
    {
        rBody = GetComponent<Rigidbody>();
        pirateAnim = GetComponent<Animator>();

        //set intial respawnLoc
        respawnLocation = transform.position;

        //make it so pirates and parrots can't collide
        Physics.IgnoreLayerCollision(10, 9);

        //Get stuff from the game manager
        GameManager gm = GameManager.Instance;
        SplitScreenCamera ssCamera = gm.GetComponent<SplitScreenCamera>();

        switch (playerNum)
        {
            case 1:
                gameCamera = ssCamera.CaptainCamera1.transform; //Get the camera
                break;

            case 2:
                gameCamera = ssCamera.CaptainCamera2.transform; //Get the camera
                break;
        }
        
        //get input manager based on player num
        switch (playerNum)
        {
            case 1:
                inputManager = gm.P1Input;
                break;

            case 2:
                inputManager = gm.P2Input;
                break;
        }

        //pirateActive = true;
    }
	
	// Update is called once per frame
    private void Update()
    {
        if (pirateActive)
        {
            if (!jumpInput && (canDoubleJump || grounded) && !stunned && canJump)
            {
                jumpInput = Input.GetButtonDown(inputManager.JUMP_AXIS);
            }

            if (Input.GetButtonDown(inputManager.RESPAWN_AXIS))
            {
                Respawn();
            }
        }
        else //pirate is not active
        {
            CheckIfGrounded();
            if (!grounded || onMoving) //if pirate is in air when not active 
            {
                //Respawn Pirate
                Respawn();
            }
        }
        Signal();

        if (Input.GetButtonDown(inputManager.PAUSE_AXIS))
        {
            //turn on/off the menu and the HUD
            MenuManager.Instance.MenuEnabled = !MenuManager.Instance.MenuEnabled;
            GameManager.Instance.HUD.GetComponent<Canvas>().enabled = (!GameManager.Instance.HUD.GetComponent<Canvas>().enabled);
        }
    }

	private void FixedUpdate ()
    {
        if (pirateActive && !stunned && !MenuManager.Instance.MenuEnabled)
        {
            GetMovementInput();
            PirateMove();
        }
	}

    private void OnCollisionEnter(Collision coll)
    {
        //turn off control of player in the air if they are in contact with something
        airControl = false;

        //Get scripts from the object
        Item itemScript = null;
        if (coll.gameObject.tag == "Item")
             itemScript = coll.gameObject.GetComponent<Item>();

        //Items colliding with the pirate
        //Fire and BearTrap handle the coroutine in their own respective classes
        if (itemScript != null && itemScript.Active)
        {
            if (coll.gameObject.name.Contains("Coconut"))
            {
                StartCoroutine(Stun(2));
                SoundManager.Instance.PlaySfx("hurt2", 100);
            }

            //Probably shouldn't do this
            //else //If this is any other object
            //{
            //    Destroy(gameObject);
            //}
        }
    }

    private void OnCollisionExit(Collision collisionInfo)
    {
        //turn on control of player in the air
        airControl = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(respawnLocation, .5f);

       // Gizmos.color = Color.red;
       // Gizmos.DrawSphere(backUpRespawn, .5f);
    }
    #endregion

    #region Methods
    private void GetMovementInput()
    {
        //Get inputs for Pirate movement
        horizontalInput = Input.GetAxis(inputManager.HORIZONTAL_AXIS);
        verticalInput = Input.GetAxis(inputManager.VERTICAL_AXIS);

        if (Mathf.Abs(verticalInput) > inputDelay || Math.Abs(horizontalInput) > inputDelay)
        {
            pirateAnim.SetBool("IsMoving", true);
        }
        else
        {
            pirateAnim.SetBool("IsMoving", false);
        }

        //calculate player movement direction to pass to pirate move
        if(gameCamera != null)
        {
            //calculates the camera relative direction  for the pirate to move
            camForwards = Vector3.Scale(gameCamera.forward, new Vector3(1, 0, 1)).normalized;
            movement = verticalInput * camForwards + horizontalInput * gameCamera.right;
        }
        else
        {
            //calculates the world relative direction for the pirate to move
            movement = verticalInput * Vector3.forward + horizontalInput * Vector3.right;
        }
    }

    private void PirateMove()
    {
        //convert the movement vector from world relative into a local relative 
        //turn amount and forward amount require to move in desired direction
        if(movement.magnitude > 1f) //movement magnitude is over 1 
        {
            //normalize the movement vector
            movement.Normalize();
        }

        movement = transform.InverseTransformDirection(movement); //transforms the movement vector from world space to local space

        //check if Pirate is grounded
        CheckIfGrounded();
        movement = Vector3.ProjectOnPlane(movement, groundPlaneNormal); //project the movement vector onto the ground plane
        turnAmount = Mathf.Atan2(movement.x, movement.z);
        forwardAmount = movement.z;

        ApplyExtraTurnRotation();
 
        //determine which movement method to use depending on whether pirate is grounded or not
        if (grounded)
        {
            
            //use grounded movement method
            rBody.velocity = transform.forward * forwardAmount * speed;
            
            if (onMoving)
            {
                rBody.velocity += movingPlatformVel;
            }

            //check we are moving by acessing the anim
            if(pirateAnim.GetBool("IsMoving"))
            {
                SoundManager.Instance.PlayWalkSound(); //play one of our lovely walk sounds
            }

            Jump();
        }
        else
        {
            //use Air movement method
            if (airControl)
            {
                //changes rigidbody's velocity horizontal speed and direction affecting vertical velocity
                rBody.velocity = new Vector3((transform.forward * forwardAmount * speed).x, rBody.velocity.y, (transform.forward * forwardAmount * speed).z);
            }

            //Allows the pirate to be affected by extra gravitational pull while in the air
            Vector3 extraGravityForce = (Physics.gravity * gravityMultiplyer) - Physics.gravity;
            rBody.AddForce(extraGravityForce);

            //call Jump for double jump
            Jump();
           
        }

        //send input and other animation state parameters to the animator
    }

    private void Jump()
    {
        if(grounded && jumpInput)
        {
            pirateAnim.Play("Jump");
            rBody.velocity = new Vector3(rBody.velocity.x, jumpForce, rBody.velocity.z);
            grounded = false;
            pirateAnim.SetBool("Grounded", false);
            jumpInput = false;
            canDoubleJump = true;

            //play random jump sound
            string num = UnityEngine.Random.Range(2, 6).ToString(); //staring from 2 bc screw jump1
            int vol = 80;
            if (num == "1" || num == "2") //turn down the volume for kevs sounds
            {
                vol = 25;
            }
            SoundManager.Instance.PlaySfx("jump" + num, vol);
        }
        else if(!grounded && canDoubleJump &&jumpInput)
        {
            pirateAnim.Play("Double Jump");
            rBody.velocity = new Vector3(rBody.velocity.x, jumpForce, rBody.velocity.z);
            grounded = false;
            jumpInput = false;
            canDoubleJump = false;

            //play random jump sound
            string num = UnityEngine.Random.Range(2, 6).ToString();
            int vol = 80;
            if (num == "1" || num == "2") //turn down the volume for kevs sounds
            {
                vol = 25;
            }
            SoundManager.Instance.PlaySfx("jump" + num, vol);
        }
    }

    public IEnumerator Stun(float stunTime)
    {
        Debug.Log("Stunned!");
        pirateAnim.Play("Stun");
        stunned = true;
        pirateAnim.SetBool("isStunned", true);
        SoundManager.Instance.PlaySfx("chirpping", 50);
        yield return new WaitForSeconds(stunTime);
        Debug.Log("unStunned!");
        pirateAnim.SetBool("isStunned", false);
        stunned = false;
    }

    public void Respawn()
    {
        transform.position = respawnLocation;
    }

    private void ApplyExtraTurnRotation()
    {
        float turnSpeed = Mathf.Lerp(idleTurnSpeed, movingTurnSpeed, forwardAmount);
        transform.Rotate(0, turnAmount * turnSpeed * Time.deltaTime, 0);
    }

    private void CheckIfGrounded()
    {
        RaycastHit rayHit;

#if UNITY_EDITOR
        //VISUALIZE GROUND CHECK WHEN IN UNITY EDITOR   
        Debug.DrawLine(transform.position + (Vector3.up * 0.1f), transform.position + (Vector3.up * 0.1f) + (Vector3.down * groundedDist),Color.magenta);
        Debug.DrawLine(transform.position + (Vector3.up * 0.1f), transform.position + (Vector3.up * 0.1f) + ((Vector3.down + transform.forward) * groundedDist), Color.magenta);
        Debug.DrawLine(transform.position + (Vector3.up * 0.1f), transform.position + (Vector3.up * 0.1f) + ((Vector3.down + -transform.forward) * groundedDist), Color.magenta);
        Debug.DrawLine(transform.position + (Vector3.up * 0.1f), transform.position + (Vector3.up * 0.1f) + ((Vector3.down + -transform.right) * groundedDist), Color.magenta);
        Debug.DrawLine(transform.position + (Vector3.up * 0.1f), transform.position + (Vector3.up * 0.1f) + ((Vector3.down + transform.right) * groundedDist), Color.magenta);
#endif
        bool groundHit = false;
        if(!Physics.Raycast(transform.position + (Vector3.up * 0.1f), (Vector3.down), out rayHit, groundedDist, groundMask))//if down returns nothing
        {
            //check forward raycast
            if(!Physics.Raycast(transform.position + (Vector3.up * 0.1f),(Vector3.down+transform.forward), out rayHit, groundedDist, groundMask))
            {
                //check backward raycast
                if (!Physics.Raycast(transform.position + (Vector3.up * 0.1f), (Vector3.down + -transform.forward), out rayHit, groundedDist, groundMask))
                {
                    //check Left raycast
                    if (!Physics.Raycast(transform.position + (Vector3.up * 0.1f), (Vector3.down + -transform.right), out rayHit, groundedDist, groundMask))
                    {
                        //check Right raycast
                        if (Physics.Raycast(transform.position + (Vector3.up * 0.1f), (Vector3.down + transform.right), out rayHit, groundedDist, groundMask))
                        {
                            groundHit = true;
                        }
                    }
                    else { groundHit = true; }
                }
                else { groundHit = true; }
            }
            else { groundHit = true; }
        }
        else { groundHit = true; }
        //0.1f is used to offest the raycast from the inside of the pirate model
        //The pirate transform should be at base of the pirate
        if(groundHit) //if raycast hits something
        {
            groundPlaneNormal = rayHit.normal; //set the ground plan normal to the raycast hit normal
            grounded = true; //set the pirate to grounded
            pirateAnim.SetBool("Grounded", true);
            canDoubleJump = false;

            
            if(rayHit.collider.gameObject.tag == "MovingPlatform")
            {
                onMoving = true;
                movingPlatformVel = rayHit.collider.gameObject.GetComponent<Rigidbody>().velocity;
                Debug.Log("Grounded on moving");
            }
            else
            {
                onMoving = false;
                movingPlatformVel = Vector3.zero;
            }

            if(rayHit.collider.gameObject.tag == "RotatingPlatform")
            {
                onRotating = true;
                transform.parent = rayHit.collider.gameObject.transform.parent;
            }
            else
            {
                onRotating = false;
                transform.parent = null;
            }

            //set respawn point
            if (rayHit.collider.gameObject.tag == "IslandPlatform")
            {
                //set the respawn point to be the previous islands center point
                GameObject prevIsland = rayHit.collider.gameObject;
                Vector3 islandLoc = new Vector3(prevIsland.transform.position.x, (prevIsland.GetComponent<MeshCollider>().bounds.size.y / 2) + prevIsland.transform.position.y, prevIsland.transform.position.z);
                //check to make sure backup respawn is not current respawn
                
                respawnLocation = islandLoc;
            }
            else if (rayHit.collider.gameObject.tag == "Terrain")
            {
                respawnLocation = transform.position - (transform.forward.normalized * 1.5f);
            }
        }
        else //raycast did not hit ground
        {
            grounded = false;
            pirateAnim.SetBool("Grounded", false);
            groundPlaneNormal = Vector3.up;

            onRotating = false;
            transform.parent = null;

            onMoving = false;
            movingPlatformVel = Vector3.zero;
        }
    }

    private void Signal()
    {
        if (pirateActive && !GameManager.Instance.SignalOn && Input.GetButtonDown(inputManager.SIGNAL_AXIS))
        {
            signalCor = StartCoroutine(GameManager.Instance.TreasureSignalBeam());
        }
        else if (!pirateActive && GameManager.Instance.SignalOn)
        {
            //GameManager.Instance.SignalOn = false;
            if (signalCor != null)
            {
                StopCoroutine(signalCor);
            }
            //GameManager.Instance.StopSignalBeam();
        }
    }
    #endregion
}