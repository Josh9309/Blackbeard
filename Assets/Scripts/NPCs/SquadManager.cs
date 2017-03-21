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
    //FIX LATER
    public GameObject TreasureDest;

    // enum representing which notification to notify the GameManager of
    public enum Notification { HAS_TREASURE, SPAWN_REQUEST_MELEE, SPAWN_REQUEST_TREASURE};  

    // reference to the game manager
    private GameManager gm;

    // int values for parsing through statess
    private const int PATROL_ID = 0;
    private const int COMBAT_ID = 1;
    private const int PICKUP_TREASURE_ID = 2;
    private const int RETURN_TREASURE_ID = 3;
    private const int DEFEND_TREASURE_ID = 4;

    // the way that unity intanstiates objects is stupid so this variable is necessary
    // for making sure an initial state is assigned to the squad
    bool firstRun = true;

    // this bool keeps track of if an allied squad has the treasure, if so, this squad will
    // go to the defendTreasure state instead of the patrol state
    // NOTE: GameManager is responsible for tracking this variable
    private bool allyHasTreasure = false;

    // indentifiers
    [SerializeField]
    private GameManager.Team team;

    // the treasure
    private GameObject treasure;

    // enemy squads
    private List<GameObject> enemySquadObjects;
    private GameObject enemyTarget;

    // states
    public enum State { PATROL, COMBAT, RETURN_TREASURE, PICKUP_TREASURE, DEFEND_TREASURE};
    private State currentState;
    FSM.State patrol;
    FSM.State combat;
    FSM.State returnTreasure;
    FSM.State pickupTreasure;
    FSM.State defendTreasure;

    // variables related to pirates in squad
    private List<GameObject> pirates;
    private List<GameObject> meleePirates;
    [SerializeField]
    private int maxPirates;
    private GameObject treasureHunter;

    //Scripts
    private List<NPC> npcScript;
    private List<BasePirate> basePirateScript;

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
    private float engagementZoneRadius;
    private Vector3 engagementZoneCentroid;
    [SerializeField]
    private float initialSpawnRadius;
    private bool drawZone = false;

    // flocking
    private Vector3 direction;

    // if you want to directly set a target for the squad
    [SerializeField]
    private GameObject DestinationNode;
    private GameObject currentNode;

    // boolean trackers
    private bool playerInEnemy = false;

    #endregion

    #region Accessors
    // use team accessor to return a string representing NPC's team
    public GameManager.Team getTeam { get { return team; } }

    // allow for game manager to assign it a list of enemies
    public List<GameObject> EnemySquadObjects { get { return enemySquadObjects; } set { enemySquadObjects = value; } }

    // allow for gameManager to assign when an allied squad has the treasure
    public bool AllyHasTreasure { get { return allyHasTreasure; } set { allyHasTreasure = value; } }

    // allow this squad to pass enemy NPCs in to its melee pirates
    public List<GameObject> Pirates { get { return pirates; } }
    public List<GameObject> MeleePirates { get { return meleePirates; } }
    public GameObject TreasureHunter { get { return treasureHunter; } }
    #endregion

    // Use this for initialization
    void Start () {
        // initialize variables
        fsm = GetComponent<FSM>();
        pirates = new List<GameObject>();
        meleePirates = new List<GameObject>();
        treasure = GameObject.FindGameObjectWithTag("Treasure");
        treasureDestination = GameObject.FindGameObjectWithTag("TreasureDestination");
        direction = new Vector3(0, 0, 0);
        currentNode = GameObject.Instantiate(DestinationNode, transform.position, Quaternion.identity);
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        npcScript = new List<NPC>();
        basePirateScript = new List<BasePirate>();

        // initialize states
        patrol = Patrol;
        combat = Combat;
        pickupTreasure = PickupTreasure;
        returnTreasure = ReturnTreasure;
        defendTreasure = DefendTreasure;

        // spawn a single treasure Hunter
        treasureHunter = Instantiate(treasureNPC, transform.position, Quaternion.identity);
        pirates.Add(treasureHunter);
        treasureHunter.GetComponent<NPC>().Squad = this.gameObject;
        treasureHunter.GetComponent<NPC>().getTeam = team;
        treasureHunter.GetComponent<HunterNPC>().treasureDestination = TreasureDest;

        int numSpawned = 1;

        // spawn melee pirates
        for (int i = 1; i <= maxPirates; i++)
        {
            Vector3 pos = new Vector3(Random.Range(-initialSpawnRadius, initialSpawnRadius) + transform.position.x, transform.position.y, Random.Range(-initialSpawnRadius, initialSpawnRadius) + transform.position.z);

            for (int j = 0; j < numSpawned; j++)
            {
                while(CalcDistance(pos, pirates[j].transform.position).magnitude <= 2)
                {
                    pos = new Vector3(Random.Range(-initialSpawnRadius, initialSpawnRadius) + transform.position.x, transform.position.y, Random.Range(-initialSpawnRadius, initialSpawnRadius) + transform.position.z);
                }
            }

            pirates.Add(GameObject.Instantiate(meleeNPC, pos, Quaternion.identity));
            pirates[i].GetComponent<NPC>().Squad = this.gameObject;
            pirates[i].GetComponent<NPC>().getTeam = team;
            pirates[i].GetComponent<MeleeNPC>().Leader= treasureHunter;

            // add to meleePirates list for tracking
            meleePirates.Add(pirates[i]);
            numSpawned++;
        }

        for (int i = 0; i < pirates.Count; i++)
        {
            npcScript.Add(pirates[i].GetComponent<NPC>());
            basePirateScript.Add(pirates[i].GetComponent<BasePirate>());
        }

        // set initial state
        fsm.SetState(patrol);
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (pirates.Count > 0)
        {
            CalcCentroid();
            CalcDirection();
        }

        fsm.UpdateState();
	}

    #region Helper Methods
    /// <summary>
    /// Method responsible for removing all references to a dead pirate from this
    /// squad's list of pirates as well as any enemy list
    /// </summary>
    /// <param name="squadMember">pirate's gameObject</param>
    /// <param name="type">NPC.PirateType of what the pirate is</param>
    public void Remove(GameObject squadMember, NPC.PirateType type)
    {
        pirates.Remove(squadMember);

        if (type == NPC.PirateType.HUNTER)
        {
            treasureHunter = null;
        }
        
        // FUTURE: check if any pirates are squadless and reassign them or
        // call in a request from gameManager to spawn a new squad member

        // remove reference to enemy squad enemy list
        if (currentState == State.COMBAT)
        {
            enemyTarget.GetComponent<SquadManager>().RemoveEnemy(squadMember, type);
        }

        // if that was the last squad member, then delete the squad
        if (pirates.Count <= 0)
        {
            // remove this squad from the enemy lists of all of its enemies
            for (int i = 0; i < enemySquadObjects.Count; i++)
            {
                enemySquadObjects[i].GetComponent<SquadManager>().RemoveEnemySquad(this.gameObject);
            }

            GameObject.Destroy(this.gameObject);
            GameObject.Destroy(currentNode);
        }
    }

    /// <summary>
    /// removes an enemy from this squad's enemy list if it is removed and if it is
    /// in combat
    /// </summary>
    /// <param name="enemy">enemy's gameObject</param>
    /// <param name="type">NPC.PirateType of what the pirate is</param>
    public void RemoveEnemy(GameObject enemy, NPC.PirateType type)
    {
        for (int i = 0; i < meleePirates.Count ; i++)
        {
            if (meleePirates[i].GetComponent<MeleeNPC>().Enemies.Contains(enemy))
            {
                meleePirates[i].GetComponent<MeleeNPC>().Enemies.Remove(enemy);
                AssignEnemies();
            }
        }
    }

    /// <summary>
    /// This will remove an enemy squad from a list of enemies if its last pirate
    /// has been killed and the squad is about to be destroyed
    /// </summary>
    /// <param name="enemy">gameObject of the enemy squad</param>
    public void RemoveEnemySquad(GameObject enemy)
    {
        if (enemySquadObjects.Contains(enemy))
            enemySquadObjects.Remove(enemy);
    }

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

    /// <summary>
    /// This will be used to orient the squadManager in the same direction that the squad is facing
    /// </summary>
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
    /// This method will be responsible for checking to see if any enemy squads can be engaged,
    /// if so, switch behavior to combat behavior
    /// </summary>
    private bool DetectEnemy()
    {
        for (int i = 0; i < enemySquadObjects.Count; i++)
        {
            if (CalcDistance(this.transform.position, enemySquadObjects[i].transform.position).magnitude <= engagementRadius)
            {
                enemyTarget = enemySquadObjects[i];
                GenerateEngagementZone();
                AssignEnemies();
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// This will be responsible for generating a zone of engagement that agents are allowed
    /// to fight in
    /// </summary>
    private void GenerateEngagementZone()
    {
        Debug.Log("Engage");
        engagementZoneCentroid = (CalcDistance(this.transform.position, enemyTarget.transform.position) / 2) + transform.position;
        currentNode.transform.position = engagementZoneCentroid;

        for (int i = 0; i < meleePirates.Count; i++)
        {
            meleePirates[i].GetComponent<NPC>().Target = currentNode;
        }

        drawZone = true;
    }

    /// <summary>
    /// This method will assign an enemy list to each melee pirate in the squad
    /// </summary>
    private void AssignEnemies()
    {
        for (int i = 0; i < meleePirates.Count; i++)
        {
            // if there are still melee pirates left in the enemy squad
            if (enemyTarget.GetComponent<SquadManager>().MeleePirates.Count > 0)
                meleePirates[i].GetComponent<MeleeNPC>().Enemies = enemyTarget.GetComponent<SquadManager>().MeleePirates;
            else // otherwise they'll target the treasure hunter
                meleePirates[i].GetComponent<MeleeNPC>().Enemies.Add(enemyTarget.GetComponent<SquadManager>().TreasureHunter);
        }
    }

    private void OnDrawGizmos()
    {
        if (drawZone)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(engagementZoneCentroid, engagementZoneRadius);
        }       
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
                case DEFEND_TREASURE_ID:
                    pirateFsm.SetState(npc.NPCDefendTreasure);
                    break;
                default:
                    Debug.Log("State ID invalid");
                    break;
            }
        }
    }

    /// <summary>
    /// used to calculate the closest enemy squad
    /// </summary>
    /// <returns>the closest enemy squad</returns>
    private GameObject CalcNearestEnemySquad()
    {
        if (enemySquadObjects.Count > 0)
        {
            GameObject closest = enemySquadObjects[0];

            for (int i = 0; i < enemySquadObjects.Count; i++)
            {
                if (CalcDistance(transform.position, enemySquadObjects[i].transform.position).magnitude <= CalcDistance(transform.position, closest.transform.position).magnitude)
                {
                    closest = enemySquadObjects[i];
                }
            }

            return closest;
        }
        else
        {
            return null;
        }
    }
    #endregion

    // All transitions are handled by the state PRIOR to the transistion
    // ex: Patrol() is what is responsible for calling the transistion to Combat()
    #region State Methods
    private void Combat()
    {
        currentState = State.COMBAT;

        // if the enemy squad is ever deleted
        if (enemyTarget == null)
        {
            
            if (!allyHasTreasure)
            {
                fsm.SetState(patrol);
                SetSquadState(PATROL_ID);
            }              
            else
            {
                fsm.SetState(defendTreasure);
                SetSquadState(DEFEND_TREASURE_ID);
            }
                
        }

        if (gm.CurrentPlayerState == GameManager.PlayerState.BUCCANEER && playerInEnemy == false)
        {
            if (CalcDistance(gm.Player.transform.position, engagementZoneCentroid).magnitude <= engagementZoneRadius)
            {
                for (int i = 1; i <= maxPirates; i++) // needs to be refactored with new melee enemy list
                {
                    pirates[i].GetComponent<MeleeNPC>().Enemies.Add(gm.Player);
                }
            }
            playerInEnemy = true;
        }
    }

    /// <summary>
    /// Initial state of the squad, when no enemies are in sight and the treasure
    /// is not in the squad's possession then the squad should be in this state
    /// </summary>
    private void Patrol()
    {
        currentState = State.PATROL;
        if (playerInEnemy == true)
            playerInEnemy = false;

        if (firstRun == true)
        {
            SetSquadState(PATROL_ID);
            firstRun = false;
        }

        if (allyHasTreasure)
        {
            fsm.SetState(defendTreasure);
            SetSquadState(PICKUP_TREASURE_ID);
        }

        if (CalcDistance(treasureHunter.transform.position, treasure.transform.position).magnitude <= 5 && treasure.GetComponentInParent<NPC>() == null)
        {
            fsm.SetState(pickupTreasure);
            SetSquadState(PICKUP_TREASURE_ID);
        }

        if (DetectEnemy())
        {
            fsm.SetState(combat);
            SetSquadState(COMBAT_ID);
        }
    }

    private void PickupTreasure()
    {
        currentState = State.PICKUP_TREASURE;
        if (playerInEnemy == true)
            playerInEnemy = false;

        if (treasureHunter.GetComponent<HunterNPC>().HasTreasure)
        {
            fsm.SetState(returnTreasure);
            gm.Notify(team, this.gameObject, Notification.HAS_TREASURE);
            treasureHunter.GetComponent<HunterNPC>().Target = treasureDestination;
            SetSquadState(RETURN_TREASURE_ID);
        }
    }

    private void ReturnTreasure()
    {
        currentState = State.RETURN_TREASURE;
        if (playerInEnemy == true)
            playerInEnemy = false;

        if (CalcDistance(treasureHunter.transform.position, treasureDestination.transform.position).magnitude <= 5)
        {
            Debug.Log("GAME WON");
        }
    }

    private void DefendTreasure()
    {
        currentState = State.DEFEND_TREASURE;

        //if (treasureHunter.GetComponent<HunterNPC>().Target == treasure)
            treasureHunter.GetComponent<HunterNPC>().Target = CalcNearestEnemySquad();
    }
    #endregion
}
