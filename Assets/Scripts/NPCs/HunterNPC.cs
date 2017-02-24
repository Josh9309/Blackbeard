using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This represents an NPC who will seek the treasure and attempt to deliver it to the base
/// </summary>
public class HunterNPC : NPC {

    #region Attributes
    // destination for taking the treasure to
    [SerializeField]
    GameObject treasureDestination;
    #endregion

    // Use this for initialization
    protected override void Start () {
        base.Start();

        target = GameObject.FindGameObjectWithTag("Treasure");

        type = PirateType.HUNTER;
	}
	
	// Update is called once per frame
	protected override void Update () {
        base.Update();
	}

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    #region State Methods

    protected override void Combat()
    {
        
    }

    /// <summary>
    /// For the Treasure Hunter pirate this will seek the treasure, if he reaches
    /// the treasure, this will initiate the return treasure state
    /// </summary>
    protected override void Patrol()
    {
        Seek();
    }

    /// <summary>
    /// This method will be called when the treasure is within the squad's radius, triggering
    /// this agent to continue to seek the treasure and play its pickup animation
    /// NOTE: for now, the animation will not play
    /// </summary>
    private void PickupTreasure()
    {
        Seek();
    }

    protected override void ReturnTreasure()
    {
        //target = treasureDestination;
        Seek();
    }
    #endregion
}
