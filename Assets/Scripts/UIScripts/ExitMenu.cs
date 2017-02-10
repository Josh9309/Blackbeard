using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitMenu : BaseWindow {

	public void Exit()
    {
        Debug.Log("Exit");
        Application.Quit();
    }
    
    public void DontExit()
    {
        Debug.Log("No pressed");
        MenuManager.Instance.GoToScreen("Start");
    }
}
