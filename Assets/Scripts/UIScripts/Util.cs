using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Util : MonoBehaviour {

    //fields
    private Image coolDownMeter;
    [SerializeField] private GameObject ply;

	private float time;
	private float cooldown;
	bool couldUseUtil;
	bool canUseUtil;

    // Use this for initialization
    void Start () {
        //assign our image
        coolDownMeter = this.gameObject.GetComponent<Image>();

		//setup timer variables bc yall like your corutines too much
		time = 0;
		couldUseUtil = ply.GetComponent<Parrot>().CanDrop;
		canUseUtil = ply.GetComponent<Parrot>().CanDrop;
		cooldown = ply.GetComponent<Parrot> ().DropCoolDown; //get cooldown time from parrot
    }
	
	// Update is called once per frame
	void Update () {
		

        //run timer when a util is used
		couldUseUtil = canUseUtil; //could we use a util last frome?
		canUseUtil = ply.GetComponent<Parrot>().CanDrop; //can we currently use a util?

		if (couldUseUtil && !canUseUtil) 
		{ //check if we just used a util(we could use it a second ago, but not now)
			cooldown = ply.GetComponent<Parrot> ().DropCoolDown; //get cooldown time from parrot
			time = 0; //reset values
		} 
		else if (!canUseUtil) 
		{
			//update timer
			time += Time.deltaTime;

			//fill cooldown meter bit by bit
			coolDownMeter.fillAmount = time / cooldown;
		} 
	}

	void UpdateCoolDown()
	{
	}

}
