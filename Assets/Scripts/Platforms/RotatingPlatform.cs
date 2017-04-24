using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingPlatform : MonoBehaviour {
    private GameObject pivot;
    //[SerializeField] private GameObject characterAnchor;
    [SerializeField] private float rotateAmount = 3.0f; // the amount the platform will rotate by each frame
    [SerializeField] private bool oscilate = false; //determines whether the platform rotates in one direction or if it should oscilate
    [SerializeField] private float rotateDest = 45.0f; //the amount the platform will rotate to and then turn around
    [SerializeField] private bool clockwise = true; //determines whether the platform should rotate clockwise or counter-clockwise

    private float rotatedNum = 0f; //amount Platform has already rotated by
    private Rigidbody rBody;

	// Use this for initialization
	void Start () {
        pivot = transform.FindChild("Pivot").gameObject; //get the pivot for the platform to rotate around
        rBody = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        if (!oscilate)
        {
            if (clockwise)
            {
                transform.parent.RotateAround(pivot.transform.position, transform.up, rotateAmount);
            }
            else
            {
                transform.parent.RotateAround(pivot.transform.position, transform.up, -rotateAmount);
            }
        }
        else
        {
            if (clockwise)
            {
                if(rotatedNum < rotateDest)
                {
                    transform.parent.RotateAround(pivot.transform.position, transform.up, rotateAmount);
                    rotatedNum += rotateAmount;
                }
                else
                {
                    clockwise = false;
                }
            }
            else
            {
                if(rotatedNum > 0)
                {
                    transform.parent.RotateAround(pivot.transform.position, transform.up, -rotateAmount);
                    rotatedNum -= rotateAmount;
                }
                else
                {
                    clockwise = true;
                }
            }
        }

        transform.rotation = transform.parent.rotation;
	}
}
