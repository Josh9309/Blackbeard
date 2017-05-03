using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Compass : MonoBehaviour {
    #region Fields
    [SerializeField] private GameObject treasure;
    #endregion

    #region Start and Update
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        FaceTreasure();
	}
    #endregion

    #region Helper Methods
    private void FaceTreasure()
    {
        //look at treasure
        this.gameObject.transform.LookAt(treasure.transform);

    }
    #endregion
}
