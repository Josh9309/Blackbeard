using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This represents an NPC who will seek the treasure and attempt to deliver it to the base
/// </summary>
public class HunterNPC : NPC {

    #region Attributes
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
    /// For the Treasure Hunter pirate this will seek the treasure
    /// </summary>
    protected override void Patrol()
    {
        Seek();
    }
    #endregion
}
