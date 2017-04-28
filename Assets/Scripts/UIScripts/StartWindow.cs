using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//child of BaseWindow
//code for start Menu
public class StartWindow : BaseWindow
{
    #region Attributes
  
    [SerializeField] Button pauseButton;
    [SerializeField] Button startButton;
    private bool pause;
    private bool newGame;
    
    #endregion

    #region inBuildMethods
    void Start()
    {
        /*only have start button if we are on mainmenu
        if (SceneManager.GetActiveScene().name != "MainMenu")
        {
            startButton.gameObject.SetActive(false);
            pauseButton.gameObject.SetActive(true);
        }
        else
        {
            startButton.gameObject.SetActive(true);
            pauseButton.gameObject.SetActive(false);
        }*/

        Open();
    }

    void Awake()
    {
        Debug.Log("find menu");
        Open();
    }

    #endregion

    #region helperMethods

    //opens the Main Menu 
    public override void Open()
    {
        //base.Open();
        /*set correct first start based on what scene we are in
        if (SceneManager.GetActiveScene().name != "MainMenu")
        {
            firstSelected = pauseButton.gameObject;
        }
        else
        {
            firstSelected = startButton.gameObject;
        }*/

        base.Open();
    }

    //method for new game button
    protected void NewGame()
    {
        MenuManager.Instance.MenuEnabled = !MenuManager.Instance.MenuEnabled;
        SceneManager.LoadScene("Poseidon");
        Debug.Log("New Game Pressed");
    }   

    protected void ResumeGame()
    {
        MenuManager.Instance.GoToScreen("Start"); //reset start screen
        MenuManager.Instance.MenuEnabled = !MenuManager.Instance.MenuEnabled;
        GameManager.Instance.HUD.GetComponent<Canvas>().enabled = (!GameManager.Instance.HUD.GetComponent<Canvas>().enabled);
        //turn on HUD
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
