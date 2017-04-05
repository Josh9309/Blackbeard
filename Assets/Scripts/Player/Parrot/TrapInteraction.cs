using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapInteraction : MonoBehaviour
{
    #region Attributes
    private List<GameObject> traps;
    private List<BaseTrap> trapScripts;
    private List<float> trapHalfHeight;
    private RaycastHit hit;
    private bool buttonDown;
    private bool inCooldown;
    private Parrot parrot;
    #endregion

    #region InBuiltMethods
    void Start() //Use this for initialization
    {
        parrot = gameObject.GetComponent<Parrot>();

        traps = new List<GameObject>();
        trapScripts = new List<BaseTrap>();
        trapHalfHeight = new List<float>();

        GameObject[] foundTraps = GameObject.FindGameObjectsWithTag("Trap");
        for (int i = 0; i < foundTraps.Length; i++) //Get all of the needed trap componenets
        {
            traps.Add(foundTraps[i]); //The trap
            trapScripts.Add(foundTraps[i].GetComponent<BaseTrap>()); //The script on the trap
            trapHalfHeight.Add(foundTraps[i].GetComponent<Collider>().bounds.size.y / 2); //Half of the Y value of the colider on the trap
        }

        buttonDown = false;
        inCooldown = false;
    }

    private IEnumerator CooldownTimer() //Trap activation/deactivation cooldown
    {
        inCooldown = true;

        yield return new WaitForSeconds(2);

        inCooldown = false;
    }
    #endregion

    #region HelperMethods
    /// <summary>
    /// Let the parrot interact with traps
    /// </summary>
    /// <param name="active">If the parrot is active or not</param>
    public void Interact(bool active)
    {
        if (active && !inCooldown)
            //Checking if the traps can be interacted with
            for (int i = 0; i < traps.Count; i++)
            {
                Vector3 direction = (traps[i].transform.position + new Vector3(0, trapHalfHeight[i], 0)) - transform.position;

                //Raycast to pick up the treasure
                Physics.Raycast(transform.position, direction, out hit);

                //TODO: update this with UI cues
                if (direction.magnitude < 10f)
                {
                    if (Input.GetButton(parrot.InputManager.PARROT_TRAP_AXIS))
                    {
                        //Activate or deactivate the trap
                        if (trapScripts[i].Activated)
                            trapScripts[i].Deactivate();
                        else if (!trapScripts[i].Activated)
                            trapScripts[i].Activate();

                        buttonDown = true; //Prevents immediate release of items
                        StartCoroutine(CooldownTimer()); //Go into cooldown to prevent immediate activation/deactivation
                    }
                }
            }
    }
    #endregion
}