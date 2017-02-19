using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is an implementation of the parent NPC class and represents
/// an NPC on the Red Team
/// </summary>
public class RedNPC : NPC {

    #region Attributes
    // temporary storage for initial array of enemies, for parsing into a list
    // in the start method
    private GameObject [] enemiesArray;
    #endregion

    // Use this for initialization
    protected override void Start () {
        base.Start();

        // assign enemy list - this will contain all Blue NPCs
        enemiesArray = GameObject.FindGameObjectsWithTag("Blue");

        // parse array into list
        for (int i = 0; i < enemiesArray.Length; i++)
        {
            enemies.Add(enemiesArray[i]);
        }
	}
	
	// Update is called once per frame
	protected override void Update () {
        base.Update();
	}
}
