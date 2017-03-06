using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour {
    #region Attributes
    private bool hit;
    private float knockback = 300000;
    private Buccaneer pirate;
    #endregion

    #region Properties
    public Buccaneer Pirate
    {
        set
        {
            pirate = value;
        }
    }
    #endregion

    // Use this for initialization
    void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        BasePirate enemy = other.gameObject.GetComponent<BasePirate>();

        if (enemy != null && !enemy.Invincible)
        {
            enemy.ModifyHealth(-pirate.AttackDam);
            Debug.Log(pirate.name + " hit " + enemy.name);
            enemy.GetComponent<Rigidbody>().AddForce(pirate.transform.forward * knockback);
        }
    }
}
