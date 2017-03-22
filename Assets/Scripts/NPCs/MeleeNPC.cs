using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This will represent an NPC capable of attacking and damaging enemies and the player
/// </summary>
public class MeleeNPC : NPC {

    #region Attributes
    // lsit of NPCs to determine enemies, this will be assigned through SquadManager
    protected List<GameObject> enemies;
    private GameObject leader;

    // FSM for individual combat
    FSM [] fsms;
    FSM combatFSM;

    // states
    FSM.State attack;
    FSM.State defend;
    FSM.State idle;

    // gameplay
    [SerializeField]
    private float followDist;
    [SerializeField]
    private float combatDist;
    private bool canAttack = true;
    private bool attackNow = false;
    System.Random rand;

    // combat skills
    [SerializeField]
    private int meleeAttack;
    [SerializeField]
    private int meleeDefense;
    [SerializeField]
    private int attackDam;
    [SerializeField]
    private float meleeCooldown;
    [SerializeField]
    private float blockTime;
    #endregion

    #region Accessors
    public GameObject Leader { set { leader = value; } }
    public List<GameObject> Enemies { set { enemies = value; } get { return enemies; } }
    #endregion

    // Use this for initialization
    protected override void Start () {
        base.Start();

        rand = new System.Random();
        fsms = GetComponents<FSM>();
        fsm = fsms[0];
        combatFSM = fsms[1];

        // declare states
        attack = Attack;
        defend = Defend;
        idle = Idle;
        combatFSM.SetState(idle);

        type = PirateType.BUCCANEER;
	}
	
	// Update is called once per frame
	protected override void Update () {
        base.Update();
	}

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    #region Helper Methods
    /// <summary>
    /// Helper method for finding the closest enemy based on the enemies list
    /// </summary>
    /// <returns>closest enemy to this NPC</returns>
    private GameObject FindNearestEnemy()
    {
        GameObject closest = enemies[0];

        for (int i = 0; i < enemies.Count; i++)
        {
            if (CalcDistance(enemies[i].transform.position).magnitude < CalcDistance(closest.transform.position).magnitude)
            {
                closest = enemies[i];
            }
        }

        return closest;
    }

    public override void SetInactive()
    {
        squad.GetComponent<SquadManager>().Remove(this.gameObject, PirateType.BUCCANEER);
        active = false;
    }

    public override void SetActive()
    {
        squad.GetComponent<SquadManager>().Add(this.gameObject, PirateType.BUCCANEER);
        active = true;
    }

    private IEnumerator AttemptAttack()
    {
        yield return new WaitForSeconds(meleeCooldown);
        combatFSM.SetState(attack);
    }

    private IEnumerator AttemptNPCAttack()
    {
        canAttack = false;
        yield return new WaitForSeconds(meleeCooldown);
        attackNow = true;
        canAttack = true;
        Debug.Log(name + " Attempts an attack!");
    }

    private IEnumerator Block()
    {
        yield return new WaitForSeconds(blockTime);
        combatFSM.SetState(idle);
    }

    /// <summary>
    /// This method is responsible for allowing the melee NPC to follow the Treasure Pirate
    /// </summary>
    private void FollowLeader()
    {
        Seek(leader.transform.position - leader.transform.forward * (-1 * followDist));
    }

    /// <summary>
    /// this method will be called any time this agent is being attacked by another
    /// agent 
    /// </summary>
    /// <returns>returns whether or not the attempted attack was successful</returns>
    public bool NPCDefense(GameObject attacker)
    {
        if (attacker.GetInstanceID() == target.GetInstanceID())
        {
            Debug.Log(team + " pirate is defending");
            if (rand.Next(0, 100) < meleeDefense)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        else
        {
            return false;
        }
    }
    #endregion

    #region State Methods
    protected override void Combat()
    {
        base.Combat();

        agent.radius = combatDist;

        target = FindNearestEnemy();

        Seek(target.transform.position);

        // if they are fighting another NPC
        if ((CalcDistance(target.transform.position).magnitude <= combatDist + 4) && target.GetComponent<NPC>().Active == true)
        {
            if (canAttack && attackNow == false)
                StartCoroutine(AttemptNPCAttack());
            if (rand.Next(0, 100) < meleeAttack && attackNow == true)
            {
                Debug.Log(name + "'s attack has hit!");

                // if target is a treasure pirate, apply damage, otherwise roll for defense
                if (target.GetComponent<NPC>().Type == NPC.PirateType.HUNTER)
                {
                    target.GetComponent<HunterNPC>().ModifyHealth(-attackDam, false);
                }
                else
                {
                    if (target.GetComponent<MeleeNPC>().NPCDefense(this.gameObject))
                    {
                        target.GetComponent<MeleeNPC>().ModifyHealth(-attackDam, false);
                        Debug.Log(target.name + "'s defense has failed!");
                    }
                }
            }
            attackNow = false;
        }

        // if fighting against a player
        if ((CalcDistance(target.transform.position).magnitude <= combatDist + 0.5) && target.GetComponent<MeleeNPC>().Active != true)
        {
            combatFSM.UpdateState();
        }
    }

    protected override void Patrol()
    {
        base.Patrol();
        FollowLeader();
    }

    protected override void ReturnTreasure()
    {
        base.ReturnTreasure();
        FollowLeader();
    }

    protected override void PickupTreasure()
    {
        base.PickupTreasure();
        FollowLeader();
    }

    protected override void DefendTreasure()
    {
        base.DefendTreasure();
        FollowLeader();
    }

    #endregion

    #region Combat State Methods

    /// <summary>
    /// Responsible for playing the animation that will swing the sword and do damage
    /// </summary>
    private void Attack()
    {
        anim.Play("SwordAttack1");
        combatFSM.SetState(idle);
    }

    /// <summary>
    /// The idle animation is the base state for player vs. AI combat
    /// </summary>
    private void Idle()
    {
        switch (target.GetComponent<Buccaneer>().AttState)
        {
            case Buccaneer.AttackState.Idle:
                StartCoroutine(AttemptAttack());
                break;
            case Buccaneer.AttackState.Attack1:
                if (UnityEngine.Random.Range(0, 100) < meleeDefense)
                {
                    combatFSM.SetState(defend);
                }
                break;
            case Buccaneer.AttackState.Attack2:
                if (UnityEngine.Random.Range(0, 100) < meleeDefense)
                {
                    combatFSM.SetState(defend);
                }
                break;
            case Buccaneer.AttackState.Attack3:
                if (UnityEngine.Random.Range(0, 100) < meleeDefense)
                {
                    combatFSM.SetState(defend);
                }
                break;
            case Buccaneer.AttackState.Block:
                if (UnityEngine.Random.Range(0, 100) < meleeAttack)
                {
                    StartCoroutine(AttemptAttack());
                }
                break;

        }
        Seek(target.transform.position);
    }

    /// <summary>
    /// Defend will have a probability of playing when a player attacks the NPC
    /// </summary>
    private void Defend()
    {
        anim.SetBool("isBlocking", true);
        StartCoroutine(Block());
    }
    #endregion
}
