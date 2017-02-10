using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//child of base window
//for game over UI
public class GameOverWindow : BaseWindow
{
    #region Attributes
   // MenuManager manager;
    #endregion

    #region Properties
    #endregion

    #region InBuildMethods


    #endregion

    #region HelperMethods
    //to see if main menu is pressed
    protected void MainMenu()
    {
        //manager.Open(0);
        MenuManager.Instance.GoToScreen("Start");
        Debug.Log("Main Menu Pressed");
    }
    #endregion

}
