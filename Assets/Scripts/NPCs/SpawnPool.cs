using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Spawn Request Struct
public struct SpawnRequest
{
    public bool loneHunter;
    public int numMeleePirates;
};

public class SpawnPool : MonoBehaviour {


    #region Attributes
    [SerializeField] private GameObject squadPrefab;
    [SerializeField] private GameObject treasureDestination;
    [SerializeField] private GameManager.Team pirateTeam;
    private List<GameObject> squads;
    private List<GameObject> orphans;
    [SerializeField] private float spawnRadius = 15;
    private float spawnDelay = 10;
    private bool canSpawn;
    private int MaxSquads = 2;
    public Queue<SpawnRequest> spawnRequests;
    private GameManager gm;
    #endregion

    // Use this for initialization
    void Start () {
        //get game manager
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        //initalize lists and queue
        squads = new List<GameObject>();
        orphans = new List<GameObject>();
        spawnRequests = new Queue<SpawnRequest>();

        //create the first squad
        GameObject squad = SpawnSquad(4);
        squads.Add(squad); //adds the new squad to the spawn pools list of squads
    }
	
	// Update is called once per frame
	void Update () {
        ProccessRequests();
	}

    #region Helper Methods

    /// <summary>
    /// This method will check the spawn request queue and will proccess any request accordingly
    /// </summary>
    private void ProccessRequests()
    {
        if(spawnRequests.Count == 0  || !canSpawn || squads.Capacity == 2) //checks if queue is empty, or if a squad cannot be spawned right now
        {
            return; //leave method
        }

        //get the next request
        SpawnRequest request = spawnRequests.Dequeue();

        if (request.loneHunter)
        {
            //TODO: INSERT CODE FOR DEALING WITH SPAWNNING A SQUAD FOR A LONE HUNTER
        }
        else
        {
            GameObject squad = SpawnSquad(request.numMeleePirates);

            squads.Add(squad); //adds the new squad to the spawn pools lis of squads
        }
    }

    /// <summary>
    /// This method will spawn a standard squad
    /// </summary>
    private GameObject SpawnSquad(int numPirates)
    {
        GameObject sq = Instantiate(squadPrefab, transform.localPosition, Quaternion.identity) as GameObject;
        SquadManager squadMan = sq.GetComponent<SquadManager>();
        squadMan.Team = pirateTeam;
        squadMan.MaxPirates = numPirates;
        squadMan.initialSpawnRadius = spawnRadius;
        squadMan.TreasureDest = treasureDestination;
        squadMan.spawningPool = this;
        //squadMan.runStart = true;
       // squadMan.Start();

        return sq;
    }
    #endregion
}
