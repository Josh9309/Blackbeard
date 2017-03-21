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
    public GameObject treasureDestination;

    // for picking up treasure
    private Transform treasureSlot;
    private Rigidbody treasureRB;

    // for checking in SquadManager if the treasure has been picked up
    bool hasTreasure = false;
    #endregion

    public bool HasTreasure { get { return hasTreasure; } }

    // Use this for initialization
    protected override void Start () {
        base.Start();

        fsm = GetComponent<FSM>();
        target = GameObject.FindGameObjectWithTag("Treasure");
        treasureSlot = transform.FindChild("TreasureSlot");
        treasureRB = target.GetComponent<Rigidbody>();

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
        base.Combat();
        agent.destination = transform.position;
    }

    /// <summary>
    /// For the Treasure Hunter pirate this will seek the treasure, if he reaches
    /// the treasure, this will initiate the return treasure state
    /// </summary>
    protected override void Patrol()
    {
        base.Patrol();
        Seek(target.transform.position);

        agent.speed = 5f;
    }

    /// <summary>
    /// This method will be called when the treasure is within the squad's radius, triggering
    /// this agent to continue to seek the treasure and play its pickup animation
    /// </summary>
    protected override void PickupTreasure()
    {
        base.PickupTreasure();
        anim.Play("Pickup1");
        anim.SetTime(0);
        if (!hasTreasure)
        {
            treasureRB.useGravity = false;
            treasureRB.transform.rotation = treasureSlot.transform.rotation;
            treasureRB.constraints = RigidbodyConstraints.FreezeAll;
            target.transform.parent = treasureSlot;
            target.transform.localPosition = Vector3.zero;
        }

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            hasTreasure = true;
        }
    }

    protected override void ReturnTreasure()
    {
        base.ReturnTreasure();
        //target = treasureDestination;
        Seek(target.transform.position);

        agent.speed = 3f;
    }

    protected override void DefendTreasure()
    {
        base.DefendTreasure();
        Seek(target.transform.position);
        agent.speed = 5f;
    }

    #endregion
}
