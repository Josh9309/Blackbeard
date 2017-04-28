using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureGoal : MonoBehaviour {

    private GameManager gm;

	// Use this for initialization
	void Start () {
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider pirate)
    {
        if(pirate.gameObject.tag == "Pirate")
        {
            CaptainPirate captain = pirate.gameObject.GetComponent<CaptainPirate>();
            SoundManager.Instance.PlaySfx("chestOpening");

            Debug.Log("Player " + captain.PlayerNum + " Wins!!!");

            gm.EndGame();
        }
    }
}
