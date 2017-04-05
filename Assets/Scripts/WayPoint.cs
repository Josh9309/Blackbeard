using UnityEngine;
using System.Collections;

public class WayPoint : MonoBehaviour {
    //attributes
    [SerializeField] private float range = 5.0f;
    [SerializeField] bool startPoint = false;
    [SerializeField] int wayPointNum;
    [SerializeField] int nextPoint;
    public bool debug = true;
    #region Properties
    public int WayPointNum
    {
        get { return wayPointNum; }
    }

    public int NextPoint
    {
        get { return nextPoint; }
    }
    public bool StartPoint
    {
        get { return startPoint; }
    }

    public float Range
    {
        get { return range; }
    }
    #endregion

    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
#if UNITY_EDITOR
        if(debug == true)
        Gizmos.DrawWireSphere(transform.position, range);
#endif
    }
}
