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
    public enum PlayerState { PARROT, CAPTAIN};
    private PlayerState currentPlayerState = PlayerState.PARROT;
    #endregion

    #region Properties
    public GameObject Player { set { player = value; } get { return player; } }
    public PlayerState CurrentPlayerState { set { currentPlayerState = value; } get { return currentPlayerState; } }
    #endregion

    protected GameManager(){}

	void Awake()
	{

	}

	public void StartGame()
	{
		MenuManager.Instance.GoToScreen("GameScreen");
		
	}

    void Start()
    {
        
    }

    void Update()
    {


    }
}
