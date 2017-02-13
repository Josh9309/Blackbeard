using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is the Sword based Melee Pirate Class
/// </summary>
public class Buccaneer : BasePirate {
    #region Attributes
    [SerializeField] private int attackDam;

    //sword attributes
    Sword buccaneerSword;
    #endregion

    #region Properties
    public int AttackDam
    {
        get { return attackDam; }
    }
    #endregion

    // Use this for initialization
    protected override void Start () {
        //call base pirate start
        base.Start();
        //setup Buccaneer Pirate stats
        base.health = 150;

        //get buccanner Sword
        buccaneerSword = transform.FindChild("Sword").GetComponent<Sword>();
        buccaneerSword.Pirate = this; //give sword a reference to this pirate.
	}

    // Update is called once per frame
    protected override void Update () {
        base.Update();

        Attack();
	}

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    #region Methods
    private void Attack()
    {
        if (Input.GetButtonDown("Attack"))
        {
            base.pirateAnim.Play("SwordAttack1");
        }
    }

    protected override void Dead()
    {
        Debug.Log("Pirate: " + name + "has Died");
        Destroy(gameObject);
    }
    #endregion
}
