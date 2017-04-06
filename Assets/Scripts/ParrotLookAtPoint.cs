using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParrotLookAtPoint : MonoBehaviour {
    [SerializeField]
    GameObject parrot;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = parrot.transform.position;
        transform.forward = new Vector3(parrot.transform.forward.x, 0, parrot.transform.forward.z);
       // Debug.Log(parrot.name + " Forward: " + parrot.transform.forward);
	}
}
