using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour {

    GameManager gm;

    int parrotTimer;
    int pirateTimer;
    public Text timer;
	// Use this for initialization
	void Start () {
        gm = GameManager.Instance;
        parrotTimer = (int)gm.ParrotPhaseTime;
        pirateTimer = (int)gm.PiratePhaseTime;
	}
	
	// Update is called once per frame
	void Update () {

        for (int i = pirateTimer; i >= 0; i--)
        {
            timer.text = "Timer: " + pirateTimer;
            Debug.Log(timer.text);
            
        }


      //  InvokeRepeating("Time", 1f, 1f);
    }

    void Time()
    {
        if(parrotTimer >= 0)
        {
            parrotTimer--;
            timer.text = "Timer: " + pirateTimer;
        }
    }
}
