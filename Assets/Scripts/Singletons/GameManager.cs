using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Controls everything relates Player input axis and their configuration
public class PlayerInput
{
    #region Attributes
    //player id
    int id;

    //General Axes
    string horizontal_axis;
    string vertical_axis;
    string cam_horizontal;
    string cam_vertical;
    string r3;

    //Parrot Axes
    string fly_up;
    string fly_down;
    string boost;
    string possess;
    string parrot_pickup;

    //Pirate Axes
    string jump;
    string pirate_pickup;

    //Menu Axes
    string submit;
    #endregion

    #region Properties
    public int ID
    {
        get { return id; }
    }

    public string HORIZONTAL_AXIS
    {
        get { return horizontal_axis; }
    }

    public string VERTICAL_AXIS
    {
        get { return vertical_axis; }
    }

    public string CAM_HORIZONTAL_AXIS
    {
        get { return cam_horizontal; }
    }

    public string CAM_VERTICAL_AXIS
    {
        get { return cam_vertical; }
    }

    public string R3_AXIS
    {
        get { return r3; }
    }

    public string FLY_UP_AXIS
    {
        get { return fly_up; }
    }

    public string FLY_DOWN_AXIS
    {
        get { return fly_down; }
    }

    public string BOOST_AXIS
    {
        get { return boost; }
    }

    public string POSSESS_AXIS
    {
        get { return possess; }
    }

    public string PARROT_PICKUP_AXIS
    {
        get { return parrot_pickup; }
    }

    public string JUMP_AXIS
    {
        get { return jump; }
    }

    public string PICKUP_MOVE_AXIS
    {
        get { return pirate_pickup; }
    }

    public string SUBMIT_AXIS
    {
        get { return submit; }
    }
    #endregion

    /// <summary>
    /// Configure the input based on the player number
    /// </summary>
    /// <param name="playerNum"></param>
    public void ConfigureInput(int playerNum)
    {
        //set player id
        id = playerNum;

        switch (id)
        {
            case 1:
                //set general axes
                horizontal_axis = "Horizontal_P1";
                vertical_axis = "Vertical_P1";
                cam_horizontal = "Cam_Horizontal_P1";
                cam_vertical = "CAM_Vertical_P1";
                r3 = "R3_P1";

                //SET PARROT AXES
                fly_up = "FlyUp_P1";
                fly_down = "FlyDown_P1";
                boost = "BoostFly_P1";
                possess = "Possess_P1";
                parrot_pickup = "ParrotPickup_P1";

                //SET PIRATE AXES
                jump = "Jump_P1";
                pirate_pickup = "Pickup/Move_P1";

                //SET MENU AXES
                submit = "Submit_P1";
                break;

            case 2:
                //set general axes
                horizontal_axis = "Horizontal_P2";
                vertical_axis = "Vertical_P2";
                cam_horizontal = "Cam_Horizontal_P2";
                cam_vertical = "CAM_Vertical_P2";
                r3 = "R3_P2";

                //SET PARROT AXES
                fly_up = "FlyUp_P2";
                fly_down = "FlyDown_P2";
                boost = "BoostFly_P2";
                possess = "Possess_P2";
                parrot_pickup = "ParrotPickup_P2";

                //SET PIRATE AXES
                jump = "Jump_P2";
                pirate_pickup = "Pickup/Move_P2";

                //SET MENU AXES
                submit = "Submit_P2";
                break;
        }
        
    }
}

/// <summary>
/// Handles main play cycle logic. Anything within game manager should happen during gameplay
/// and terminate when advancing to other menus.
/// </summary>
public class GameManager : Singleton<GameManager>
{
    #region Fields
    //player input for player 1 + 2
    PlayerInput p1Input = new PlayerInput();
    PlayerInput p2Input = new PlayerInput();

    private GameObject player1;
    private GameObject player2;

    
    public enum PlayerState { PARROT, CAPTAIN};
    private PlayerState currentPlayerState = PlayerState.CAPTAIN;
    #endregion

    #region Properties
    public GameObject Player1
    {
        get { return player1; }
        set
        {
            player1 = value;
        }
    }

    public PlayerState CurrentPlayerState {
        get { return currentPlayerState; }
        set {
            currentPlayerState = value;
        }
    }
    
    public PlayerInput P1Input
    {
        get { return p1Input; }
    }

    public PlayerInput P2Input
    {
        get { return p2Input; }
    }
    #endregion

    protected GameManager(){}

	void Awake()
	{

	}

	public void StartGame()
	{
		MenuManager.Instance.GoToScreen("GameScreen");
		
	}

    void Start()
    {
        p1Input.ConfigureInput(1);
        p2Input.ConfigureInput(2);
    }

    void Update()
    {


    }
}
