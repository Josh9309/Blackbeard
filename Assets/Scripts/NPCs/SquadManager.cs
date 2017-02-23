using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class represents the manager for a group of pirates, it will be
/// responsible for delegating behaviors to them based on circumstances that
/// it perceives in the game world, as well as managing itself by maintaining 
/// its full strength
/// </summary>
public class SquadManager : MonoBehaviour {

    #region Attributes
    // the treasure
    [SerializeField]
    private GameObject treasure;

    // states
    FSM.State patrol;
    FSM.State combat;
    FSM.State returnTreasure;
    FSM.State pickupTreasure;

    // variables related to pirates in squad
    private List<GameObject> pirates;
    [SerializeField]
    private int maxPirates;

    // components and prefabs
    [SerializeField]
    private GameObject treasureNPC;
    [SerializeField]
    private GameObject meleeNPC;
    private FSM fsm;

    // radii for spawning, engagement, etc..
    [SerializeField]
    private float squadRadius;
    [SerializeField]
    private float engagementRadius;
    [SerializeField]
    private float initialSpawnRadius;

    #endregion

    // Use this for initialization
    void Start () {
        // initialize variables
        fsm = this.GetOrAddComponent<FSM>();
        pirates = new List<GameObject>();

        // initialize states
        patrol = Patrol;
        combat = Combat;
        pickupTreasure = PickupTreasure;
        returnTreasure = ReturnTreasure;

        // test treasure pirate spawns
        // NOTE: in future versions each squad should have one treasure pirate,
        // this is simply for testing purposes
        for (int i = 0; i < maxPirates; i++)
        {
            //Vector3 pos = (transform.position + Quaternion.AngleAxis(spawnAngle, transform.up) * transform.forward) * 10;

            Vector3 pos = new Vector3(Random.Range(-initialSpawnRadius, initialSpawnRadius), transform.position.y, Random.Range(-initialSpawnRadius, initialSpawnRadius));

            pirates.Add(GameObject.Instantiate(treasureNPC, pos, Quaternion.identity));

        }       
	}
	
	// Update is called once per frame
	void Update () {
        CalcCentroid();

        //fsm.Update();
	}

    #region Helper Methods
    /// <summary>
    /// Helper method for calculating the distance between two objects
    /// </summary>
    /// <param name="targetPos">position of desired target</param>
    /// <returns>distance to targetPos parameter</returns>
    private Vector3 CalcDistance(Vector3 targetPos, Vector3 destPos)
    {
        return destPos - targetPos;
    }

    /// <summary>
    /// Method to calulate the center of the flock. The SquadManager itself will occupy this spot
    /// </summary>
    private void CalcCentroid()
    {
        float totalX = 0;
        float totalY = 1f; // hardcode y value
        float totalZ = 0;
        
        for (int i = 0; i < pirates.Count; i++)
        {
            totalX += pirates[i].transform.position.x;
            totalZ += pirates[i].transform.position.z;
        }

        transform.position = new Vector3(totalX / pirates.Count, totalY, totalZ / pirates.Count);   
    }
    #endregion

    // All transitions are handled by the state PRIOR to the transistion
    // ex: Patrol() is what is responsible for calling the transistion to Combat()
    #region State Methods
    private void Combat()
    {

    }

    /// <summary>
    /// Initial state of the squad, when no enemies are in sight and the treasure
    /// is not in the squad's possession then the squad should be in this state
    /// </summary>
    private void Patrol()
    {

    }

    private void PickupTreasure()
    {

    }

    private void ReturnTreasure()
    {

    }
    #endregion
}
