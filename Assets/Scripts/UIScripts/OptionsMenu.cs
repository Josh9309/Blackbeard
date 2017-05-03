using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//child of BaseWindow
//code for start Menu
public class OptionsMenu: BaseWindow
{
    #region Attributes
    [SerializeField]
    Button pauseButton;
    [SerializeField]
    Button startButton;
    private bool pause;
    private bool newGame;

    #endregion

    #region inBuildMethods
    void Start()
    {
       

    }

    #endregion

    #region helperMethods

    //opens the Main Menu 
    public override void Open()
    {
        //to turn pause and new game on/off
        //if pause if off window is on and vise versa
        //turn off pause
        //pauseButton.gameObject.SetActive(pause);
        //if (pauseButton.gameObject.activeSelf)
        //{
        //    firstSelected = startButton.gameObject;
        //}

        ////turn off start
        //startButton.gameObject.SetActive(newGame);
        //if (startButton.gameObject.activeSelf)
        //{
        //    firstSelected = pauseButton.gameObject;
        //}

        base.Open();
    }

    //method for new game button
    protected void NewGame()
    {
        SceneManager.LoadScene("Posideon");
        //MenuManager.Instance.GoToScreen("GameHUD");
        Debug.Log("New Game Pressed");
    }

    protected void ResumeGame()
    {
        MenuManager.Instance.MenuEnabled = false; //turn off menu system and return to game
        Debug.Log(" Resume Pressed");
    }
    //method for main menu
    public void MainMenu()
    {
        MenuManager.Instance.GoToScreen("Start");
        Debug.Log("Main Menu");
    }
    //method for options button
    protected void Options()
    {
        MenuManager.Instance.GoToScreen("Options");
        Debug.Log("Option Pressed");
    }

    //method for credits menu
    protected void Credits()
    {
        MenuManager.Instance.GoToScreen("Credits");
        Debug.Log("Credits Pressed");
    }

    //method for exit menu
    protected void Exit()
    {
        MenuManager.Instance.GoToScreen("Exit");
        Debug.Log("Exit Pressed");
    }
    #endregion
}
