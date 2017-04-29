using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour {

    [SerializeField] CheckpointPlatform checkPlatform;
    

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider col)
    {
        if(col.tag == "Pirate")
        {
            CaptainPirate pirate = col.gameObject.GetComponent<CaptainPirate>();

            if(checkPlatform.P1Checkpoint && pirate.PlayerNum == 1)
            {
                checkPlatform.Active = true;
            }
            else if(!checkPlatform.P1Checkpoint && pirate.PlayerNum == 2)
            {
                checkPlatform.Active = true;
            }
        }
        
    }
}
