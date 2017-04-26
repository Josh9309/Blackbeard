using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdFly : MonoBehaviour {

    [SerializeField]
    GameObject bird;
    Vector3 pos;
	// Use this for initialization
	void Start () {
        pos = bird.transform.position;
	}
	
	// Update is called once per frame
	void Update () {

        if (pos.x < 150)
        {
            pos.x++;
        }
        else
        {
            pos.x = -220;
        }
            bird.transform.position = pos;
            
       
            
	}
}
