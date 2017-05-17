using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrap : BaseTrap {

    //Attributes
    [SerializeField] private float stunTime = 2.0f;
    //private GameManager gm;
    //private ParticleSystem particle;

	// Use this for initialization
	protected override void Start () {
        base.Start();
       // gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
       // particle = GetComponentInChildren<ParticleSystem>();
       // particle.Pause();
	}
	
	// Update is called once per frame
	protected override void Update () {
        base.Update();
        //if (gm.CurrentPlayer1State == GameManager.PlayerState.PARROT && gm.CurrentPlayer2State == GameManager.PlayerState.PARROT)
        //    particle.Play();
        //else
        //    particle.Pause();
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
        triggered = true;
        Vector3 knockbackForce = pirate.transform.forward * -500;
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