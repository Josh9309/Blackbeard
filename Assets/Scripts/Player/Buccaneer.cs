using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is the Sword based Melee Pirate Class
/// </summary>
public class Buccaneer : BasePirate {
    public enum AttackState { Idle, Attack1, Attack2, Attack3};
    #region Attributes
    [SerializeField] private int attackDam;

    //melee system attributes
    private float canSQTime2 = 0.20f; //can switch queue time for 2nd attack
    private float canSQTime3 = 0.50f;
    private bool canQueue2 = false;
    private bool canQueue3 = false;
    //private bool canSwitch2 = false;
    //private bool canSwitch3 = false;
    private AttackState attState = AttackState.Idle;

    //sword attributes
    Sword buccaneerSword;
    //private bool attack1End;
    private bool attacking = false; //TODO: remove this after updating
    //private bool attacking2 = false;
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
        SetupAttackAnimationEvent(2, 0.86f, "Sword2");
        SetupAttackAnimationEvent(3, 0f, "Sword3");
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
        if (Input.GetButtonDown("Attack") && canQueue3 && attState == AttackState.Attack2)
        {
            Debug.Log("Attack 3");
            pirateAnim.SetBool("canAttack3", true);
            attState = AttackState.Attack3;
        }
        else if(Input.GetButtonDown("Attack") && canQueue2 && attState == AttackState.Attack1)
        {
            pirateAnim.SetBool("canAttack2", true);
            StartCoroutine(CheckAttackInput(3));
            Debug.Log("Attack 2");
            attState = AttackState.Attack2;
        }
        else if (Input.GetButtonDown("Attack") && attState == AttackState.Idle)
        {
            pirateAnim.Play("SwordAttack1");
            StartCoroutine(CheckAttackInput(2));
            Debug.Log("Attack 1");
            attState = AttackState.Attack1;
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
                Debug.Log("Attack 3 can queue");
                canQueue3 = true;
                break;
        }
    }

    private void SetupAttackAnimationEvent(int attackNum, float canSwitchTime, string animationName)
    {
        AnimationClip attackClip = null;
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

        endEvent.intParameter = attackNum;
        Debug.Log(attackNum);
        endEvent.time = attackClip.length;
        endEvent.functionName = "ResetAttackClip";

        attackClip.AddEvent(endEvent);
    }

    private void ResetAttackClip(int attackNum)
    {
        switch (attackNum)
        {
            case 1:
                if(attState == AttackState.Attack1)
                {
                    attState = AttackState.Idle;
                }
                canQueue2 = false;
                break;

            case 2:
               if(attState == AttackState.Attack2)
                {
                    attState = AttackState.Idle;
                }
                canQueue2 = false;
                canQueue3 = false;
                break;

            case 3:
                attState = AttackState.Idle;
                canQueue3 = false;
                Debug.Log("attatack 3jkdaf");
                break;
        }
    }
    protected override void Dead()
    {
        Debug.Log("Pirate: " + name + "has Died");
        Destroy(gameObject);
    }
    #endregion
}
