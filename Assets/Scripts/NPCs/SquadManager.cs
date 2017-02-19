using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquadManager : MonoBehaviour {

    #region Attributes
    private List<GameObject> pirates;
    [SerializeField]
    private int maxPirates;
    private FSM fsm;
    private float squadRadius;
    private float engagementRadius;
    #endregion

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        fsm.Update();
	}

    #region Helper Methods
    /// <summary>
    /// Helper method for calculating the distance from the NPC's current position
    /// to a given target
    /// </summary>
    /// <param name="targetPos">position of desired target</param>
    /// <returns>distance to targetPos parameter</returns>
    private Vector3 CalcDistance(Vector3 targetPos)
    {
        return targetPos - transform.position;
    }

    /// <summary>
    /// Method to calulate the center of the flock. The SquadManager itself will occupy this spot
    /// </summary>
    private void CalcCentroid()
    {
        
    }
    #endregion
}
