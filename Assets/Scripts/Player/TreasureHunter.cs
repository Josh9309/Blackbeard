using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureHunter : BasePirate
{
    #region Attributes
    private bool pickingUp; //If the treasure pirate is currently picking anything up
    private GameObject treasure;
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
	}

    protected override void Update() //Update is called once per frame
    {
        base.Update();
	}

    protected override void FixedUpdate() //Physics updates
    {
        base.FixedUpdate();
    }

    private void OnTriggerStay(Collider coll)
    {
        if (coll.tag == "Treasure" && pirateActive) //If this pirate is within range of the treasure and tries to pick it up
            Pickup(); //Check for pickup
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
    private void Pickup()
    {
        if (Input.GetButton("Attack"))
        {
            pirateAnim.Play("PickupTreasure1");
            pirateAnim.SetTime(0); //Reset the animation timer
        }

        if (pirateAnim.GetCurrentAnimatorStateInfo(0).IsName("PickupTreasure2")) //If the animation for picking up an object is halfway played
        {
            treasure.transform.position = gameObject.transform.position;
        }
    }
    #endregion
}