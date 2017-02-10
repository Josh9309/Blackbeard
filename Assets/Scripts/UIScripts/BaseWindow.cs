using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

//parent class for UI
public class BaseWindow : MonoBehaviour
{
    #region Attributes
    //to select the default button
    [SerializeField]protected GameObject firstSelected;
    //getter for EventSystem to pass in values
    //public static WindowManager manager;
    #endregion

    #region Properties
    protected EventSystem eventSystem
    {
        get
        {
            //gets the event System 
            return GameObject.Find("EventSystem").GetComponent<EventSystem>();
        }
    }

    #endregion

    #region inBuildMethods
    protected virtual void Awake ()
    {
        Close();
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}
    #endregion

    #region helperMethods

    //select object method
    protected void OnFocus()
    {
        eventSystem.SetSelectedGameObject(firstSelected);
    }

    //show menu
    protected void Display(bool value)
    {
        gameObject.SetActive(value);
    }

    //to Open UI/Button
    protected virtual void Open()
    {
        Display(true);
        OnFocus();
    }

    //closes the UI
    protected virtual void Close()
    {
        Display(false);
    }

    #endregion
}
