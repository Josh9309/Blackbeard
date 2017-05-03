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
        //only have start button if we are on mainmenu
        if (SceneManager.GetActiveScene().name != "MainMenu")
        {
            startButton.gameObject.SetActive(false);
            pauseButton.gameObject.SetActive(true);
        }
        else
        {
            startButton.gameObject.SetActive(true);
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
            //chnage Start out for Resume
            startButton.gameObject.SetActive(false);
            pauseButton.gameObject.SetActive(true);

            firstSelected = pauseButton.gameObject; //make resume first selected

            //change the nav hook ups for Options(2) and Exit(4)
            Button[] btns = this.gameObject.GetComponentsInChildren<Button>(); //get all our buttons

            Navigation nav = btns[1].navigation; //make new nav object
            nav.mode = Navigation.Mode.Explicit; //set its nav mdoe to explicit so we can dicttate it

            nav.selectOnDown = btns[0]; //set nav down to be resume button
            btns[1].navigation = nav; //save nav to button

            nav = btns[btns.Length - 1].navigation; //get new nav button again
            nav.selectOnUp = btns[0]; //hook exit up to resume button 
            btns[btns.Length - 1].navigation = nav; //save nav to button
        }
        else
        {
            //Use Start instead of Resume
            startButton.gameObject.SetActive(true);
            pauseButton.gameObject.SetActive(false);

            //select start
            firstSelected = startButton.gameObject;
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
