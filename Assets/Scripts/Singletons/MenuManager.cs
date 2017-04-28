using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

/// <summary>
/// Attached to main canvas in scene. Menu manager holds references to all ui screens.
/// Should cycle through ui in a stack. Holds methods to go to each screen and an all encompassing back method.
/// </summary>
public class MenuManager : Singleton<MenuManager>
{
	#region Fields
	//Screens
	Dictionary<string, GameObject> screens = new Dictionary<string, GameObject>();
	Stack<string> screenStack = new Stack<string>();
    [SerializeField] Canvas menu;

    //pauseing
    private bool pause;
 
    #endregion

    #region Properties
    public Dictionary<string, GameObject> Screens { get { return screens; } }
	public string CurrentScreen { get { return screenStack.Peek(); } }

    public bool MenuEnabled
    {
        get { return menu.enabled; }
        set
        {
            menu.enabled = value;
        }
    }

    public bool Pause
    {
        get { return pause; }
        set
        {
            pause = value;
        }
    }
    #endregion

    #region inBuiltMethods
    void Awake()
	{
		//Load all screens below the canvas into a dictionary for reference
		for (int i = 0; i < gameObject.transform.GetChild(0).childCount; i++)
		{
			screens.Add(gameObject.transform.GetChild(0).GetChild(i).name, gameObject.transform.GetChild(0).GetChild(i).gameObject);
		}

        //make menu only initally there on main menu
        if (SceneManager.GetActiveScene().name != "MainMenu")
        {
            menu.enabled = false;
        }

        //keep this when we change scenes
        DontDestroyOnLoad(this);
	}

	void Start()
	{
		GoToScreen("Start"); //goto the main menu start screen
        pause = false; //we def arent in the pause menu rn

        //make menu only initally there on main menu
        if (SceneManager.GetActiveScene().name != "MainMenu")
        {
            menu.enabled = false;
        }
        else
        {
            menu.enabled = true;
        }
	}

    private void Update()
    {
        
    }
    #endregion

    #region helperMethods
    protected MenuManager() { }
    /// goes to a screen given a name
    /// </summary>
    /// <param name="name">Name.</param>
    public void GoToScreen(string name)
	{
		if (screenStack.Count > 0)
		{
			screens[screenStack.Peek()].SetActive(false);
		}
		screenStack.Push(name);
		screens[name].SetActive(true);
	}

	public void Back()
	{
		screens[screenStack.Peek()].SetActive(false);
		screenStack.Pop();
		screens[screenStack.Peek()].SetActive(true);
	}
    #endregion
}
