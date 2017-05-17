using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Fire is the script controlling a fire when that a lantern spawns
/// </summary>
public class Fire : MonoBehaviour
{
    #region Attributes
    [SerializeField]
    private float duration;
    [SerializeField]
    private float stunTime;
    private bool burntOut = false;
    #endregion

    // Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (burntOut)
            Destroy(gameObject);
	}

    private IEnumerator Burn()
    {
        yield return new WaitForSeconds(duration);
        burntOut = true;
    }

    /// <summary>
    /// Ignite should be called whenever a fire is spawned, it should
    /// take in the gameObject representing the platform this fire will be
    /// a child of
    /// </summary>
    /// <param name="platform">platform that the fire is on</param>
    public void Ignite()
    {
        // TODO: spawn many small fires in the future
        StartCoroutine(Burn());
    }

    public void OnTriggerEnter(Collider coll)
    {
        if (coll.tag == "Pirate")
        {
            //Debug.Log("Pirate hits fire");
            coll.GetComponent<CaptainPirate>().StartCoroutine(coll.GetComponent<CaptainPirate>().Stun(stunTime));

            //if (coll.GetComponent<CaptainPirate>().Grounded)
            //    coll.GetComponent<Rigidbody>().AddForce(-350 * transform.forward); //Knockback to push the pirate out of the fire
        }
    }
}