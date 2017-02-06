using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiratePlayer : MonoBehaviour {
    #region Attributes
    public bool active = false;

    //pirate Stats
    protected int health = 25;
    protected float speed = 5.0f;
    protected int attackPower;

    //input attributes
    private float inputDelay = 0.3f;
    private float horizontalInput = 0;
    private float verticalInput = 0;

    private Rigidbody rBody;
    #endregion

    #region Properties
    #endregion

    // Use this for initialization
    void Start () {
        rBody = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    #region Methods
    private void PirateMove()
    {
        //Get inputs for Pirate movement
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
    }
    #endregion
}
