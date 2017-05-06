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
    string fly;
    string boostBrake;
    string possess;
    string utility_switch;
    string parrot_pickup;
    string signal;

    //Pirate Axes
    string jump;
    string respawn;

    //Menu Axes
    string submit;
    string pause;


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

    public string FLY_AXIS
    {
        get { return fly; }
    }

    public string BOOST_BRAKE_AXIS
    {
        get { return boostBrake; }
    }

    public string POSSESS_AXIS
    {
        get { return possess; }
    }

    public string UTILITY_SWITCH
    {
        get { return utility_switch; }
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

    public string RESPAWN_AXIS
    {
        get { return respawn; }
    }
    public string SUBMIT_AXIS
    {
        get { return submit; }
    }
    public string PAUSE_AXIS
    {
        get { return pause; }
    }

    #endregion

    /// <summary>
    /// Configure the input based on the player number
    /// </summary>
    /// <param name="playerNum"> the players number (1 index based)</param>
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
                fly = "Fly_P1";
                boostBrake = "Boost_Brake_P1";
                possess = "Possess_P1";
                parrot_pickup = "ParrotPickup_P1";
                utility_switch = "UtilitySwitch_P1";
                signal = "Signal_P1";

                //SET PIRATE AXES
                jump = "Jump_P1";
                respawn = "Respawn_P1";

                //SET MENU AXES
                submit = "Submit_P1";
                pause = "Pause_P1";
                break;

            case 2:
                //set general axes
                horizontal_axis = "Horizontal_P2";
                vertical_axis = "Vertical_P2";
                cam_horizontal = "Cam_Horizontal_P2";
                cam_vertical = "Cam_Vertical_P2";
                r3 = "R3_P2";

                //SET PARROT AXES
                fly = "Fly_P2";
                boostBrake = "Boost_Brake_P2";
                possess = "Possess_P2";
                parrot_pickup = "ParrotPickup_P2";
                utility_switch = "UtilitySwitch_P2";
                signal = "Signal_P2";

                //SET PIRATE AXES
                jump = "Jump_P2";
                respawn = "Respawn_P2";

                //SET MENU AXES
                submit = "Submit_P2";
                pause = "Pause_P2";
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
    [SerializeField] private int phaseTime; //sets the amount of time a phase will take 
    private int currentPhaseTime; //tracks how much time is left in a phase

    private Coroutine phaseTimerRoutine;

    //holds the Parrot and Pirate gameObjects for both players
    [SerializeField] private CaptainPirate pirateP1;
    [SerializeField] private CaptainPirate pirateP2;
    [SerializeField] private Parrot parrotP1;
    [SerializeField] private Parrot parrotP2;
    [SerializeField] private GameObject parrotSpawn;
    [SerializeField] private GameObject hud;
    [SerializeField] private GameObject parrotNotif;


    private PlayerState currentPlayer1State = PlayerState.CAPTAIN;
    private PlayerState currentPlayer2State = PlayerState.PARROT;

    //Cameras
    PirateCam captainCamera1;
    PirateCam captainCamera2;
    ParrotCam parrotCamera1;
    ParrotCam parrotCamera2;

    // debugging bools
    [SerializeField]
    private bool parrotDebug;

	// transitional parrots
	private GameObject parrotFlyover1;
	private GameObject parrotFlyover2;
	private bool flyBy = false;

    //HUD

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
    public float PhaseTime
    {
        get
        {
            return phaseTime;
        }
    }

    public int CurrentPhaseTime
    {
        get { return currentPhaseTime; }
    }

    public bool SignalOn
    {
        get { return signalOn; }
        set { signalOn = value; }
    }

    public GameObject HUD
    {
        get { return hud; }
        set
        {
            hud = value;
        }
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

		parrotFlyover1 = GameObject.FindGameObjectWithTag ("Parrot1Flyover");
		parrotFlyover2 = GameObject.FindGameObjectWithTag ("Parrot2Flyover");

		parrotFlyover1.active = false;
		parrotFlyover2.active = false;

        //set p1 to pirate and p2 to parrot
        if (!parrotDebug)
        {
            pirateP1.PirateActive = true;
            parrotP1.active = false;
            currentPlayer1State = PlayerState.CAPTAIN;

            pirateP2.PirateActive = false;
            parrotP2.active = true;
            currentPlayer2State = PlayerState.PARROT;
        }
        else
        {
            phaseTime = 500;
            pirateP1.PirateActive = false;
            parrotP1.active = true;
            currentPlayer1State = PlayerState.PARROT;

            pirateP2.PirateActive = true;
            parrotP2.active = false;
            currentPlayer2State = PlayerState.CAPTAIN;
        }

       phaseTimerRoutine = StartCoroutine(PhaseTimer());

    }

    void Update()
    {
		if (flyBy) {
			// player 1 is about to switch
			if (currentPlayer1State == PlayerState.CAPTAIN) {
				parrotFlyover1.active = true;
				parrotFlyover1.transform.position += new Vector3 (.1f * parrotFlyover1.transform.forward.x, .1f * parrotFlyover1.transform.forward.y, .1f * parrotFlyover1.transform.forward.z);
			} else if (currentPlayer2State == PlayerState.CAPTAIN) {
				parrotFlyover2.active = true;
				parrotFlyover2.transform.position += new Vector3 (.1f * parrotFlyover2.transform.forward.x, .1f * parrotFlyover2.transform.forward.y, .1f * parrotFlyover2.transform.forward.z);
			}
		} else {
			parrotFlyover1.transform.localPosition = new Vector3 (-.05f, 2.34f, -5.6f);
			parrotFlyover2.transform.localPosition = new Vector3 (-.05f, 2.34f, -5.6f);

			parrotFlyover1.active = false;
			parrotFlyover2.active = false;


		}

    }

	//private void FlyBy()
	/*{
		// player 1 is about to switch
		if (currentPlayer1State == PlayerState.CAPTAIN)
		{
			parrotFlyover1.active = true;
			parrotFlyover1.transform.position += new Vector3(.5f * parrotFlyover1.transform.forward.x, .5f * parrotFlyover1.transform.forward.y, .5f * parrotFlyover1.transform.forward.z);
		} 
		else if (currentPlayer2State == PlayerState.CAPTAIN) 
		{
			parrotFlyover2.active = true;
			parrotFlyover2.transform.position += new Vector3(2 * parrotFlyover2.transform.forward.x, 2 * parrotFlyover2.transform.forward.y, 2 * parrotFlyover2.transform.forward.z);
		}
	}*/

    /// <summary>
    /// Swaps which state the players are in and resets phase timer
    /// </summary>
    private void SwitchPhase()
    {
        //turn off signal beams
        StopSignalBeam();
        signalOn = false;

        switch (currentPlayer1State)
        {
            //if current state is captain then switch to parrot
            case PlayerState.CAPTAIN:

                //Recenter parrot Camera
                parrotCamera1.Recenter();

                //set pirate to inactive
                pirateP1.PirateActive = false;

                //freeze pirates rigidbody
                pirateP1.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;

                //set parrot to active
                parrotP1.active = true;


                currentPlayer1State = PlayerState.PARROT;

                
                break;

            //If state is parrot swap to pirate state
            case PlayerState.PARROT:
                //Recenter pirate Cam
                captainCamera1.Recenter();

                //set pirate 1 to active
                pirateP1.PirateActive = true;

                //freeze the rigidbody rotation
                pirateP1.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;

                //set parrot 1 to inactive
                parrotP1.active = false;

                //set current state to pirate
                currentPlayer1State = PlayerState.CAPTAIN;
                break;
        }

        switch (currentPlayer2State)
        {
            case PlayerState.CAPTAIN:
                //Recenter parrot Camera
                parrotCamera2.Recenter();

                //set pirate to inactive
                pirateP2.PirateActive = false;

                //freeze pirates rigidbody
                pirateP2.RBody.constraints = RigidbodyConstraints.FreezeAll;

                //set parrot to active
                parrotP2.active = true;


                currentPlayer2State = PlayerState.PARROT;
                break;

            case PlayerState.PARROT:
                //Recenter pirate Cam
                captainCamera2.Recenter();

                //set pirate 2 to active
                pirateP2.PirateActive = true;

                //freeze the rigidbody rotation
                pirateP2.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;

                //set parrot 2 to inactive
                parrotP2.active = false;

                //set current state to pirate
                currentPlayer2State = PlayerState.CAPTAIN;
                break;
        }

        //turn off signal beams
        StopSignalBeam();
    }


    private IEnumerator PhaseTimer()
    {
        
        for (currentPhaseTime = phaseTime; CurrentPhaseTime > 0; currentPhaseTime--)
        {
            yield return new WaitForSeconds(1);


			if (currentPhaseTime < 5)
				flyBy = true;

            //counterbalance phase timer if we are in the pasue menu
            if (MenuManager.Instance.MenuEnabled)
            {
                currentPhaseTime++;
            }
        }

		flyBy = false;
        SwitchPhase();
        phaseTimerRoutine = StartCoroutine(PhaseTimer());
    }
		

    public IEnumerator SignalBeam(string name)
    {
        //turn on signal beams
        signalOn = true;

        if (pirateP1.PirateActive)
            signal1.Play();
        else if (pirateP2.PirateActive)
            signal2.Play();

        //treasureSignal.Play(); //The parrot probably shouldn't do this

        yield return new WaitForSeconds(10);

        //turn off signal Beams
        signalOn = false;

        if (pirateP1.PirateActive)
            signal1.Stop();
        else if (pirateP2.PirateActive)
            signal2.Stop();

        //treasureSignal.Stop();
    }

    public IEnumerator TreasureSignalBeam()
    {
        //turn on signal beams
        signalOn = true;
        treasureSignal.Play();

        yield return new WaitForSeconds(10);

        //turn off signal Beams
        signalOn = false;
        treasureSignal.Stop();
    }

    public void StopSignalBeam()
    {
        signalOn = false;
        treasureSignal.Stop();
        signal1.Stop();
        signal2.Stop();
    }
    /// <summary>
    /// This Method is called when the game is over and should handle all task for ending the game.
    /// </summary>
    public void EndGame()
    {
        StopAllCoroutines();
    }
}
