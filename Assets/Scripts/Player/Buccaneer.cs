using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is the Sword based Melee Pirate Class
/// </summary>
public class Buccaneer : BasePirate {
    #region Attributes
    [SerializeField] private int attackDam;

    //melee system attributes
    private float canSQTime2 = 0.20f; //can switch queue time for 2nd attack
    private float canSQTime3 = 0.75f;
    private bool canQueue2 = false;
    private bool canQueue3 = false;
    private bool canSwitch2 = false;
    private bool canSwitch3 = false;

    //sword attributes
    Sword buccaneerSword;
    private bool attack1End;
    private bool attacking = false;
    #endregion

    #region Properties
    public int AttackDam
    {
        get { return attackDam; }
    }

    public bool Attacking
    {
        get
        {
            return attacking;
        }
    }
    #endregion

    // Use this for initialization
    protected override void Start () {
        //call base pirate start
        base.Start();
        //setup Buccaneer Pirate stats
        health = 150;
        maxHealth = health;

        //get buccanner Sword
        buccaneerSword = transform.FindChild("Sword").GetComponent<Sword>();
        buccaneerSword.Pirate = this; //give sword a reference to this pirate.

        SetupAttackAnimationEvent(1, 0.75f, "Sword1");
        SetupAttackAnimationEvent(2, 0f, "Sword2");
    }

    // Update is called once per frame
    protected override void Update () {
        base.Update();

        if (pirateActive) //Does not inherit, still must be active
            Attack();
	}

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    #region Methods
    private void Attack()
    {
        if (Input.GetButtonDown("Attack") && !attacking )
        {
            pirateAnim.Play("SwordAttack1");
            Debug.Log("Attack 1");
            attacking = true;
            StartCoroutine(CheckAttackInput(2));
        }
        else if(Input.GetButtonDown("Attack") && canQueue2)
        {
            pirateAnim.SetBool("canAttack2", true);
            attacking = true;
            Debug.Log("Attack 2");
        }
        else if(Input.GetButtonDown("Attack") && canQueue3)
        {
            Debug.Log("Attack 3");
            //attacking = true;
        }
    }

    private IEnumerator CheckAttackInput(int attackNum)
    {
        switch (attackNum)
        {
            case 2:
                yield return new WaitForSeconds(canSQTime2);
                Debug.Log("Attack2 can queue");
                canQueue2 = true;
                break;

            case 3:
                yield return new WaitForSeconds(canSQTime3);
                canQueue3 = true;
                break;
        }
    }

    private void SetupAttackAnimationEvent(int attackNum, float canSwitchTime, string animationName)
    {
        AnimationClip attackClip = null;
        AnimationEvent switchEvent = new AnimationEvent();
        AnimationEvent endEvent = new AnimationEvent();

        for(int i =0; i < pirateAnim.runtimeAnimatorController.animationClips.Length; i++)
        {
            if(pirateAnim.runtimeAnimatorController.animationClips[i].name == animationName)
            {
                attackClip = pirateAnim.runtimeAnimatorController.animationClips[i];
            }
        }

        if(attackClip == null)
        {
            Debug.LogError("Could not find Attack animation clip " + animationName + " for event setup");
            return;
        }

        if (attackNum != 2)
        {
            switchEvent.intParameter = attackNum;
            switchEvent.time = canSwitchTime;
            switchEvent.functionName = "SetAttackCanSwitch";

            attackClip.AddEvent(switchEvent);
        }

        endEvent.intParameter = attackNum;
        endEvent.time = attackClip.length;
        endEvent.functionName = "ResetAttackClip";

        attackClip.AddEvent(endEvent);
    }

    private void SetAttackCanSwitch(int attackNum)
    {
        switch (attackNum)
        {
            case 1:
                canSwitch2 = true;
                break;

            case 2:
                canSwitch3 = true;
                break;
        }
    } 

    private void ResetAttackClip(int attackNum)
    {
        switch (attackNum)
        {
            case 1:
                canSwitch2 = false;
                canQueue2 = false;
                break;

            case 2:
                canSwitch3 = false;
                canQueue3 = false;
                pirateAnim.SetBool("canAttack2", false);
                break;
        }

        attacking = false;
    }
    protected override void Dead()
    {
        Debug.Log("Pirate: " + name + "has Died");
        Destroy(gameObject);
    }
    #endregion
}
