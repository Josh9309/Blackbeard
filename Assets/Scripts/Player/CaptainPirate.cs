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

    //pirate animation attributes
    private Animator pirateAnim;

    //pirate movement attributes
    private Vector3 movement;
    private Vector3 camForwards;
    private Vector3 groundPlaneNormal;
    private float groundedDist = 0.2f;
    [SerializeField] private float idleTurnSpeed = 360;
    [SerializeField] private float movingTurnSpeed = 180;
    [SerializeField] private float jumpForce = 50;
    private float turnAmount;
    private float forwardAmount;
    private bool grounded;
    private bool isJumping;

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
    #endregion

    #region InBuiltMethods
    // Use this for initialization
    private void Start()
    {
        rBody = GetComponent<Rigidbody>();
        pirateAnim = GetComponent<Animator>();

        //Get stuff from the game manager
        GameManager gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
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

        pirateActive = true;
    }
	
	// Update is called once per frame
    private void Update()
    {
        if (!isJumping && grounded && pirateActive && !stunned)
        {
            isJumping = Input.GetButtonDown(inputManager.JUMP_AXIS);
        }
    }

	private void FixedUpdate ()
    {
        if (pirateActive && !stunned)
        {
            GetMovementInput();
            PirateMove();
        }
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
            Jump();
        }
        else
        {
            //use Air movement method
        }

        //send input and other animation state parameters to the animator
    }

    private void Jump()
    {
        if(grounded && isJumping)
        {
            pirateAnim.Play("Jump");
            rBody.velocity = new Vector3(rBody.velocity.x, jumpForce, rBody.velocity.z);
            grounded = false;
            pirateAnim.SetBool("Grounded", false);
            isJumping = false;
        }
    }

    public IEnumerator Stun(float stunTime)
    {
        Debug.Log("Stunned!");
        stunned = true;
        yield return new WaitForSeconds(stunTime);
        Debug.Log("unStunned!");
        stunned = false;
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
#endif
        //0.1f is used to offest the raycast from the inside of the pirate model
        //The pirate transform should be at base of the pirate
        if(Physics.Raycast(transform.position + (Vector3.up * 0.1f), (Vector3.down), out rayHit, groundedDist)) //if raycast hits something
        {
            groundPlaneNormal = rayHit.normal; //set the ground plan normal to the raycast hit normal
            grounded = true; //set the pirate to grounded
            pirateAnim.SetBool("Grounded", true);
        }
        else //raycast did not hit ground
        {
            grounded = false;
            pirateAnim.SetBool("Grounded", false);
            groundPlaneNormal = Vector3.up;
        }
    }    
    #endregion
}