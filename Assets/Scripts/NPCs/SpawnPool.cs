using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPool : MonoBehaviour {
    #region Attributes
    [SerializeField] private GameManager.Team pirateTeam;
    private List<GameObject> squads;
    private List<GameObject> orphans;
    [SerializeField] private float spawnRadius = 15;
    private float spawnDelay = 10;
    private bool canSpawn;
    private int MaxSquads = 2;
    private Queue spawnRequests;
    #endregion

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    #region Helper Methods
    private void SpawnSquad()
    {

    }
    #endregion
}
