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
            //get winning pirate
            CaptainPirate captain = pirate.gameObject.GetComponent<CaptainPirate>();

            //open that chest boi
            SoundManager.Instance.PlaySfx("chestOpening", 300);

            //goto win screen of menu
            MenuManager.Instance.MenuEnabled = true;
            GameManager.Instance.HUD.GetComponent<Canvas>().enabled = (!GameManager.Instance.HUD.GetComponent<Canvas>().enabled); //turn off hud
            MenuManager.Instance.GoToScreen("GameOverWindow");
            MenuManager.Instance.Screens["GameOverWindow"].GetComponent<GameOverWindow>().SetWinner(captain.PlayerNum); //set winner text

            //kev will kill this line and its entire extended family Debug.Loss
            Debug.Log("Player " + captain.PlayerNum + " Wins!!!");

            gm.EndGame();
        }
    }
}
