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

	//Platform
	#endregion

	#region Properties
	
	#endregion

	protected GameManager(){}

	void Awake()
	{
	}

	public void StartGame()
	{
		//MenuManager.Instance.GoToScreen("GameScreen");
	}

	/// <summary>
	/// Will be called in other places later on
	/// </summary>
	public void StartRound()
	{
      
	}

    /// <summary>
    /// starts round, and moves us to the next level
    /// </summary>
  

	/// <summary>
	/// When the player lands on a new platform this should be called
	/// </summary>
	

    void Update()
    {
       
    }
}
