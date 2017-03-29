using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearTrap : BaseTrap {

    #region Attributes
    [SerializeField]
    private float stunTime;
    #endregion

    // Use this for initialization
    protected override void Start () {
        base.Start();
	}
	
	// Update is called once per frame
	protected override void Update () {
        base.Update();
	}

    public override void Activate()
    {
        Debug.Log("Bear trap activated");
        activated = true;       
    }

    public override void Deactivate()
    {
        activated = false;
        trapAnim.Play("Deactivate");
    }

    public override void Reset()
    {
        activated = false;
        triggered = false;
        trapAnim.Play("Deactivate");
    }

    public override void Trigger(GameObject pirate)
    {
        triggered = true;
        trapAnim.Play("Activate");
        Debug.Log("Bear trap triggered");
        StartCoroutine(pirate.GetComponent<CaptainPirate>().Stun(stunTime));
    }
}
