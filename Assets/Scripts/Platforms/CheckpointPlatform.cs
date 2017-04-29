using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointPlatform : MonoBehaviour
{
    #region Attributes
    [SerializeField]
    private float platformSpeed;
    [SerializeField]
    private Path platformPath;
    [SerializeField]
    private WayPoint currentPoint;
    [SerializeField]
    private bool canMoveVertical = false;
    [SerializeField]
    private bool canMoveHorizontal = true;
    [SerializeField]
    bool p1Checkpoint; //is this checkpoint for player 1

    private Rigidbody rBody;

    private Vector3 ultimateForce;
    private Vector3 velocity;
    private Vector3 acceleration;
    private bool playerActivated = true;
    private bool platformActive = false;
    private bool inPirateState = false;
    #endregion

    #region Properties
    public bool Active
    {
        get { return playerActivated; }
        set { playerActivated = value; }
    }

    public bool P1Checkpoint
    {
        get { return p1Checkpoint; }
    }
    #endregion
    // Use this for initialization
    void Start()
    {
        rBody = GetComponent<Rigidbody>();

        playerActivated = false;
        platformActive = false;

        Physics.IgnoreLayerCollision(8, 9); //ingnore collisions with the parrots
    }

    // Update is called once per frame
    void Update()
    {
        //check if this checkpoints player is active as the pirate
        if (p1Checkpoint && GameManager.Instance.CurrentPlayer1State == GameManager.PlayerState.CAPTAIN)
        {
            inPirateState = true;
        }
        else if (!p1Checkpoint && GameManager.Instance.CurrentPlayer2State == GameManager.PlayerState.CAPTAIN)
        {
            inPirateState = true;
        }
        else
        {
            inPirateState = false;
        }

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
        //velocity.y = 0;

        //limit velocity to max speed
        velocity = Vector3.ClampMagnitude(velocity, platformSpeed);

        //transform.forward = velocity.normalized;
        if (!canMoveVertical)
        {
            velocity.y = 0;
        }

        if (!canMoveHorizontal)
        {
            velocity.x = 0;
            velocity.z = 0;
        }
        rBody.velocity = velocity;

        acceleration = Vector3.zero;
    }

    private void OnCollisionStay(Collision col)
    {
        if (col.collider.tag == "Pirate")
        {
            platformActive = true;
        }
    }

    private void OnCollisionExit(Collision col)
    {
        platformActive = false;
    }

    private void CheckCurrentPoint()
    {
        if (playerActivated && platformActive && inPirateState) //if platform is active proceed as normal.
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
        }
        else if (currentPoint.StartPoint != true)
        {
            for (int i = 0; i < platformPath.PathWayPoints.Length; i++)
            {
                if (platformPath.PathWayPoints[i].StartPoint)
                {
                    currentPoint = platformPath.PathWayPoints[i];
                }
            }
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
        return desired;
    }

    protected void ApplyForce(Vector3 steeringForce)
    {
        acceleration = acceleration + (steeringForce / rBody.mass);
    }
}
