using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrap : BaseTrap {

    //Attributes
    [SerializeField] private float stunTime = 2.0f;

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
        activated = true;
        trapAnim.Play("Activate");
    }

    public override void Deactivate()
    {
        activated = false;
        trapAnim.Play("Deactivate");
    }

    public override void Trigger(GameObject pirate)
    {
        Vector3 knockbackForce = pirate.transform.forward * -100;
        pirate.gameObject.GetComponent<Rigidbody>().AddForce(knockbackForce);
        StartCoroutine(pirate.GetComponent<CaptainPirate>().Stun(stunTime));
    }

    public override void Reset()
    {
        activated = false;
        triggered = false;
        trapAnim.Play("Deactivate");
    }

}
