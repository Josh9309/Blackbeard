using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitScreenCamera : MonoBehaviour
{
    [SerializeField] private bool vertical;
    private Camera player1Camera;
    private Camera player2Camera;
    private GameObject parrot1;
    private GameObject parrot2;
    private GameObject pirate1;
    private GameObject pirate2;

    #region Properties
    public bool Vertical
    {
        get
        {
            return vertical;
        }
        set
        {
            vertical = value;
        }
    }
    #endregion

    void Start() //Use this for initialization
    {
        //Get the needed components
        player1Camera = GameObject.FindGameObjectWithTag("Camera1").GetComponent<Camera>();
        player2Camera = GameObject.FindGameObjectWithTag("Camera2").GetComponent<Camera>();
        parrot1 = GameObject.FindGameObjectWithTag("ParrotPlayer1");
        parrot2 = GameObject.FindGameObjectWithTag("ParrotPlayer2");
        //Implement these later
        //pirate1 = GameObject.FindGameObjectWithTag("PiratePlayer1");
        //pirate2 = GameObject.FindGameObjectWithTag("PiratePlayer2");

		if (vertical)
        {
            player1Camera.rect = new Rect(0, 0, .5f, 1);
            player2Camera.rect = new Rect(.5f, 0, .5f, 1);
        }
        else
        {
            player1Camera.rect = new Rect(0, 0, 1, .5f);
            player2Camera.rect = new Rect(0, .5f, 1, .5f);
        }
	}
	
	void Update() //Update is called once per frame
    {
        //For now, look at the parrots that are in the scene
        player1Camera.transform.LookAt(parrot1.transform);
        player2Camera.transform.LookAt(parrot2.transform);
	}
}