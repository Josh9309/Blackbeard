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

        treasure = GameObject.FindGameObjectWithTag("Treasure");
        treasureRB = treasure.GetComponent<Rigidbody>();

        treasureSlot = GameObject.FindGameObjectWithTag("Slot").transform;

        canPickup = false;
        pickingUp = false;
        hasTreasure = false;
	}

    protected override void Update() //Update is called once per frame
    {
        if (!pickingUp)
            base.Update();

        if (pirateActive)
            Pickup(canPickup);
    }

    protected override void FixedUpdate() //Physics updates
    {
        if (!pickingUp)
            base.FixedUpdate();
    }

    private void OnTriggerStay(Collider coll)
    {
        if (coll.tag == "Treasure" && pirateActive) //If this pirate is within range of the treasure
            canPickup = true;
    }

    private void OnTriggerExit(Collider coll)
    {
        if (coll.tag == "Treasure" && pirateActive) //If this pirate is within range of the treasure
            canPickup = false;
    }
    #endregion

    #region Methods
    protected override void Dead()
    {
        Debug.Log("Pirate: " + name + "has Died");
        Destroy(gameObject);
    }

    /// <summary>
    /// Let the treasure pirate pick up treasure
    /// </summary>
    private void Pickup(bool yesWeCan)
    {
        //Start pickup
        if (Input.GetButton("Attack") && !pickingUp && yesWeCan)
        {
            pirateAnim.Play("Pickup1");
            pickingUp = true;
        }

        //Picking up treasure
        if (!hasTreasure && yesWeCan)
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