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

        timer.color = Color.gray;
        timer.text = "Phase Time: " + gm.CurrentPhaseTime;
        timer.fontSize = 60;
        if(gm.CurrentPhaseTime <= 5 && gm.CurrentPhaseTime >= 0)
        {
            timer.text = "Phase change in " + gm.CurrentPhaseTime;
            timer.color = Color.gray;
            timer.fontSize = 80;
        }
    }
}
