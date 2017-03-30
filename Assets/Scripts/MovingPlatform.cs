using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour {

    #region Attributes
    [SerializeField] private float platformSpeed;
    [SerializeField] private Path platformPath;
    [SerializeField] private WayPoint currentPoint;
    [SerializeField] private bool canMoveVertical = false;
    [SerializeField] private bool canMoveHorizontal = true;

    private Rigidbody rBody;

    private Vector3 ultimateForce;
    private Vector3 velocity;
    private Vector3 acceleration;
    #endregion

    // Use this for initialization
    void Start () {
        rBody = GetComponent<Rigidbody>();

        Physics.IgnoreLayerCollision(8, 9); //ingnore collisions with the parrots
	}
	
	// Update is called once per frame
	void Update () {
        ultimateForce = Vector3.zero;

        //Check Current Point
        CheckCurrentPoint();

        //call PathFollow using currentPoint;
        ultimateForce = PathFollow(currentPoint.WayPointNum);
        //limit the ultimate steering force
        ultimateForce = Vector3.ClampMagnitude(ultimateForce, 10.0f);

        //apply that acceleration 
        acceleration = acceleration + (ultimateForce / rBody.mass);

        //add accel to velocity
        velocity += acceleration * Time.deltaTime;
        velocity.y = 0; //keeps us on the same plane

        //limit velocity to max speed
        velocity = Vector3.ClampMagnitude(velocity, platformSpeed);

        transform.forward = velocity.normalized;

        rBody.velocity = velocity;

        acceleration = Vector3.zero;
    }

    private void CheckCurrentPoint()
    {
        //Check if Vehicle is within range of that point
        Vector3 VecToCenter = currentPoint.transform.position - transform.position;
        float dist = VecToCenter.magnitude;
        if (dist < currentPoint.Range)
        {
            //Get next Point To seek
            for (int p = 0; p < platformPath.PathWayPoints.Length; p++)
            {
                WayPoint NextPoint = platformPath.PathWayPoints[p];
                if (NextPoint.WayPointNum == currentPoint.NextPoint)
                {
                    currentPoint = platformPath.PathWayPoints[p];
                    break;
                }
            }
        }
        else
        {
            //Do Nothing
        }
    }

    protected Vector3 PathFollow(int currentPoint)
    {
        Vector3 ultimatePathFollowForce = Vector3.zero;

        //find Current Path Point to seek
        for (int i = 0; i < platformPath.PathWayPoints.Length; i++)
        {
            WayPoint point = platformPath.PathWayPoints[i];
            if (point.WayPointNum == currentPoint)
            {
                ultimatePathFollowForce = Arrive(platformPath.PathWayPoints[i].transform.position, 10.0f);
                Debug.DrawLine(transform.position, platformPath.PathWayPoints[i].transform.position, Color.white);
            }
        }


        return ultimatePathFollowForce;
    }

    protected Vector3 Seek(Vector3 targetPosition)
    {
        Vector3 desired = targetPosition - transform.position;
        desired = desired.normalized * platformSpeed;
        desired -= velocity;
        if (!canMoveVertical)
        {
            desired.y = 0;
        }

        if (!canMoveHorizontal)
        {
            desired.x = 0;
            desired.z = 0;
        }
        return desired;
    }

    protected Vector3 Arrive(Vector3 targetPosition, float slowRadius)
    {
        //calculate the desired velocity
        Vector3 desired = targetPosition - transform.position;
        float dist = desired.magnitude;

        //check the distance to detect wther the character is inside the slowing area
        if (dist < slowRadius)
        {
            //inside slowing area
            desired = desired.normalized * platformSpeed * (dist / slowRadius);
        }
        else
        {
            //outside slowing area
            desired = desired.normalized * platformSpeed;
        }

        desired -= velocity;
        desired.y = 0;
        return desired;
    }

    protected void ApplyForce(Vector3 steeringForce)
    {
        acceleration = acceleration + (steeringForce / rBody.mass);
    }
}
