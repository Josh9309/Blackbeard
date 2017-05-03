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
        //select start
        //firstSelected = startButton.gameObject;

        //turn on resume button if we arent in the main menu
        if (SceneManager.GetActiveScene().name != "MainMenu")
        {
            pauseButton.gameObject.SetActive(true);
        }
        else
        {
            pauseButton.gameObject.SetActive(false);
        }

        Open();
    }

    void Awake()
    {
        Open();
    }

    #endregion

    #region helperMethods

    //opens the Main Menu 
    public override void Open()
    {
        //base.Open();
        //set correct first start based on what scene we are in
        if (SceneManager.GetActiveScene().name != "MainMenu")
        {
            //turn on resume button
            pauseButton.gameObject.SetActive(true);

            //change the nav hook ups for the start menu
            Button[] btns = this.gameObject.GetComponentsInChildren<Button>(); //get all our buttons

            Navigation nav = btns[0].navigation; //make new nav object
            nav.mode = Navigation.Mode.Explicit; //set its nav mdoe to explicit so we can dicttate it

            //start -> resume
            nav.selectOnUp = btns[1];
            btns[0].navigation = nav;

            //options -> resume
            nav = btns[2].navigation;
            nav.selectOnDown = btns[1];
            btns[2].navigation = nav;
        }
        else
        {
            //turn off resume
            pauseButton.gameObject.SetActive(false);
        }

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
