using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parrot : MonoBehaviour {
    #region Attributes
    private int health = 10;
    [SerializeField] private float speed = 2.0f;
    [SerializeField] private float turnSpeed = 2.0f;
    private float currentHeight;
    
    private Rigidbody rBody;

    //input stuff
    private float inputDelay = 0.3f;
    private float horizontalInput = 0;
    private float verticalInput = 0;
    private float camXInput = 0;
    private float camYInput = 0;
    #endregion

    #region Properties
    #endregion

    // Use this for initialization
    void Start () {
        rBody = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        ParrotMove();
        
	}

    #region Methods
    private void ParrotMove()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        camXInput = Input.GetAxis("Cam_Horizontal");
        //moves parrot constantly forward
        rBody.velocity = transform.forward * speed;
        
        if(Mathf.Abs(horizontalInput) > inputDelay)
        {
            //if input is greater than 0;
            if(horizontalInput > 0)
            {
                transform.Rotate(new Vector3(0, turnSpeed, 0));
            }
            else if(horizontalInput < 0)
            {
                transform.Rotate(new Vector3(0, -turnSpeed, 0));
            }
        }

        if(Mathf.Abs(verticalInput) > inputDelay)
        {
            if(verticalInput > 0)
            {
                transform.Rotate(new Vector3(turnSpeed, 0, 0));
            }
            else if(verticalInput < 0)
            {
                transform.Rotate(new Vector3(-turnSpeed, 0, 0));
            }
        }
        print(rBody.velocity);

        if(Mathf.Abs(camXInput) > inputDelay)
        {
            if (camXInput > 0)
            {
                transform.Rotate(new Vector3(0, 0, turnSpeed));
            }
            else
            {
                transform.Rotate(new Vector3(0, 0, -turnSpeed));
            }
        }
        
    }
    #endregion
}
