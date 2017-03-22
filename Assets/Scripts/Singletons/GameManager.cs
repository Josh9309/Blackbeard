using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Handles main play cycle logic. Anything within game manager should happen during gameplay
/// and terminate when advancing to other menus.
/// </summary>
public class GameManager : Singleton<GameManager>
{
    #region Fields
    //Assigned in inspector
    //public Player player;
    private GameObject player;

    // put enums in here so all NPC's and squads have access to them
    public enum Team { RED, BLUE };
    public enum PlayerState { PARROT, BUCCANEER, HUNTER};
    private PlayerState currentPlayerState = PlayerState.PARROT;

    // list of all red and blue squads
    private List<GameObject> blueSquads;
    private List<GameObject> redSquads;

    [SerializeField]
    private int numSquadsPerTeam;
    #endregion

    #region Properties
    public GameObject Player { set { player = value; } get { return player; } }
    public PlayerState CurrentPlayerState { set { currentPlayerState = value; } get { return currentPlayerState; } }
    public List<GameObject> BlueSquads
    {
        get
        {
            return blueSquads;
        }
    }
    public List<GameObject> RedSquads
    {
        get
        {
            return redSquads;
        }
    }
    #endregion

    protected GameManager(){}

	void Awake()
	{
        // assign arrays
        //blueSquadsArray = GameObject.FindGameObjectsWithTag("BlueSquad");
        //redSquadsArray = GameObject.FindGameObjectsWithTag("RedSquad");
        //Debug.Log("Game Manager is alive");

        redSquads = new List<GameObject>();
        blueSquads = new List<GameObject>();
        player = GameObject.FindGameObjectWithTag("Parrot");
	}

	public void StartGame()
	{
		MenuManager.Instance.GoToScreen("GameScreen");
		
	}

	/// <summary>
	/// Will be called in other places later on
    /// assign enemies to squads
	/// </summary>
	public void StartRound()
	{
        //set score to zero
        //MAY BE DEPRICATED
        GameObject[] rs = GameObject.FindGameObjectsWithTag("RedSquad");
        GameObject[] bs = GameObject.FindGameObjectsWithTag("BlueSquad");

        for (int i = 0; i < rs.Length; i++)
        {
            redSquads.Add(rs[i]);
        }

        for (int i = 0; i < bs.Length; i++)
        {
            blueSquads.Add(bs[i]);
        }

        for (int i = 0; i < numSquadsPerTeam; i++)
        {
            redSquads[i].GetComponent<SquadManager>().EnemySquadObjects = blueSquads;
            blueSquads[i].GetComponent<SquadManager>().EnemySquadObjects = redSquads;
        }

    }

    /// <summary>
    /// Any time a squad needs to notify the GameManager of something, it should
    /// call this method
    /// </summary>
    /// <param name="squad">squad that is notifying</param>
    /// <param name="note">enum notification</param>
    public void Notify(Team team, GameObject squad, SquadManager.Notification note)
    {
        switch(note)
        {
            case SquadManager.Notification.HAS_TREASURE:
                if (team == Team.BLUE)
                {
                    foreach (GameObject s in blueSquads)
                    {
                        s.GetComponent<SquadManager>().AllyHasTreasure = true;
                    }
                }
                else
                {
                    foreach (GameObject s in redSquads)
                    {
                        s.GetComponent<SquadManager>().AllyHasTreasure = true;
                    }
                }
                break;

            case SquadManager.Notification.SPAWN_REQUEST_MELEE:
                Debug.Log("Spawn Request unhandled at this time. STAHP BREAKIN MUH CODE");
                break;

            case SquadManager.Notification.SPAWN_REQUEST_TREASURE:
                Debug.Log("Spawn Request unhandled at this time. STAHP BREAKIN MUH CODE");
                break;
        }
    }

    void Start()
    {
        StartRound();
    }

    void Update()
    {


    }
}
