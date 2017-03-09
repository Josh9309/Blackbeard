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
    private bool canAttack = false;

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

    private IEnumerator AttemptAttack()
    {
        yield return new WaitForSeconds(meleeCooldown);
        combatFSM.SetState(attack);
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

    #region State Methods
    protected override void Combat()
    {
        agent.radius = combatDist;

        if (FindNearestEnemy().GetComponent<MeleeNPC>() != null)
        {
            target = FindNearestEnemy();
        }

        Seek(target.transform.position);

        // if they are fighting another NPC
        if ((CalcDistance(target.transform.position).magnitude <= combatDist + .05) && target.GetComponent<MeleeNPC>().Active == true)
        {
            
        }

        if ((CalcDistance(target.transform.position).magnitude <= combatDist + .05) && target.GetComponent<MeleeNPC>().Active != true)
        {
            combatFSM.UpdateState();
        }
    }
    protected override void Patrol()
    {
        FollowLeader();
    }

    protected override void ReturnTreasure()
    {
        FollowLeader();
    }

    protected override void PickupTreasure()
    {
        FollowLeader();
    }
    #endregion

    #region Combat State Methods
    private void Attack()
    {
        anim.Play("SwordAttack1");
        combatFSM.SetState(idle);
    }

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

    private void Defend()
    {
        anim.SetBool("isBlocking", true);
        StartCoroutine(Block());
    }
    #endregion
}
