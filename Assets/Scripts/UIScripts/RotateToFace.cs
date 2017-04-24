using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateToFace : MonoBehaviour
{
    //The cameras we will be facing
    private Camera parrotCamera1;
    private Camera parrotCamera2;

	void Start() //Use this for initialization
    {
        SplitScreenCamera ssC = GameObject.FindWithTag("GameManager").GetComponentInChildren<SplitScreenCamera>(); //Get the SplitScreenCamera once
        parrotCamera1 = ssC.ParrotCamera1;
        parrotCamera2 = ssC.ParrotCamera2;
	}
	
	void Update() //Update is called once per frame
    {
        //Determine which camera to look at and look at it
        if (parrotCamera1.enabled)
            transform.LookAt(parrotCamera1.transform);
        else if (parrotCamera2.enabled)
            transform.LookAt(parrotCamera2.transform);
    }
}