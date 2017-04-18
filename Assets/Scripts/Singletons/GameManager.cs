using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Controls everything related Player input axis and their configuration
/// </summary>
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
    string utility_switch;
    string trap_activate;
    string signal;

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

    public string SIGNAL_AXIS
    {
        get { return signal; }
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

    public string UTILITY_SWITCH
    {
        get { return utility_switch; }
    }

    public string PARROT_TRAP_AXIS
    {
        get { return trap_activate; }
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
                cam_vertical = "Cam_Vertical_P1";
                r3 = "R3_P1";

                //SET PARROT AXES
                fly_up = "FlyUp_P1";
                fly_down = "FlyDown_P1";
                boost = "BoostFly_P1";
                possess = "Possess_P1";
                parrot_pickup = "ParrotPickup_P1";
                utility_switch = "UtilitySwitch_P1";
                trap_activate = "TrapActivate_P1";
                signal = "Signal_P1";

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
                cam_vertical = "Cam_Vertical_P2";
                r3 = "R3_P2";

                //SET PARROT AXES
                fly_up = "FlyUp_P2";
                fly_down = "FlyDown_P2";
                boost = "BoostFly_P2";
                possess = "Possess_P2";
                parrot_pickup = "ParrotPickup_P2";
                utility_switch = "UtilitySwitch_P2";
                trap_activate = "TrapActivate_P2";
                signal = "Signal_P2";

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
    public enum PlayerState { PARROT, CAPTAIN };

    #region Fields
    //player input for player 1 + 2
    PlayerInput p1Input = new PlayerInput();
    PlayerInput p2Input = new PlayerInput();

    //Phase Times
    [SerializeField] private int piratePhaseTime;
    [SerializeField] private int parrotPhaseTime;
    private int currentPirateTime;  //holds the current time left in pirate phase
    private int currentParrotTime; //holds the current time left in parrot phase
    private Coroutine pirateTimerRoutine;
    private Coroutine parrotTimerRoutine;

    //holds the Parrot and Pirate gameObjects for both players
    [SerializeField] private CaptainPirate pirateP1;
    [SerializeField] private CaptainPirate pirateP2;
    [SerializeField] private Parrot parrotP1;
    [SerializeField] private Parrot parrotP2;
    [SerializeField] private GameObject parrotSpawn;

    private PlayerState currentPlayer1State = PlayerState.CAPTAIN;
    private PlayerState currentPlayer2State = PlayerState.CAPTAIN;

    //Cameras
    PirateCam captainCamera1;
    PirateCam captainCamera2;
    ParrotCam parrotCamera1;
    ParrotCam parrotCamera2;
    #endregion

    //Signals
    private bool signalOn = false; //tells if the signal beams are on 
    private ParticleSystem signal1;
    private ParticleSystem signal2;
    [SerializeField] private ParticleSystem treasureSignal;

    #region Properties
    public PlayerState CurrentPlayer1State
    {
        get { return currentPlayer1State; }
    }

    public PlayerState CurrentPlayer2State
    {
        get { return currentPlayer2State; }
    }
    
    public PlayerInput P1Input
    {
        get { return p1Input; }
    }

    public PlayerInput P2Input
    {
        get { return p2Input; }
    }

    public CaptainPirate PirateP1
    {
        get
        {
            return pirateP1;
        }
    }
    public CaptainPirate PirateP2
    {
        get
        {
            return pirateP2;
        }
    }
    public Parrot ParrotP1
    {
        get
        {
            return parrotP1;
        }
    }
    public Parrot ParrotP2
    {
        get
        {
            return parrotP2;
        }
    }
    public GameObject ParrotSpawn
    {
        get { return parrotSpawn; }
    }
    public float ParrotPhaseTime
    {
        get
        {
            return parrotPhaseTime;
        }
    }
    public float PiratePhaseTime
    {
        get
        {
            return piratePhaseTime;
        }
    }

    public int CurrentPirateTime
    {
        get { return currentPirateTime; }
    }

    public int CurrentParrotTime
    {
        get { return currentParrotTime; }
    }

    public bool SignalOn
    {
        get { return signalOn; }
        set { signalOn = value; }
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
        //Get the player scripts
        //pirateP1 = GameObject.FindGameObjectWithTag("Captain1").GetComponent<CaptainPirate>();
        //pirateP2 = GameObject.FindGameObjectWithTag("Captain2").GetComponent<CaptainPirate>();
        //parrotP1 = GameObject.FindGameObjectWithTag("Parrot1").GetComponent<Parrot>();
        //parrotP2 = GameObject.FindGameObjectWithTag("Parrot2").GetComponent<Parrot>();

        captainCamera1 = GameObject.FindGameObjectWithTag("CaptainCamera1").GetComponent<PirateCam>();
        captainCamera2 = GameObject.FindGameObjectWithTag("CaptainCamera2").GetComponent<PirateCam>();
        parrotCamera1 = GameObject.FindGameObjectWithTag("ParrotCamera1").GetComponent<ParrotCam>();
        parrotCamera2 = GameObject.FindGameObjectWithTag("ParrotCamera2").GetComponent<ParrotCam>();

        p1Input.ConfigureInput(1);
        p2Input.ConfigureInput(2);

        signal1 = pirateP1.gameObject.transform.FindChild("Signal Beam").GetComponent<ParticleSystem>();
        signal2 = pirateP2.gameObject.transform.FindChild("Signal Beam").GetComponent<ParticleSystem>();
        

        pirateTimerRoutine = StartCoroutine(PiratePhaseTimer());

    }

    void Update()
    {

    }

    /// <summary>
    /// Switch to Pirate phase
    /// </summary>
    private void SwitchPiratePhase()
    {
        //Recenter pirate Cam
        captainCamera1.Recenter();
        captainCamera2.Recenter();

        //set pirates to active
        pirateP1.PirateActive = true;
        pirateP2.PirateActive = true;

        pirateP1.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
        pirateP2.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;

        //set parrots to inactive
        parrotP1.active = false;
        parrotP2.active = false;

        currentPlayer1State = PlayerState.CAPTAIN;
        currentPlayer2State = PlayerState.CAPTAIN;
    }

    /// <summary>
    /// Switches to parrot phase
    /// </summary>
    private void SwitchParrotPhase()
    {
        //Recenter parrot Camera
        parrotCamera1.Recenter();
        parrotCamera2.Recenter();

        //set pirates to inactive
        pirateP1.PirateActive = false;
        pirateP2.PirateActive = false;

        //free pirates rigidbody
        pirateP1.RBody.constraints = RigidbodyConstraints.FreezeAll;
        pirateP2.RBody.constraints = RigidbodyConstraints.FreezeAll;

        //set parrots to active
        parrotP1.active = true;
        parrotP2.active = true;

        currentPlayer1State = PlayerState.PARROT;
        currentPlayer2State = PlayerState.PARROT;

        //turn of signal beams
        signal1.Stop();
        signal2.Stop();
        //treasureSignal.Stop();
    }

    private IEnumerator PiratePhaseTimer()
    {
        SwitchPiratePhase();
        Debug.Log("Pirate Phase");
        for (currentPirateTime = piratePhaseTime; CurrentPirateTime > 0; currentPirateTime--)
        {
            yield return new WaitForSeconds(1);
        }

        parrotTimerRoutine = StartCoroutine(ParrotPhaseTimer());
    }

    private IEnumerator ParrotPhaseTimer()
    {
        SwitchParrotPhase();
        Debug.Log("Parrot Phase");
        for (currentParrotTime = parrotPhaseTime; currentParrotTime > 0; currentParrotTime--)
        {
            yield return new WaitForSeconds(1);
        }

        pirateTimerRoutine = StartCoroutine(PiratePhaseTimer());
    } 

    public IEnumerator SignalBeam()
    {
        

        //turn on signal beams
        signalOn = true;
        signal1.Play();
        signal2.Play();
        treasureSignal.Play();

        yield return new WaitForSeconds(10);

        //turn off signal Beams
        signalOn = false;
        signal1.Stop();
        signal2.Stop();
        treasureSignal.Stop();
    }
    /// <summary>
    /// This Method is called when the game is over and should handle all task for ending the game.
    /// </summary>
    public void EndGame()
    {
        StopAllCoroutines();
    }
}
