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
    //private bool buttonDown;
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

        //buttonDown = false;
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
        {
            List<GameObject> cutDownTraps = new List<GameObject>(); //Used to pick closest trap to activate

            //Checking if the traps can be interacted with
            for (int i = 0; i < traps.Count; i++)
            {
                Vector3 direction = (traps[i].transform.position + new Vector3(0, trapHalfHeight[i], 0)) - transform.position;

                //Raycast to pick up the treasure
                Physics.Raycast(transform.position, direction, out hit);

                if (direction.magnitude < 10f)
                {
                    Debug.Log(direction.magnitude);
                    if (Input.GetButton(parrot.InputManager.POSSESS_AXIS))
                    {
                        cutDownTraps.Add(traps[i]); //Add the trap to the list to cut down from

                        //buttonDown = true; //Prevents immediate release of items
                        StartCoroutine(CooldownTimer()); //Go into cooldown to prevent immediate activation/deactivation
                    }
                }
            }

            if (cutDownTraps.Count > 0) //If there are traps in the list
            {
                GameObject trapToUse = cutDownTraps[0];
                Vector3 firstTrapDirection = (cutDownTraps[0].transform.position + new Vector3(0, trapHalfHeight[0], 0)) - transform.position;

                if (cutDownTraps.Count > 1) //More than one trap in the list
                {
                    for (int i = 1; i < cutDownTraps.Count; i++)
                    {
                        Vector3 direction = (cutDownTraps[i].transform.position + new Vector3(0, trapHalfHeight[i], 0)) - transform.position;

                        //Take the closest trap
                        if (direction.magnitude <= firstTrapDirection.magnitude)
                            trapToUse = cutDownTraps[i];
                    }
                }

                //Activate or deactivate the trap
                if (trapScripts[trapScripts.IndexOf(trapToUse.GetComponent<BaseTrap>())].Activated)
                    trapScripts[trapScripts.IndexOf(trapToUse.GetComponent<BaseTrap>())].Deactivate();
                else if (!trapScripts[trapScripts.IndexOf(trapToUse.GetComponent<BaseTrap>())].Activated)
                    trapScripts[trapScripts.IndexOf(trapToUse.GetComponent<BaseTrap>())].Activate();
            }
        }
    }
    #endregion
}