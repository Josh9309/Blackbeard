using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is the Sword based Melee Pirate Class
/// </summary>
public class Buccaneer : BasePirate {
    public enum AttackState { Idle, Attack1, Attack2, Attack3, Block};
    #region Attributes
    [SerializeField] private int attackDam;

    //melee system attributes
    private float canSQTime2 = 0.20f; //can switch queue time for 2nd attack
    private float canSQTime3 = 0.50f; //can switch queue time for 3rd attack
    private bool canQueue2 = false; //can attack 2 Queue
    private bool canQueue3 = false; //can attack 3 Queue
    private AttackState attState = AttackState.Idle; //attack State for buccaneer. set to idle to begin
    
    //sword attributes
    Sword buccaneerSword;
    #endregion

    #region Properties
    public int AttackDam
    {
        get { return attackDam; }
    }

    public AttackState AttState
    {
        get
        {
            return attState;
        }
    }
    #endregion

    // Use this for initialization
    protected override void Start () {
        //call base pirate start
        base.Start();
        //setup Buccaneer Pirate stats
        base.health = 150;
        base.maxHealth = health;

        //get buccanner Sword
        buccaneerSword = transform.FindChild("Sword").GetComponent<Sword>();
        buccaneerSword.Pirate = this; //give sword a reference to this pirate.

        //sets up the animation events for the attack animations
        SetupAttackAnimationEvent(1, "Sword1");
        SetupAttackAnimationEvent(2, "Sword2");
        SetupAttackAnimationEvent(3, "Sword3");
    }

    // Update is called once per frame
    protected override void Update () {
        base.Update();

        if (pirateActive) //Does not inherit, still must be active
        {
            if (attState != AttackState.Block) //make sure player is not currently blocking
            {
                Attack();
            }
            if(attState != AttackState.Attack1 && AttackState.Attack2 != attState && attState != AttackState.Attack3) //make sure player is not currently attacking
            {
                Block(); 
            }
        }
	}

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    #region Methods
    /// <summary>
    /// This method takes in attack input and will setup the next attack based on what attack state the pirate is in.
    /// </summary>
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
            StartCoroutine(CheckCanSwitch(3));
            Debug.Log("Attack 2");
            attState = AttackState.Attack2;
        }
        else if (Input.GetButtonDown("Attack") && attState == AttackState.Idle)
        {
            pirateAnim.Play("SwordAttack1");
            StartCoroutine(CheckCanSwitch(2));
            Debug.Log("Attack 1");
            attState = AttackState.Attack1;
        }

        //if in the idle animation
        AnimatorStateInfo currentState = pirateAnim.GetCurrentAnimatorStateInfo(0);
        if (currentState.IsName("Idle"))
        {
            attState = AttackState.Idle;
            pirateAnim.SetBool("canAttack2", false);
            pirateAnim.SetBool("canAttack3", false);
            buccaneerSword.hit = false;
        }
    }

    /// <summary>
    /// This method will control the blocking system
    /// </summary>
    private void Block()
    {
        if(Input.GetButton("Block") && attState == AttackState.Idle)
        {
            Debug.Log("Block 1");
            pirateAnim.SetBool("isBlocking", true);
            invincible = true;
            attState = AttackState.Block;
        }
        else if(!Input.GetButton("Block") && AttState == AttackState.Block)
        {
            pirateAnim.SetBool("isBlocking", false);
            invincible = false;
            attState = AttackState.Idle;
        }

        //if in the idle animation
        //AnimatorStateInfo currentState = pirateAnim.GetCurrentAnimatorStateInfo(0);
        //if (currentState.IsName("Idle"))
        //{
        //    attState = AttackState.Idle;
        //    pirateAnim.SetBool("isBlocking", false);
        //}
    }

    /// <summary>
    /// The CheckCanSwitch methods is coroutine method that will wait for a certain amount based on the attack number and then once that time
    /// was finish it will set the corresponding canQueue to true.
    /// </summary>
    /// <param name="attackNum"></param>
    /// <returns></returns>
    private IEnumerator CheckCanSwitch(int attackNum)
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

    /// <summary>
    /// Responsible for setting up the animation events for the attack animation.
    /// </summary>
    /// <param name="attackNum"></param>
    /// <param name="animationName"></param>
    private void SetupAttackAnimationEvent(int attackNum, string animationName)
    {
        AnimationClip attackClip = null; //stores the current attack animation to setup
        AnimationEvent endEvent = new AnimationEvent(); //holds the animation event that will run at the end of current attack animation

        //loops through the buccaneers animator's animation clips and finds the correct animation based off of animation name
        for(int i =0; i < pirateAnim.runtimeAnimatorController.animationClips.Length; i++)
        {
            if(pirateAnim.runtimeAnimatorController.animationClips[i].name == animationName)
            {
                attackClip = pirateAnim.runtimeAnimatorController.animationClips[i]; //assigns correct animation clip to attack clip
            }
        }

        //ERROR CASE - If no animation clip was found, will throw an error
        if(attackClip == null)
        {
            Debug.LogError("Could not find Attack animation clip " + animationName + " for event setup");
            return;
        }

        endEvent.intParameter = attackNum; //pass in parameters for attack ending animation event method.
        endEvent.time = attackClip.length; //sets the end Event to trigger at end of attack animation time length
        endEvent.functionName = "ResetAttackClip"; //set the end Event to run ResetAttackClip when triggered.

        attackClip.AddEvent(endEvent); //adds end event to the animation clip
    }

    /// <summary>
    /// Reset attack clip will reset melee system states based of which animation is ending
    /// </summary>
    /// <param name="attackNum"></param>
    private void ResetAttackClip(int attackNum)
    {
        //used to deterine which attack is ending and needs to be reset
        switch (attackNum)
        {
            case 1: //Attack 1 Reset
                if(attState == AttackState.Attack1) //if buccaneer is still in attack state 1
                {
                    attState = AttackState.Idle; //reset attack state to idle
                }
                canQueue2 = false; //sets can queue for attack 2 to false
                break;

            case 2: //Attack 2 Reset
                if(attState == AttackState.Attack2) //if buccaneer is still in attack state 2
                {
                    attState = AttackState.Idle; //reset attack state 2 to idle
                }

                //set both can queues for attack 2 and 3 to false
                canQueue2 = false;
                canQueue3 = false;

                pirateAnim.SetBool("canAttack2", false);
                break;

            case 3: //Attack 3 Reset
                attState = AttackState.Idle; //set attack state to idle
                canQueue3 = false; //set can queue for attack 3 to false;

                pirateAnim.SetBool("canAttack2", false);
                pirateAnim.SetBool("canAttack3", false);
                break;
        }
    }

    private void resetSword()
    {
        buccaneerSword.hit = false;
    }
    //protected override void Dead()
    //{
    //    Debug.Log("Pirate: " + name + "has Died");
    //    Destroy(gameObject);
    //}
    #endregion
}
