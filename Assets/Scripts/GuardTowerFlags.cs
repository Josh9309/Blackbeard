using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardTowerFlags : MonoBehaviour {
	private GameObject redFlag;
	private SkinnedMeshRenderer redMesh;
	private bool redEnabled;
	private GameObject blueFlag;
	private SkinnedMeshRenderer blueMesh;
	private bool blueEnabled;
	private float height; //The height of this object's collider
	private float originalYPosition; //The original Y position of this object
	private bool activatedAtLeastOnce; //If the trap has been activated at least once

	private void Start() //Use this for initialization
	{
		GameObject[] flags = GameObject.FindGameObjectsWithTag("Flag");

		foreach (GameObject flag in flags)
		{
			if (name == "Collider1")
				if (flag.name.Contains("Red1"))
				{
					redFlag = flag;
					originalYPosition = redFlag.transform.position.y;
					redMesh = redFlag.GetComponent<SkinnedMeshRenderer>();
					height = redFlag.GetComponent<Collider>().bounds.size.y - .1f;
					redMesh.enabled = false;
				}
				else if (flag.name.Contains("Blue1"))
				{
					blueFlag = flag;
					blueMesh = blueFlag.GetComponent<SkinnedMeshRenderer>();
					blueMesh.enabled = false;
				}
			else if (name == "Collider2")
				if (flag.name.Contains("Red2"))
				{
					redFlag = flag;
					originalYPosition = redFlag.transform.position.y;
					redMesh = redFlag.GetComponent<SkinnedMeshRenderer>();
					height = redFlag.GetComponent<Collider>().bounds.size.y - .1f;
					redMesh.enabled = false;
				}
				else if (flag.name.Contains("Blue2"))
				{
					blueFlag = flag;
					blueMesh = blueFlag.GetComponent<SkinnedMeshRenderer>();
					blueMesh.enabled = false;
				}
		}

		redEnabled = false;
		blueEnabled = false;
	}

	private void Update()
	{
		if (redEnabled)
			redFlag.transform.position = Vector3.Slerp(redFlag.transform.position, new Vector3(redFlag.transform.position.x, originalYPosition + height, redFlag.transform.position.z), Time.deltaTime * 2);
		else if (blueEnabled)
			blueFlag.transform.position = Vector3.Slerp(blueFlag.transform.position, new Vector3(blueFlag.transform.position.x, originalYPosition + height, blueFlag.transform.position.z), Time.deltaTime * 2);
	}

	void OnTriggerStay(Collider coll)
	{
		if (coll.name == "CaptainPlayer1")
		{
			redMesh.enabled = true;
			redEnabled = true;
		}
		else if (coll.name == "CaptainPlayer2")
		{
			blueMesh.enabled = true;
			blueEnabled = true;
		}
	}
}
