using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour {

    GameManager gm;

    public Text timer;
    public Text bigTime;
    public Text bigTime2;

	// Use this for initialization
	void Start () {
        gm = GameManager.Instance;
	}

	void Update () {
        bigTime.text = " ";
        bigTime2.text = " ";

        timer.color = Color.white;
        timer.text = "" + gm.CurrentPhaseTime;
        timer.fontSize = 60;
        if(gm.CurrentPhaseTime <= 5 && gm.CurrentPhaseTime >= 0)
        {
            timer.text = " " + gm.CurrentPhaseTime;
            timer.color = Color.red;
            timer.fontSize = 80;

            bigTime.text = " " + gm.CurrentPhaseTime;
            bigTime.color = new Color32(0, 0, 0, 100);
            bigTime.fontSize = 300;

            bigTime2.text = " " + gm.CurrentPhaseTime;
            bigTime2.color = new Color32(0, 0, 0, 100);;
            bigTime2.fontSize = 300;


        }
    }
}
