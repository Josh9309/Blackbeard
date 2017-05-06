using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraMode : MonoBehaviour {
    #region Fields
    [SerializeField] private Text notif;
    [SerializeField] private float visibleTime;
    private float timer;
    #endregion

    #region Start and Update
	// Use this for initialization
	void Start () {
        notif.text = " ";
	}
	
	// Update is called once per frame
	void Update () {
        //update text
        if (visibleTime > 0)
        {
            //remove some time
            visibleTime -= Time.deltaTime;
        }
        else if(visibleTime > -10.0f) //reset if below zero
        {
            Reset();
        }
        //dont do anything after reset
		
	}
    #endregion

    #region Helper Methods
    public void ShowText(string text, float seconds)
    {
        //turn me oonnn
        this.gameObject.SetActive(true);

        //set timer
        visibleTime = seconds;

        //set string
        notif.text = text;
    }

    private void Reset()
    {
        visibleTime = -100.00f;
        notif.text = " "; //remove text
        this.gameObject.SetActive(false); //goodnight sweet prince
    }
    #endregion
}
