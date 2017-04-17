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
        //base.Start();
	}

    private void Awake()
    {

        trapAnim = gameObject.GetComponent<Animator>();

        triggered = false;

        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        particle = GetComponentInChildren<ParticleSystem>();
        particle.Pause();
    }
	
	// Update is called once per frame
	protected override void Update () {
        base.Update();
	}

    public override void Activate()
    {
        //Debug.Log("Bear trap activated");
        activated = true;
        trapAnim.Play("Deactivate");
    }

    public override void Deactivate()
    {
        activated = false;
        trapAnim.Play("Activate");
    }

    public override void Reset()
    {
        activated = false;
        triggered = false;
        Destroy(this.gameObject); // NOTE: not sure if this is the desired effect, talk it over with team once people wake up
        //trapAnim.Play("Activate");
    }

    public override void Trigger(GameObject pirate)
    {
        triggered = true;
        trapAnim.Play("Activate");
        Debug.Log("Bear trap triggered");
        StartCoroutine(pirate.GetComponent<CaptainPirate>().Stun(stunTime));
        
    }

    protected override void OnTriggerEnter(Collider coll)
    {
        base.OnTriggerEnter(coll);

        if (coll.tag == "Terrain" || coll.tag == "IslandPlatform")
        {
            //GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        }
    }
}
