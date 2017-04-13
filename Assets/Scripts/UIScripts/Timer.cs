using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour {

    GameManager gm;

    public Text timer;
	// Use this for initialization
	void Start () {
        gm = GameManager.Instance;
        //parrotTimer = (int)gm.ParrotPhaseTime;
        //pirateTimer = (int)gm.PiratePhaseTime;
	}
	
	// Update is called once per frame
	void Update () {

        if( gm.CurrentPlayer1State == GameManager.PlayerState.CAPTAIN)
        {
            timer.text = "Timer: " + gm.CurrentPirateTime;
        }
        else
        {
            timer.text = "Time: " + gm.CurrentParrotTime;
        }


      //  InvokeRepeating("Time", 1f, 1f);
    }
}
