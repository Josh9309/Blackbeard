using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is my generic FSM implementation, this is done so that we can reduce repetition
/// </summary>
public class FSM : MonoBehaviour
{

    #region Attributes
    // declare a delegate to represent a state in the machine
    public delegate void State();
    private State currentState;
    #endregion

    /// <summary>
    /// This method is NOT called every frame in this class, rather it is called in the 
    /// update method of an NPC or squadManager class
    /// </summary>
    public void UpdateState()
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
}


