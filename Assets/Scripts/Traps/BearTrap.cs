using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearTrap : BaseTrap {

    #region Attributes
    [SerializeField]
    private float stunTime;
    private byte activeParrotNum;
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

        if (gm.CurrentPlayer1State == GameManager.PlayerState.PARROT)
            activeParrotNum = 1;
        else if (gm.CurrentPlayer2State == GameManager.PlayerState.PARROT)
            activeParrotNum = 2;
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
        //trapAnim.Play("Activate");
    }

    public override void Trigger(GameObject pirate)
    {
		if (!triggered)
		{
			triggered = true;
			trapAnim.Play ("Activate");

			StartCoroutine (pirate.GetComponent<CaptainPirate> ().Stun (stunTime));
			StartCoroutine (DestroyAfterStun (stunTime + 1));

            //Decrement the bear trap count
            if (activeParrotNum == 1)
                gm.P1BearTrapCount--;
            else
                gm.P2BearTrapCount--;
		}
    }

    protected override void OnTriggerEnter(Collider coll)
    {
        base.OnTriggerEnter(coll);

        if (coll.tag == "Terrain" || coll.tag == "IslandPlatform")
        {
            //GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        }
    }

    //Move the bear trap off screen and wait to destroy it
    //References are needed for coroutines outside of this object
    public IEnumerator DestroyAfterStun(float destroyTime)
    {
		yield return new WaitForSeconds(destroyTime);

        Destroy(gameObject); //Destroy object
    }
}
