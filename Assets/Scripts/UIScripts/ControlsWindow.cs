using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//child of BaseWindow
//code for start Menu
public class ControlsWindow : BaseWindow
{
    #region Attributes
    [SerializeField] private Image pirateImg;
    [SerializeField] private Image parrotImg;
    #endregion

    #region inBuildMethods

    #endregion

    #region helperMethods
    public override void Open()
    {
        //run baseWindoes open method
        base.Open();

        //turn off parrot image and pirate on
        pirateImg.enabled = true;
        parrotImg.enabled = false;
    }

    public void ShowParrot()
    {
        //turn on parrot image and pirate off
        pirateImg.enabled = false;
        parrotImg.enabled = true;
    }

    public void ShowPirrate()
    {
        //turn off parrot image and pirate on
        pirateImg.enabled = true;
        parrotImg.enabled = false;
    }

    public void Back()
    {
        MenuManager.Instance.GoToScreen("Start");
    }


    #endregion
}
