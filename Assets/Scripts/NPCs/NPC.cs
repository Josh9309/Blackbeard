using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// abstract class to govern NPCs when they are not under the player's control
/// this class will contain attributes and methods relating to both child classes
/// this class is abstract and has two children, RedNPC and BlueNPC
/// </summary>
public abstract class NPC : MonoBehaviour {

    public enum PirateType { BUCCANEER, HUNTER };

    #region Attributes
    // Components
    protected Rigidbody rb;
    protected NavMeshAgent agent;
    protected FSM fsm;
    protected GameObject squad;
    protected Animator anim;
    protected float radius;

    // states
    protected FSM.State patrol;
    protected FSM.State combat;
    protected FSM.State pickupTreasure;
    protected FSM.State returnTreasure;
    protected bool active = true;
    protected bool rbActive = false;

    // identifications
    protected PirateType type;
    // this variable will represent what team the NPC is on, two options are Red or Blue
    // this cooresponds to the string tag of the agent, to check if an agent is on a team
    // check if their tag equals their team attribute
    protected GameManager.Team team;

    // for seeking using NavMeshAgent
    protected GameObject target;

    // gameplay
    [SerializeField]
    protected int health;
    #endregion

    #region Accessors
    // state accessors for allowing the SquadManager to manage NPC states
    public FSM.State NPCPatrol { get { return patrol; } }
    public FSM.State NPCCombat { get { return combat; } }
    public FSM.State NPCReturnTreasure { get { return returnTreasure; } }
    public FSM.State NPCPickupTreasure { get { return pickupTreasure; } }

    public int Health { get { return health; } }

    // for player to change the state
    public bool Active { set { active = value; } get { return active; } }

    // getters and setters to modify states through the SquadManager
    public GameObject Squad { set { squad = value; } get { return squad; } }

    // for allowing SquadManager to set targets
    public GameObject Target { set { target = value; } get { return target; } }
    #endregion

    // use team accessor to return a string representing NPC's team
    public GameManager.Team getTeam { get { return team; } set { team = value; } }
 
    // Use this for initialization
    protected virtual void Start () {
        // assign components
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        radius = agent.radius;

        // for now, disable animator if this script runs, it breaks movement
        //anim.enabled = false;

        // assign states
        patrol = Patrol;
        combat = Combat;
        returnTreasure = ReturnTreasure;
        pickupTreasure = PickupTreasure;

        // assign team based on tag
        // will pull from SM

        // everything starts in a patrol state
        //fsm.SetState(patrol);

        // for target initialization
        target = null;
	}
	
	// Update is called once per frame
	protected virtual void Update () {
        if (active)
        {
            if (!agent.enabled)
                agent.enabled = true;
            rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
            fsm.UpdateState();
        }
        else
        {
            if (agent.enabled)
                agent.enabled = false;
            rb.constraints = RigidbodyConstraints.None;
            rb.constraints = RigidbodyConstraints.FreezeRotation;
        }
	}

    protected virtual void FixedUpdate()
    {

    }

    #region Helper Methods
    /// <summary>
    /// Helper method for calculating the distance from the NPC's current position
    /// to a given target
    /// </summary>
    /// <param name="targetPos">position of desired target</param>
    /// <returns>distance to targetPos parameter</returns>
    protected Vector3 CalcDistance(Vector3 targetPos)
    {
        return targetPos - transform.position;
    }

    /// <summary>
    /// Tells the agent to seek the given target
    /// </summary>
    protected void Seek(Vector3 seekTarget)
    {
        agent.destination = seekTarget;
    }

    /// <summary>
    /// Method responsible for applying damage, the attacker should be the one applying
    /// the damage
    /// </summary>
    /// <param name="damage">amount of damage to be taken</param>
    public void TakeDamage(int damage)
    {
        health -= damage;
    }
    #endregion

    #region State Methods

    protected abstract void Combat();

    protected abstract void Patrol();

    protected abstract void ReturnTreasure();

    protected abstract void PickupTreasure();

    #endregion
}
