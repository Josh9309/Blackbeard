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

    // gameplay
    [SerializeField]
    private int attackDam;
    [SerializeField]
    private float followDist;
    [SerializeField]
    private float combatDist;
    #endregion

    #region Accessors
    public GameObject Leader { set { leader = value; } }
    public List<GameObject> Enemies { set { enemies = value; } }
    #endregion

    // Use this for initialization
    protected override void Start () {
        base.Start();

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
}
