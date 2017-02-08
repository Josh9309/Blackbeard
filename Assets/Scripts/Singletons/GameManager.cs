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
    private List<GameObject> pirates = new List<GameObject>();
    private GameObject player; 
    #endregion

    #region Properties
    #endregion

    protected GameManager() { }

    void Awake()
    {
        //start game
        StartGame();
    }

    public void StartGame()
    {
        //MenuManager.Instance.GoToScreen("GameScreen");

    }



    void Update()
    {

    }
}
