using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Util : MonoBehaviour {

    //fields
    private Image coolDownMeter;
    [SerializeField] private GameObject ply;

    // Use this for initialization
    void Start () {
        //assign our image
        coolDownMeter = this.gameObject.GetComponent<Image>();
    }
	
	// Update is called once per frame
	void Update () {
        //define how much shows by the cooldown from parrot
        float amount = ply.GetComponent<Parrot>().DropCoolDown / 100;
        coolDownMeter.fillAmount = 100 - amount;
	}
}
