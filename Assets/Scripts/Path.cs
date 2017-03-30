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
        GameObject[] nodes = GameObject.FindGameObjectsWithTag("platformNode");
        pathWayPoints = new WayPoint[nodes.Length];

        for (int i = 0; i < nodes.Length; i++)
        {
            pathWayPoints[i] = nodes[i].GetComponent<WayPoint>();
        }

        //get the wayPoints and set the start node in the path
        //for (int i = 0; i < pathWayPoints.Length; i++)
        //{
        //    WayPoint point = pathWayPoints[i];
        //    Debug.Log(point.WayPointNum);
        //    if (point.StartPoint == true)
        //    {
        //        Debug.Log(PathName + " Start point is: " + point.WayPointNum);
        //        startNode = pathWayPoints[i];
        //    }
        //}
 
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
