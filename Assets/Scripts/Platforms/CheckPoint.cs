using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour {

    [SerializeField] private int checkPointNum;
	private GameObject redFlag;
	private SkinnedMeshRenderer redMesh;
	private GameObject blueFlag;
	private SkinnedMeshRenderer blueMesh;
	private float height; //The height of this object's collider
	private float originalYPosition; //The original Y position of this object
	private bool activatedAtLeastOnce; //If the trap has been activated at least once

	public int CheckPointNum
	{
		get { return checkPointNum; }
	}

	private void Start() //Use this for initialization
	{
		GameObject[] flags = GameObject.FindGameObjectsWithTag("Flag");

		foreach (GameObject flag in flags)
		{
			if (flag.name.Contains("Red"))
			{
				redFlag = flag;
				redMesh = redFlag.GetComponent<SkinnedMeshRenderer>();
				redMesh.enabled = false;
			}
			else if (flag.name.Contains("Blue"))
			{
				blueFlag = flag;
				blueMesh = blueFlag.GetComponent<SkinnedMeshRenderer>();
				blueMesh.enabled = false;
			}
		}

		height = redFlag.GetComponent<Collider>().bounds.size.y - .1f;
		originalYPosition = transform.position.y;
		activatedAtLeastOnce = false;
	}

	private void Update()
	{
		if (redMesh.enabled)
			redFlag.transform.position = Vector3.Slerp(transform.position, new Vector3(transform.position.x, originalYPosition + height, transform.position.z), Time.deltaTime * 2);
		else if (blueMesh.enabled)
			blueFlag.transform.position = Vector3.Slerp(transform.position, new Vector3(transform.position.x, originalYPosition + height, transform.position.z), Time.deltaTime * 2);
	}

	void OnTriggerEnter(Collider coll)
	{
		if (coll.name.Contains("CaptainPirate1"))
			redMesh.enabled = true;
		else if (coll.name.Contains("CaptainPirate2"))
			blueMesh.enabled = true;
	}
}
