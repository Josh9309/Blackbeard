using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// class to govern the behaviour of NPC's when they are not under the player's control
/// this class is abstract and has two children, RedPirate and BluePirate
/// </summary>
public abstract class NPC : MonoBehaviour {
    // for use in identifying what team the pirate is on
    protected string team;

    // all components of the pirate
    protected Rigidbody rb;

    // accessors

	// Use this for initialization
	protected virtual void Start () {
        rb = this.GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	protected virtual void Update () {
		
	}
}
