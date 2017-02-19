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
            Debug.Log("No assigned state");
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
}


