using UnityEngine;
using System.Collections;

public class Path : MonoBehaviour {
    //attributes
    private string PathName;
    private WayPoint startNode; //which waypoint do you start at
    [SerializeField] private WayPoint[] pathWayPoints;
    public bool debug = false;

    #region properties
    public WayPoint[] PathWayPoints
    {
        get { return pathWayPoints; }
    }

    public WayPoint StartNode
    {
        get { return startNode; }
    }
    #endregion
    // Use this for initialization
    void Start () 
    {
        //gets waypoint nodes under the path gameobject
        pathWayPoints = GetComponentsInChildren<WayPoint>();
        
        
        //*DEBUG CHECK WHICH POINTS ARE IN PATHWAYPOINTS ARRAY
        
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

#if UNITY_EDITOR
        //VISUALIZE GROUND CHECK WHEN IN UNITY EDITOR   
        //for (int i = 0; i < pathWayPoints.Length; i++)
        //{
        //    Gizmos.DrawWireSphere(pathWayPoints[i].transform.position, pathWayPoints[i].Range);
        //}
#endif
        if (debug)
        {
            for (int i = 0; i < pathWayPoints.Length; i++)
            {
                Gizmos.DrawSphere(pathWayPoints[i].transform.position, pathWayPoints[i].Range);
            }
        }
    }

}
