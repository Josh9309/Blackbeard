using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is my generic FSM implementation, this is done so that
/// we can reduce repetition
/// </summary>
public class FSM : MonoBehaviour
{
    // declare a delegate to represent a state in the machine
    public delegate void State();
    private State currentState;

    // Update is called once per frame
    public void Update()
    {
        if (currentState != null)
        {
            currentState();
        }
        else
        {
            Debug.Log(this.gameObject.name + ": No assigned state");
        }
    }

    /// <summary>
    /// Changes the current state parameter to a new state that has been passed in
    /// </summary>
    /// <param name="newState">delegate to new state method</param>
    public void SetState(State newState)
    {
        currentState = newState;
    }

    /// <summary>
    /// For use by SquadManager only, this will set the state of the entire squad
    /// </summary>
    /// <param name="newState">delegate to new state method</param>
    /// <param name="squad">list of squad members</param>
    public void SetSquadState(State newState, List<GameObject> squad)
    {
        for (int i = 0; i < squad.Count; i++)
        {
            FSM pirateFsm = squad[i].GetComponent<FSM>();
            pirateFsm.SetState(newState);
        }
    }
}


