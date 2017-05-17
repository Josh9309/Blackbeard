using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitScreenCamera : MonoBehaviour
{
    #region Attributes
    [SerializeField] private bool debug; //Single camera view for debug

    //Player objects
    private CaptainPirate captain1;
    private CaptainPirate captain2;
    private Parrot parrot1;
    private Parrot parrot2;

    //Camera objects
    private Camera captainCamera1;
    private Camera captainCamera2;
    private Camera parrotCamera1;
    private Camera parrotCamera2;

    //HUD placement
    private Transform[] HUD;
    #endregion

    #region Properties
    public Camera CaptainCamera1
    {
        get
        {
            return captainCamera1;
        }
        set
        {
            captainCamera1 = value;
        }
    }
    public Camera CaptainCamera2
    {
        get
        {
            return captainCamera2;
        }
        set
        {
            captainCamera2 = value;
        }
    }
    public Camera ParrotCamera1
    {
        get
        {
            return parrotCamera1;
        }
        set
        {
            parrotCamera1 = value;
        }
    }
    public Camera ParrotCamera2
    {
        get
        {
            return parrotCamera2;
        }
        set
        {
            parrotCamera2 = value;
        }
    }
    #endregion

    void Start() //Use this for initialization
    {
        //Get the game manager
        GameManager gm = GameManager.Instance;

        //set the hud elements
        HUD = GameManager.Instance.HUD.GetComponentsInChildren<Transform>();

        //Get the player objects from the game manager
        captain1 = gm.PirateP1;
        captain2 = gm.PirateP2;
        parrot1 = gm.ParrotP1;
        parrot2 = gm.ParrotP2;

        //Get the camera objects
        captainCamera1 = GameObject.FindGameObjectWithTag("CaptainCamera1").GetComponent<Camera>();
        captainCamera2 = GameObject.FindGameObjectWithTag("CaptainCamera2").GetComponent<Camera>();
        parrotCamera1 = GameObject.FindGameObjectWithTag("ParrotCamera1").GetComponent<Camera>();
        parrotCamera2 = GameObject.FindGameObjectWithTag("ParrotCamera2").GetComponent<Camera>();

        //Splitscreen type
        if (SettingManager.Instance.Vertical)
        {

            //set camera
            captainCamera1.rect = new Rect(0, 0, .5f, 1);
            captainCamera2.rect = new Rect(.5f, 0, .5f, 1);
            parrotCamera1.rect = new Rect(0, 0, .5f, 1);
            parrotCamera2.rect = new Rect(.5f, 0, .5f, 1);

            //place hud at the bottom of the screen
            //HUD[0].localPosition = new Vector3(HUD[0].localPosition.x, (Screen.height - 30), HUD[0].localPosition.z);
            //HUD[1].localPosition = new Vector3(HUD[1].localPosition.x, (Screen.height - 30), HUD[1].localPosition.z);
        }
        else
        {
            //set camera
            captainCamera1.rect = new Rect(0, .5f, 1, .5f);
            captainCamera2.rect = new Rect(0, 0, 1, .5f);
            parrotCamera1.rect = new Rect(0, .5f, 1, .5f);
            parrotCamera2.rect = new Rect(0, 0, 1, .5f);

            //place hud in middle of screen
            //HUD[0].localPosition = new Vector3(HUD[0].localPosition.x, (Screen.height / 2), HUD[0].localPosition.z);
            //HUD[1].localPosition = new Vector3(HUD[1].localPosition.x, (Screen.height / 2), HUD[1].localPosition.z);
        }

        //Recenter all cameras intially
        parrotCamera1.GetComponent<ParrotCam>().Recenter();
        parrotCamera2.GetComponent<ParrotCam>().Recenter();
        captainCamera1.GetComponent<PirateCam>().Recenter();
        captainCamera2.GetComponent<PirateCam>().Recenter();

        //Recenter all cameras intially
        parrotCamera1.GetComponent<ParrotCam>().Recenter();
        parrotCamera2.GetComponent<ParrotCam>().Recenter();
        captainCamera1.GetComponent<PirateCam>().Recenter();
        captainCamera2.GetComponent<PirateCam>().Recenter();

        //Set inital cameras
        if (gm.CurrentPlayer1State == GameManager.PlayerState.CAPTAIN)
        {
            parrotCamera1.enabled = false;
            captainCamera1.enabled = true;
        }
        else
        {
            parrotCamera1.enabled = true;
            captainCamera1.enabled = false;
        }

        if (gm.CurrentPlayer2State == GameManager.PlayerState.CAPTAIN)
        {
            parrotCamera2.enabled = false;
            captainCamera2.enabled = true;
        }
        else
        {
            parrotCamera2.enabled = true;
            captainCamera2.enabled = false;
        }

        if (debug)
        {
            //Resize viewports
            captainCamera1.rect = new Rect(0, 0, 1, 1);
            captainCamera2.rect = new Rect(0, 0, 0, 0);
            parrotCamera1.rect = new Rect(0, 0, 1, 1);
            parrotCamera2.rect = new Rect(0, 0, 0, 0);

            //Deactivate other pirate camera
            captainCamera2.enabled = false;
            parrotCamera2.enabled = false;
        }
    }
	
	void Update() //Update is called once per frame
    {
        SwitchCameras(); //Switches the cameras
        ChangeSplitAxis(); //update how cams split based on SettingsManager
	}

    /// <summary>
    /// Switches the camera between the pirate and the parrot
    /// </summary>
    private void SwitchCameras()
    {
        switch (GameManager.Instance.CurrentPlayer1State)
        {
            case GameManager.PlayerState.CAPTAIN:
                captainCamera1.enabled = true;
                captainCamera2.enabled = false;
                parrotCamera1.enabled = false;
                parrotCamera2.enabled = true;
                break;

            case GameManager.PlayerState.PARROT:
                captainCamera1.enabled = false;
                captainCamera2.enabled = true;
                parrotCamera1.enabled = true;
                parrotCamera2.enabled = false;
                break;
        }
    }

    private void ChangeSplitAxis()
    {
        //Splitscreen type
        if (SettingManager.Instance.Vertical)
        {
            
            //set camera
            captainCamera1.rect = new Rect(0, 0, .5f, 1);
            captainCamera2.rect = new Rect(.5f, 0, .5f, 1);
            parrotCamera1.rect = new Rect(0, 0, .5f, 1);
            parrotCamera2.rect = new Rect(.5f, 0, .5f, 1);

            //place hud at the bottom of the screen
            //HUD[0].localPosition = new Vector3(HUD[0].localPosition.x, (Screen.height - 30) - HUD[0].position.x, HUD[0].localPosition.z);
            //HUD[1].localPosition = new Vector3(HUD[1].localPosition.x, (Screen.height - 30) - HUD[1].position.x, HUD[1].localPosition.z);
        }
        else
        {
            //set camera
            captainCamera1.rect = new Rect(0, .5f, 1, .5f);
            captainCamera2.rect = new Rect(0, 0, 1, .5f);
            parrotCamera1.rect = new Rect(0, .5f, 1, .5f);
            parrotCamera2.rect = new Rect(0, 0, 1, .5f);

            //place hud in middle of screen
            //HUD[0].localPosition = new Vector3(HUD[0].localPosition.x, (Screen.height / 2) - HUD[0].position.x, HUD[0].localPosition.z);
            //HUD[1].localPosition = new Vector3(HUD[1].localPosition.x, (Screen.height / 2) - HUD[1].position.x, HUD[1].localPosition.z);
        }
        if (debug)
        {
            //Resize viewports
            captainCamera1.rect = new Rect(0, 0, 1, 1);
            captainCamera2.rect = new Rect(0, 0, 0, 0);
            parrotCamera1.rect = new Rect(0, 0, 1, 1);
            parrotCamera2.rect = new Rect(0, 0, 0, 0);

            //Deactivate other pirate camera
            captainCamera2.enabled = false;
            parrotCamera2.enabled = false;
        }
    }

}