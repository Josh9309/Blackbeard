using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureHunter : BasePirate
{
    #region Attributes
    private bool canPickup, pickingUp; //If the treasure pirate is currently picking anything up
    private bool hasTreasure;
    private GameObject treasure;
    private Transform treasureSlot;
    private Rigidbody treasureRB;
    private RaycastHit hit;
    private int visionAngle;
    #endregion

    #region Properties
    public bool PickingUp
    {
        get
        {
            return pickingUp;
        }
    }
    #endregion

    #region InBuiltMethods
    protected override void Start() //Use this for initialization
    {
        base.Start();
        base.health = 100;

        treasure = GameObject.FindGameObjectWithTag("Treasure");
        treasureRB = treasure.GetComponent<Rigidbody>();

        treasureSlot = transform.FindChild("TreasureSlot");

        canPickup = false;
        pickingUp = false;
        hasTreasure = false;

        visionAngle = 30;
	}

    protected override void Update() //Update is called once per frame
    {
        if (!pickingUp && !hasTreasure)
            base.Update();

        if (pirateActive)
            Pickup();
    }

    protected override void FixedUpdate() //Physics updates
    {
        if (!pickingUp)
            base.FixedUpdate();
    }
    #endregion

    #region Methods
    //protected override void Dead()
    //{
    //    Debug.Log("Pirate: " + name + "has Died");
    //    Destroy(gameObject);
    //}

    /// <summary>
    /// Let the treasure pirate pick up treasure
    /// </summary>
    private void Pickup()
    {
        //TODO: update raycast outside of button press for UI purposes
        //When trying to pick something up, make sure nothing is currently held and the object is within the vision cone
        if (Input.GetButton("Attack") && !hasTreasure && !pickingUp)
        {
            Vector3 direction = treasure.transform.position - transform.position;
            float angle = Vector3.Angle(transform.forward, direction);

            //Raycast to pick up the treasure
            Physics.Raycast(transform.position, direction, out hit);

            if (visionAngle > angle && direction.magnitude < 5 && hit.transform.tag == "Treasure")
                canPickup = true;
        }

        //Start pickup or putdown
        if (!pickingUp && canPickup)
        {
            pirateAnim.Play("Pickup1");
            pickingUp = true;
        }
        else if (!pickingUp && hasTreasure && Input.GetButton("Attack"))
        {
            pirateAnim.Play("Pickup1");
            pickingUp = true;
        }

        //Picking up treasure
        if (!hasTreasure && canPickup)
        {
            if (pirateAnim.GetCurrentAnimatorStateInfo(0).IsName("Pickup2")) //If the animation for picking up an object is halfway played
            {
                treasureRB.useGravity = false;

                //Set the treasure for when it is picked up
                treasure.transform.parent = treasureSlot;
                treasure.transform.localPosition = Vector3.zero;
                treasure.transform.rotation = treasureSlot.transform.rotation;
            }
        }

        //Putting down treasure
        if (hasTreasure)
        {
            if (pirateAnim.GetCurrentAnimatorStateInfo(0).IsName("Pickup2")) //If the animation for picking up an object is halfway played
            {
                treasureRB.useGravity = true;
                treasure.transform.parent = null;
            }
        }

        //Stop picking things up and putting them down
        if (pirateAnim.GetCurrentAnimatorStateInfo(0).IsName("Idle") && pickingUp) //If the object is being picked up
        {
            pickingUp = false;
            canPickup = false;
        }

        //Set whether the treasure is picked up or not
        if (!pickingUp && treasure.transform.parent != null)
        {
            hasTreasure = true;
            speed = 3;
        }
        else if (!pickingUp && treasure.transform.parent == null)
        {
            hasTreasure = false;
            speed = 5;
        }
    }
    #endregion
}