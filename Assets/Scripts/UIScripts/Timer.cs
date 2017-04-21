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

        timer.color = Color.white;
        timer.text = "" + gm.CurrentPhaseTime;
        timer.fontSize = 60;
        if(gm.CurrentPhaseTime <= 5 && gm.CurrentPhaseTime >= 0)
        {
            timer.text = " " + gm.CurrentPhaseTime;
            timer.color = Color.red;
            timer.fontSize = 80;
        }
    }
}
