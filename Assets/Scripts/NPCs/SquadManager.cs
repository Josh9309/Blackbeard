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
    // int values for parsing through statess
    private const int PATROL_ID = 0;
    private const int COMBAT_ID = 1;
    private const int PICKUP_TREASURE_ID = 2;
    private const int RETURN_TREASURE_ID = 3;

    // the way that unity intanstiates objects is stupid so this variable is necessary
    // for making sure an initial state is assigned to the squad
    bool firstRun = true;

    // indentifiers
    [SerializeField]
    private GameManager.Team team;

    // the treasure
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
    private GameObject treasureHunter;

    // components and prefabs
    [SerializeField]
    private GameObject treasureNPC;
    [SerializeField]
    private GameObject meleeNPC;
    //[SerializeField]
    private GameObject treasureDestination;
    private FSM fsm;

    // radii for spawning, engagement, etc..
    [SerializeField]
    private float squadRadius;
    [SerializeField]
    private float engagementRadius;
    [SerializeField]
    private float initialSpawnRadius;

    // flocking
    Vector3 direction;

    #endregion

    #region Accessors
    // use team accessor to return a string representing NPC's team
    public GameManager.Team getTeam { get { return team; } }
    #endregion

    // Use this for initialization
    void Start () {
        // initialize variables
        fsm = GetComponent<FSM>();
        pirates = new List<GameObject>();
        treasure = GameObject.FindGameObjectWithTag("Treasure");
        treasureDestination = GameObject.FindGameObjectWithTag("TreasureDestination");
        direction = new Vector3(0, 0, 0);

        // initialize states
        patrol = Patrol;
        combat = Combat;
        pickupTreasure = PickupTreasure;
        returnTreasure = ReturnTreasure;

        // spawn a single treasure Hunter
        treasureHunter = Instantiate(treasureNPC, transform.position, Quaternion.identity);
        pirates.Add(treasureHunter);
        treasureHunter.GetComponent<NPC>().Squad = this.gameObject;
        treasureHunter.GetComponent<NPC>().getTeam = team;

        // spawn melee pirates
        for (int i = 1; i <= maxPirates; i++)
        {
            //Vector3 pos = (transform.position + Quaternion.AngleAxis(spawnAngle, transform.up) * transform.forward) * 10;

            Vector3 pos = new Vector3(Random.Range(-initialSpawnRadius, initialSpawnRadius) + transform.position.x, transform.position.y, Random.Range(-initialSpawnRadius, initialSpawnRadius) + transform.position.z);

            pirates.Add(GameObject.Instantiate(meleeNPC, pos, Quaternion.identity));
            pirates[i].GetComponent<NPC>().Squad = this.gameObject;
            pirates[i].GetComponent<NPC>().getTeam = team;
            pirates[i].GetComponent<MeleeNPC>().Leader= treasureHunter;

        }

        // set initial state
        fsm.SetState(patrol);
    }
	
	// Update is called once per frame
	void Update () {
        CalcCentroid();
        CalcDirection();

        fsm.UpdateState();
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

        Debug.Log(totalX + " " + totalZ);

        transform.position = new Vector3(totalX / pirates.Count, totalY, totalZ / pirates.Count);   
    }

    private void CalcDirection()
    {
        Vector3 totalForward = new Vector3(0, 0, 0);

        for (int i = 0; i < pirates.Count; i++)
        {
            totalForward = pirates[i].transform.forward + totalForward;
        }

        direction = totalForward / pirates.Count;
        direction.Normalize();

        // set the game manager's position to that of the centroid
        this.transform.forward = direction;
    }

    /// <summary>
    /// For use by SquadManager only, this will set the state of the entire squad
    /// </summary>
    /// <param name="stateID">constant int identifier of the state</param>
    /// <param name="squad">list of squad members</param>
    public void SetSquadState(int stateID)
    {
        for (int i = 0; i < pirates.Count; i++)
        {
            FSM pirateFsm = pirates[i].GetComponent<FSM>();
            NPC npc = pirates[i].GetComponent<NPC>();

            switch(stateID)
            {
                case PATROL_ID:
                    pirateFsm.SetState(npc.NPCPatrol);
                    break;
                case COMBAT_ID:
                    pirateFsm.SetState(npc.NPCCombat);
                    break;
                case PICKUP_TREASURE_ID:
                    pirateFsm.SetState(npc.NPCPickupTreasure);
                    break;
                case RETURN_TREASURE_ID:
                    pirateFsm.SetState(npc.NPCReturnTreasure);
                    break;
                default:
                    Debug.Log("State ID invalid");
                    break;
            }
        }
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
        if (firstRun == true)
        {
            SetSquadState(PATROL_ID);
            firstRun = false;
        }

        if (CalcDistance(treasureHunter.transform.position, treasure.transform.position).magnitude <= 5 && treasure.GetComponentInParent<NPC>() == null)
        {            
            fsm.SetState(pickupTreasure);
            SetSquadState(PICKUP_TREASURE_ID);
        }
    }

    /// <summary>
    /// Useless for now
    /// </summary>
    private void PickupTreasure()
    {
        if (treasureHunter.GetComponent<HunterNPC>().HasTreasure)
        {
            fsm.SetState(returnTreasure);
            treasureHunter.GetComponent<HunterNPC>().Target = treasureDestination;
            SetSquadState(RETURN_TREASURE_ID);
        }
    }

    private void ReturnTreasure()
    {
        if (CalcDistance(treasureHunter.transform.position, treasureDestination.transform.position).magnitude <= 5)
        {
            Debug.Log("GAME WON");
        }
    }
    #endregion
}
