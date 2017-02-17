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
    private delegate void State();
    private State currentState;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}


