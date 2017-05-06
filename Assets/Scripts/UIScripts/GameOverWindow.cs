using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

//child of base window
//for game over UI
public class GameOverWindow : BaseWindow
{
    #region Attributes
   // MenuManager manager;
    [SerializeField] private Text winMessage;
    #endregion

    #region Properties
    #endregion

    #region InBuildMethods


    #endregion

    #region HelperMethods
    //to see if main menu is pressed
    protected void MainMenu()
    {
        //Debug.Log("Main Menu Pressed");
        MenuManager.Instance.GoToScreen("Start");
    }

    public void SetWinner(int playerNum)
    {
        switch (playerNum)
        {
            case 1:
                winMessage.text = "Redbeard Wins!";
                break;
            case 2:
                winMessage.text = "BlueJay Wins!";
                break;
            default:
                winMessage.text = "You Lose, go Home.";
                break;
        }
    }
    #endregion

}
