using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HudInteraction : MonoBehaviour {
   
    [SerializeField] GameObject parrot1;
    [SerializeField] GameObject parrot2;
    [SerializeField] GameObject trap;
    [SerializeField] Canvas HUD;
	// Use this for initialization
	void Start ()
    {
   
        HUD.GetComponent<Canvas>().enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		 if((Vector3.Distance(parrot1.transform.position, trap.transform.position) < 10) || (Vector3.Distance(parrot2.transform.position, trap.transform.position) < 10))
            {
            HUD.GetComponent<Canvas>().enabled = true;
        }
	}
}
