using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// abstract class to govern NPCs when they are not under the player's control
/// this class will contain attributes and methods relating to both child classes
/// this class is abstract and has two children, MeleeNPC and HunterNPC
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
    protected Transform top;

    // states
    public enum State { PATROL, COMBAT, PICKUP_TREASURE, RETURN_TREASURE, DEFEND_TREASURE, DEAD};
    protected State currentState;
    protected FSM.State patrol;
    protected FSM.State combat;
    protected FSM.State pickupTreasure;
    protected FSM.State returnTreasure;
    protected FSM.State dead;
    protected FSM.State defendTreasure;
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
    protected HealthSynch hpSynch;
    #endregion

    #region Accessors
    // state accessors for allowing the SquadManager to manage NPC states
    public FSM.State NPCPatrol { get { return patrol; } }
    public FSM.State NPCCombat { get { return combat; } }
    public FSM.State NPCReturnTreasure { get { return returnTreasure; } }
    public FSM.State NPCPickupTreasure { get { return pickupTreasure; } }
    public FSM.State NPCDefendTreasure { get { return defendTreasure; } }
    public State CurrentState { get { return currentState; } }

    // return type for identification
    public PirateType Type { get { return type; } }

    public int Health {
        get { return health; }
        set
        {
            if (value >= hpSynch.MaxHealth)
            {
                health = hpSynch.MaxHealth;
            }
            else if (value < 0)
            {
                health = 0;
            }
            else
            {
                health = value;
            }
        }
    }

    // for player to change the state
    public bool Active { get { return active; } }

    // getters and setters to modify states through the SquadManager
    public GameObject Squad { set { squad = value; } get { return squad; } }

    // for allowing SquadManager to set targets
    public GameObject Target { set { target = value; } get { return target; } }

    public Transform Top
    {
        get
        {
            return top;
        }
    }
    #endregion

    // use team accessor to return a string representing NPC's team
    public GameManager.Team getTeam { get { return team; } set { team = value; } }
 
    // Use this for initialization
    protected virtual void Start () {
        //setup health
        hpSynch = GetComponent<HealthSynch>();

        // assign components
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        radius = agent.radius;

        // for now, disable animator if this script runs, it breaks movement
        //anim.enabled = false;

        Transform  []tops = GetComponentsInChildren<Transform>();

        foreach (Transform t in tops)
            if (t.tag == "Top")
                top = t;

        // assign states
        patrol = Patrol;
        combat = Combat;
        returnTreasure = ReturnTreasure;
        pickupTreasure = PickupTreasure;
        defendTreasure = DefendTreasure;
        dead = Dead;

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
            DeadCheck();
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
    /// The Modify method should be used to make any modifications to the pirates health. It can either replace the health, or modify it; and it will update the other
    /// pirate script accordingly
    /// </summary>
    /// <param name="mod">either the new health amount or the amount you want to modify the health by</param>
    /// <param name="replaceHealth">tells method whether to replace the health with new value or just modify it by new value </param>
    public void ModifyHealth(int mod, bool replaceHealth)
    {
        if (!replaceHealth) //check if you are just modifying the health
        {
            health += mod; //Add mod amount to health
            hpSynch.UpdateHealth(false); //tells hpSynch to update the player pirate script
        }
        else //you are replacing the health
        {
            health = mod; //sets health = to new health
            hpSynch.UpdateHealth(false); //tells hpSynch to update the player pirate script
        }
    }

    /// <summary>
    /// Method responsible for setting this NPC to be inactive if the player possesses them
    /// NOTE: use this method for setting things to be inactive, don't directly modify the bool
    /// </summary>
    public abstract void SetInactive();

    /// <summary>
    /// Method responsible for setting this NPC to be active if the player leaves them
    /// NOTE: use this method for setting things to be active, don't directly modify the bool
    /// </summary>
    public abstract void SetActive();

    /// <summary>
    /// Method responsible for checking if the pirate is dead and changing its state if true
    /// </summary>
    private void DeadCheck()
    {
        if (health <= 0) // NOTE: replace health variable when health system is redone
        {
            fsm.SetState(dead);
        }
    }
    #endregion

    #region State Methods
    /// <summary>
    /// Agent will be in this state when SquadManager detects another enemy squad
    /// </summary>
    protected virtual void Combat()
    {
        currentState = State.COMBAT;
    }

    /// <summary>
    /// Agent will be in this state when looking for the treasure
    /// </summary>
    protected virtual void Patrol()
    {
        currentState = State.PATROL;
    }

    /// <summary>
    /// Agent will be in this state when its squad has the treasure
    /// </summary>
    protected virtual void ReturnTreasure()
    {
        currentState = State.RETURN_TREASURE;
    }

    /// <summary>
    /// Agent will be in this state when its squad is currently picking up the treasure
    /// </summary>
    protected virtual void PickupTreasure()
    {
        currentState = State.PICKUP_TREASURE;
    }

    /// <summary>
    /// Agent will be in this state when another squad on its team has the treasure
    /// </summary>
    protected virtual void DefendTreasure()
    {
        currentState = State.DEFEND_TREASURE;
    }

    /// <summary>
    /// Agent will be in this state when its health is less than or equal to 0
    /// </summary>
    protected void Dead()
    {
        currentState = State.DEAD;
        squad.GetComponent<SquadManager>().Remove(this.gameObject, type);
        Destroy(this.gameObject);
    }

    #endregion
}
