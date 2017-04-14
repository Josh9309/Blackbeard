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
	}

	void Update () {

        if( gm.CurrentPlayer1State == GameManager.PlayerState.CAPTAIN)
        {
            timer.color = Color.white;
            timer.text = "Pirate Phase: " + gm.CurrentPirateTime;
            if(gm.CurrentPirateTime <= 5 && gm.CurrentPirateTime >=0)
            {
                timer.text = "Phase change in " + gm.CurrentPirateTime;
                timer.color = Color.red;
                timer.fontSize = 40;
            }
        }
        else
        {
            timer.color = Color.white;
            timer.text = "Parrot Phase: " + gm.CurrentParrotTime;
            if (gm.CurrentParrotTime <= 5 && gm.CurrentParrotTime >= 0)
            {
                timer.text = "Phase change in " + gm.CurrentParrotTime;
                timer.color = Color.red;
                timer.fontSize = 40;
            }
        }
    }
}
