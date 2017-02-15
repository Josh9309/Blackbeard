using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureHunter : BasePirate
{
    #region Attributes
    #endregion
    
    #region Properties
    #endregion
    
    protected override void Start() //Use this for initialization
    {
        base.Start();
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
        if (coll.tag == "Treasure") //If this pirate is within range of the treasure
            Pickup(); //Check for pickup
    }

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
        if (Input.GetButton("Attack")) //If X is pressed
        {
            pirateAnim.Play("PickupTreasure");
        }
    }
    #endregion
}