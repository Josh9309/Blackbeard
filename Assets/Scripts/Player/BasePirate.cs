using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]

public abstract class BasePirate: MonoBehaviour
{
    #region Attributes
    public enum PirateType { BUCCANEER, HUNTER };

    //pirate Stats
    [SerializeField] protected int health;
    protected HealthSynch hpSynch;
    protected bool invincible = false;
    protected float speed = 5.0f;
    [SerializeField] protected PirateType pirate;
    protected bool pirateActive; //The pirate will only recieve input if it is active

    //pirate animation attributes
    protected Animator pirateAnim;

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
    private bool isJumping;
    
    //input attributes
    [SerializeField] protected float inputDelay = 0.3f;
    private float horizontalInput = 0;
    private float verticalInput = 0;

    //game camera variables
    private Transform gameCamera;
    private Rigidbody rBody;
    #endregion

    #region Properties
    public int Health
    {
        get { return health; }
        set
        {
            if(value >= hpSynch.MaxHealth)
            {
                health = hpSynch.MaxHealth;
            }
            else if(value < 0)
            {
                health = 0;
            }
            else
            {
                health = value;
            }
        }
    }

    public bool Invincible
    {
        get { return invincible; }
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

    public PirateType Pirate
    {
        get { return pirate; }
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
    #endregion

    #region InBuiltMethods
    // Use this for initialization
    protected virtual void Start()
    {
        rBody = GetComponent<Rigidbody>();
        pirateAnim = GetComponent<Animator>();

        //Get all cameras and assign the main camera
        Camera[] camList = FindObjectsOfType<Camera>();
        foreach (Camera c in camList)
        {
            if (c.name.Contains("Pirate"))
                gameCamera = c.transform;
        }

        hpSynch = GetComponent<HealthSynch>();

        pirateActive = false;
    }
	
	// Update is called once per frame
    protected virtual void Update()
    {
        if (!isJumping && grounded && pirateActive)
        {
            isJumping = Input.GetButtonDown("Jump");
        }
    }

	protected virtual void FixedUpdate ()
    {
        if (pirateActive)
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
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

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
            pirateAnim.SetBool("Grounded", true);
        }
        else //raycast did not hit ground
        {
            grounded = false;
            pirateAnim.SetBool("Grounded", false);
            groundPlaneNormal = Vector3.up;
        }
    }

    /// <summary>
    /// The Modify method should be used to make any modifications to the pirates health. It can either replace the health, or modify it; and it will update the other
    /// pirate script accordingly
    /// </summary>
    /// <param name="mod">either the new health amount or the amount you want to modify the health by</param>
    /// <param name="replaceHealth">tells method whether to replace the health with new value or just modify it by new value </param>
    public void ModifyHealth(int mod, bool replaceHealth)
    {
        if (!replaceHealth) //check if you are just modifying the health
        {
            health += mod; //Add mod amount to health
            hpSynch.UpdateHealth(true); //tells hpSynch to update the ai pirate script
        }
        else //you are replacing the health
        {
            health = mod; //sets health = to new health
            hpSynch.UpdateHealth(true); //tells hpSynch to update the ai pirate script
        }
    }
    

    /// <summary>
    /// Dead will run all the neccessary code for when a pirate dies. Each pirate class must implement a Dead() Method.
    /// </summary>
    //abstract protected void Dead();
    #endregion
}