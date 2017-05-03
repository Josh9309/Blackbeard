using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Compass : MonoBehaviour {
    #region Fields
    [SerializeField] private GameObject treasure;
    private GameObject childObj; //object that is being pointed at the treasure
    private bool active;
    private float time; //amount of time the compass is active
    #endregion

    #region Properties
    public bool Active
    {
        get{ return active; }
        set
        {
            active = value;
        }
    }
    #endregion

    #region Start and Update
	// Use this for initialization
    void Start () {
        //intialize fields
        time = 10.00f;	
        childObj = GetComponentInChildren<Transform>().gameObject;

        //turn off initally
        childObj.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
	}
    #endregion

    #region Helper Methods
    public IEnumerator FaceTreasure()
    {
        //turn on compass child
        childObj.SetActive(true);

        //loop until timer runs out
        do
        {
            //look at treasure
            this.gameObject.transform.LookAt(treasure.transform);

            //wait a second
            yield return new WaitForSeconds(.01f);

            //cut down timer
            time -= Time.deltaTime;
        }
        while(time > 0);

        //reset timer
        time = 10.00f;

        //turn off compass child
        childObj.SetActive(false);

    }
    #endregion
}
