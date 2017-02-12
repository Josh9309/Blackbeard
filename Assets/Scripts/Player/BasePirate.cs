using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Animator))]
public abstract class BasePirate: MonoBehaviour {
    #region Attributes
    //public bool active = false;

    //pirate Stats
    protected int health = 25;
    protected float speed = 5.0f;

    //pirate movement attributes
    private Vector3 movement;
    private Vector3 camForwards;
    private Vector3 groundPlaneNormal;
    private float groundedDist = 0.2f;
    [SerializeField] protected float idleTurnSpeed = 360;
    [SerializeField] protected float movingTurnSpeed = 180;
    [SerializeField] protected float jumpForce = 50;
    private float turnAmount;
    private float forwardAmount;
    protected bool grounded;
    private bool jump;
    
    //input attributes
    [SerializeField] protected float inputDelay = 0.3f;
    private float horizontalInput = 0;
    private float verticalInput = 0;

    //game camera variables
    private Transform gameCamera;
    private Rigidbody rBody;
    #endregion

    #region Properties
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
    #endregion

    // Use this for initialization
    protected virtual void Start () {
        rBody = GetComponent<Rigidbody>();

        //Get all cameras and assign the main camera
        Camera[] camList = FindObjectsOfType<Camera>();
        foreach (Camera c in camList)
        {
            if (c.name.Contains("Pirate"))
                gameCamera = c.transform;
        }
        //if(Camera.main != null) //if there is a main camera
        //{
        //    //get the transform of the main camera
        //    gameCamera = Camera.main.transform;
        //}
        //else
        //{
        //    Debug.LogWarning("PiratePlayer needs a 3rd person camera to move relative to camera. Tag the camera \"MainCamera\"", gameObject);
        //}
    }
	
	// Update is called once per frame
    protected virtual void Update()
    {
            if (!jump && grounded)
            {
                jump = Input.GetButtonDown("Jump");
            }
    }

	protected virtual void FixedUpdate () {
            GetMovementInput();
            PirateMove();
	}

    #region Methods
    private void GetMovementInput()
    {
        //Get inputs for Pirate movement
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        

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
        if(grounded && jump)
        {
            rBody.velocity = new Vector3(rBody.velocity.x, jumpForce, rBody.velocity.z);
            grounded = false;
            jump = false;
        }
    }

    private void ApplyExtraTurnRotation()
    {
        float turnSpeed = Mathf.Lerp(idleTurnSpeed, movingTurnSpeed, forwardAmount);
        transform.Rotate(0, turnAmount * turnSpeed * Time.deltaTime, 0);
    }

    protected void CheckIfGrounded()
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
        }
        else //raycast did not hit ground
        {
            grounded = false;
            groundPlaneNormal = Vector3.up;
        }

    }
    #endregion
}
