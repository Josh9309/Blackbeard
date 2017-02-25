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
    public enum Team { RED, BLUE};

    #region Attributes
    // Components
    protected Rigidbody rb;
    protected NavMeshAgent agent;
    protected FSM fsm;
    protected GameObject squad;
    protected Animator anim;

    // states
    protected FSM.State patrol;
    protected FSM.State combat;
    protected FSM.State returnTreasure;
    protected bool active = true;

    // identifications
    protected PirateType type;
    // this variable will represent what team the NPC is on, two options are Red or Blue
    // this cooresponds to the string tag of the agent, to check if an agent is on a team
    // check if their tag equals their team attribute
    protected Team team;

    // for seeking using NavMeshAgent
    protected GameObject target;

    // gameplay
    [SerializeField]
    protected int health;
    #endregion

    #region Accessors
    // use team accessor to return a string representing NPC's team
    public Team getTeam { get { return team; } }

    // state accessors for allowing the SquadManager to manage NPC states
    public FSM.State NPCPatrol { get { return patrol; } }
    public FSM.State NPCCombat { get { return combat; } }
    public FSM.State NPCReturnTreasure { get { return returnTreasure; } }

    // for player to change the state
    public bool Active { set { active = value; } get { return active; } }

    // getters and setters to modify states through the SquadManager
    public GameObject Squad { set { squad = value; } get { return squad; } }

    // for allowing SquadManager to set targets
    public GameObject Target { set { target = value; } get { return target; } }
    #endregion

    // Use this for initialization
    protected virtual void Start () {
        // assign components
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        fsm = GetComponent<FSM>();
        anim = GetComponent<Animator>();

        // for now, disable animator if this script runs, it breaks movement
        anim.enabled = false;

        // assign states
        patrol = Patrol;
        combat = Combat;
        returnTreasure = ReturnTreasure;

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
            fsm.UpdateState();
        }
        else
        {
            agent.enabled = false;
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
    /// Tells the agent to seek the current target
    /// </summary>
    protected void Seek()
    {
        agent.destination = target.transform.position;
    }
    #endregion

    #region State Methods

    protected abstract void Combat();

    protected abstract void Patrol();

    protected abstract void ReturnTreasure();

    #endregion
}
