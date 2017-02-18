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

    // gameplay
    [SerializeField]
    private int attackDam;
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

    #region Helper Methods
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
    #endregion
}
