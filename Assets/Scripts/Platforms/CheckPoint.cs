using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour {

    [SerializeField] private int checkPointNum;

	public int CheckPointNum
	{
		get { return checkPointNum; }
	}
}
