using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Physics.IgnoreLayerCollision(0, 4); //ignores every collision in default layer
        Physics.IgnoreLayerCollision(4, 8); //ignores every collision in moving platform layer
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == "Pirate")
        {
            col.gameObject.GetComponent<CaptainPirate>().Respawn();


            if (Random.Range(0, 100) > 80)
            {
                SoundManager.Instance.PlaySfx("drowning");
            }
            else
            {
                SoundManager.Instance.PlaySfx("splash");
            }
        }
    }
}
