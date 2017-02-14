using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitMenu : BaseWindow {

    #region Attributes

    #endregion

    #region Properties
    #endregion

    #region inBuiltMethods
    #endregion

    #region helperMethods
    
    public void Exit()
    {
        Debug.Log("Exit");
        Application.Quit();
    }
    
    public void DontExit()
    {
        Debug.Log("No pressed");
        //MenuManager.Instance.GoToScreen("Start");
    }
    #endregion
}
