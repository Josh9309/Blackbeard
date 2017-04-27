using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Util : MonoBehaviour {

    //fields
	#region fields
	[SerializeField] private GameObject ply; //parrot

	//cooldown fields
    private Image coolDownMeter;
	private float time;
	private float cooldown;
	bool couldUseUtil;
	bool canUseUtil;

	//util num uses fields
	private Vector2 fullScale;
	private int totalLanterns;
	private Color32 colorSave;

	//util icon fields
	[SerializeField] private List<Sprite> utilImages;
	[SerializeField] private Image currentUtil;
	#endregion

	#region inBuiltMethods
    // Use this for initialization
    void Start () {
        //assign our image for cooldown
        coolDownMeter = this.gameObject.GetComponent<Image>();

		//setup timer variables bc yall like your corutines too much
		time = 0;
		couldUseUtil = ply.GetComponent<Parrot>().CanDrop;
		canUseUtil = ply.GetComponent<Parrot>().CanDrop;
		cooldown = ply.GetComponent<Parrot> ().DropCoolDown; //get cooldown time from parrot
		currentUtil.sprite = utilImages[0]; //set inital utility

		//set fields for num of uses
		fullScale = coolDownMeter.rectTransform.sizeDelta; //original scale
		totalLanterns = ply.GetComponent<Parrot>().NumLanterns; //total number of lanterns(whatever num they ahve at first)
		colorSave = coolDownMeter.color; //original color

    }
	
	// Update is called once per frame
	void Update () {
		//set util images based on what the parrot is using
		currentUtil.sprite = utilImages[ply.GetComponent<Parrot>().CurrentUtilityID];

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

        //scale util cooldown based on numLantern
        if (ply.GetComponent<Parrot>().CurrentUtilityID < 1) //lantern
        {
            int dif = (totalLanterns - ply.GetComponent<Parrot>().NumLanterns); //difference bwteen total num lanterns and current num

            //check if we dont have anymore lanterns
            if (ply.GetComponent<Parrot>().NumLanterns > 0)
            {
                //scale
                coolDownMeter.rectTransform.sizeDelta = fullScale - (new Vector2(15 * dif, 15 * dif));
            }
            else
            {
                //fullscale but transparent
                coolDownMeter.rectTransform.sizeDelta = fullScale;
                coolDownMeter.color = new Color32(146, 146, 146, 225); //grey
            }
        }
        else
        {
            //reset scale  and color bc we dont need it for the other utils
            coolDownMeter.rectTransform.sizeDelta = fullScale;
            coolDownMeter.color = colorSave;
        }
	}
	#endregion

	#region helperMethods
	#endregion
}
