using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureHunter : BasePirate
{
    #region Attributes
    private bool canPickup, pickingUp; //If the treasure pirate is currently picking anything up
    private GameObject treasure;
    private bool hasTreasure;
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

        //Get the treasure gameobject
        GameObject[] allGO = FindObjectsOfType<GameObject>();
        foreach (GameObject go in allGO)
        {
            if (go.tag == "Treasure")
                treasure = go;
        }

        canPickup = false;
        pickingUp = false;
        hasTreasure = false;
	}

    protected override void Update() //Update is called once per frame
    {
        base.Update();

        if (pirateActive)
            Pickup(canPickup);
    }

    protected override void FixedUpdate() //Physics updates
    {
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
        if (Input.GetButton("Attack") && !pickingUp)
        {
            pirateAnim.Play("Pickup1");
            pickingUp = true;
        }

        //Picking up treasure
        if (!hasTreasure && yesWeCan)
        {
            if (pirateAnim.GetCurrentAnimatorStateInfo(0).IsName("Pickup2")) //If the animation for picking up an object is halfway played
            {
                treasure.transform.parent = gameObject.transform;
                treasure.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 3, gameObject.transform.position.z + 2);
            }
        }
        else if (pirateAnim.GetCurrentAnimatorStateInfo(0).IsName("Idle") && pickingUp) //If the object is being picked up
        {
            pickingUp = false;
        }

        //Putting down treasure
        if (hasTreasure && !yesWeCan)
        {
            if (pirateAnim.GetCurrentAnimatorStateInfo(0).IsName("Pickup2")) //If the animation for picking up an object is halfway played
            {
                treasure.transform.parent = null;
            }
        }
        else if (pirateAnim.GetCurrentAnimatorStateInfo(0).IsName("Idle") && pickingUp) //If the object is being picked up
        {
            pickingUp = false;
        }

        //Set whether the treasure is picked up or not
        if (!pickingUp && treasure.transform.parent != null)
            hasTreasure = true;
        else if (!pickingUp && treasure.transform.parent == null)
            hasTreasure = false;
    }
    #endregion
}