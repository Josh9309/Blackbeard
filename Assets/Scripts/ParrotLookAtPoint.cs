using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParrotLookAtPoint : MonoBehaviour {
    [SerializeField]
    GameObject parrot;
    [SerializeField]
    private float forward;

    [SerializeField] private float heightOffset = 0.192f;

    public float Forward
    {
        set { forward = value; }
    }
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = new Vector3(parrot.transform.position.x, parrot.transform.position.y + heightOffset, parrot.transform.position.z);
        transform.forward = new Vector3(parrot.transform.forward.x, forward, parrot.transform.forward.z);

        Debug.DrawLine(transform.position, transform.forward.normalized + transform.position, Color.blue);
       // Debug.Log(parrot.name + " Forward: " + parrot.transform.forward);
	}
}
