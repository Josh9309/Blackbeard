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
 
    #region Attributes
    // Components
    protected Rigidbody rb;
    protected NavMeshAgent agent;

    // this variable will represent what team the NPC is on, two options are Red or Blue
    // this cooresponds to the string tag of the agent, to check if an agent is on a team
    // check if their tag equals their team attribute
    protected string team;

    // for seeking using NavMeshAgent
    protected GameObject target;

    // lsit of NPCs to determine enemies
    // NOTE: this is not effiecient and will be changed in the next release
    protected List<GameObject> enemies;
    protected GameObject squad;
    #endregion

    #region Accessors
    // use team accessor to return a string representing NPC's team
    public string Team { get { return team; } }
    #endregion

    // Use this for initialization
    protected virtual void Start () {
        // assign components
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();

        // assign team based on tag
        team = gameObject.tag;

        // for target initialization
        target = null;
	}
	
	// Update is called once per frame
	protected virtual void Update () {
		if (target == null)
        {
            target = findNearestEnemy();
        }
        else
        {
            Seek();
        }
	}

    /// <summary>
    /// Helper method for calculating the distance from the NPC's current position
    /// to a given target
    /// </summary>
    /// <param name="targetPos">position of desired target</param>
    /// <returns>distance to targetPos parameter</returns>
    protected Vector3 calcDistance(Vector3 targetPos)
    {
        return targetPos - transform.position;
    }

    /// <summary>
    /// Helper method for finding the closest enemy based on the enemies list
    /// </summary>
    /// <returns>closest enemy to this NPC</returns>
    protected GameObject findNearestEnemy()
    {
        GameObject closest = enemies[0];

        for (int i = 0; i < enemies.Count; i++)
        {
            if (calcDistance(enemies[i].transform.position).magnitude < calcDistance(closest.transform.position).magnitude)
            {
                closest = enemies[i];
            }
        }

        return closest;
    }

    /// <summary>
    /// Tells the agent to seek the current target
    /// </summary>
    protected void Seek()
    {
        agent.destination = target.transform.position;
    }
}
