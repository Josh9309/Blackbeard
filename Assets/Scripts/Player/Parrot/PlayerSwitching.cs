using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwitching : MonoBehaviour
{
    #region Attributes
    private BasePirate basePirateScript; //The pirate
    private Buccaneer buccScript; //Buccaneer script
    private TreasureHunter treasureHScript; //Treasure hunter script
    private NPC npcScript;
    private GameCamera cam; //The camera
    private GameManager gm; //The game manager
    private GameObject targetPirate; //The currently targeted pirate
    private bool canChangeCharacter; //If the parrot can land or take off again
    #endregion

    void Start() //Use this for initialization
    {
        cam = FindObjectOfType<GameCamera>();
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        canChangeCharacter = true; //The parrot can be a parrotsite
    }

    /// <summary>
    /// Coroutine to prevent immediate landing or takeoff from pirate
    /// </summary>
    /// <param name="time">The amount of time to wait</param>
    /// <returns>Returns when finished waiting</returns>
    internal IEnumerator ChangeTimer(int time)
    {
        canChangeCharacter = false;

        yield return new WaitForSeconds(time);

        canChangeCharacter = true;
    }

    /// <summary>
    /// Makes sure the parrot stays with the pirate it is controlling
    /// </summary>
    public void StayWithPirate(Rigidbody rBody)
    {
        //Make sure the parrot stays with the pirate it is controlling
        rBody.velocity = Vector3.zero;
        transform.position = new Vector3(basePirateScript.transform.position.x, transform.position.y, basePirateScript.transform.position.z);
        transform.rotation = basePirateScript.transform.rotation;

        //TODO: make parrot invisible. Best done when rough assets arrive
    }

    /// <summary>
    /// This method is used to allow the parrot to land
    /// </summary>
    /// <param name="active">If the parrot is active or not</param>
    /// <param name="carriedItemState">If the parrot is carrying an item or not</param>
    /// <returns>The active state of the parrot, modified or not</returns>
    public bool Land(bool active, GameObject carriedItemState)
    {
        if (active)
        {
            //Get a close player
            foreach (GameObject bPlayer in gm.BlueSquads)
            {
                if (targetPirate != null)
                    break;

                foreach (GameObject bNPC in bPlayer.GetComponent<SquadManager>().Pirates)
                {
                    if ((bNPC.GetComponent<NPC>().Top.position - transform.position).magnitude <= 3)
                    {
                        Debug.Log("B " + bNPC.name);
                        targetPirate = bNPC;
                        break;
                    }
                }
            }
            foreach (GameObject rPlayer in gm.RedSquads)
            {
                if (targetPirate != null)
                    break;

                foreach (GameObject rNPC in rPlayer.GetComponent<SquadManager>().Pirates)
                {
                    if ((rNPC.GetComponent<NPC>().Top.position - transform.position).magnitude <= 3)
                    {
                        Debug.Log("R " + rNPC.name);
                        targetPirate = rNPC;
                        break;
                    }
                }
            }

            //Landing on pirate
            if (targetPirate != null && Input.GetButton("Interact") && canChangeCharacter && carriedItemState == null)
            {
                //Set the target of the camera
                cam.Target = targetPirate.gameObject.transform.GetChild(0).transform; //This assumes that the child gameobject is always the first one under the parent

                //Get scripts from the pirate
                basePirateScript = targetPirate.GetComponent<BasePirate>();
                npcScript = targetPirate.GetComponent<NPC>();
                npcScript.Active = false;

                if (basePirateScript is Buccaneer)
                {
                    buccScript = targetPirate.GetComponent<Buccaneer>();
                    gm.CurrentPlayerState = GameManager.PlayerState.BUCCANEER;
                }
                else if (basePirateScript is TreasureHunter)
                {
                    treasureHScript = targetPirate.GetComponent<TreasureHunter>();
                    gm.CurrentPlayerState = GameManager.PlayerState.HUNTER;
                }

                gm.Player = targetPirate.gameObject;

                //Enable the pirate
                basePirateScript.PirateActive = true;
                //Disable the parrot
                active = false;

                StartCoroutine(ChangeTimer(2));
            }

            targetPirate = null;
        }

        return active;
    }

    /// <summary>
    /// This method is used to allow the parrot to take off again
    /// </summary>
    /// <param name="active">If the parrot is active or not</param>
    /// <returns>The active state of the parrot, modified or not</returns>
    public bool Takeoff(bool active)
    {
        if (!active)
        {
            bool doingRelevantAction = false;

            //If either pirate type is perfoming an action
            if (buccScript != null && buccScript.AttState != Buccaneer.AttackState.Idle)
                doingRelevantAction = true;
            else if (treasureHScript != null && treasureHScript.PickingUp)
                doingRelevantAction = true;

            //Taking off from pirate
            //Timer for switching must be completed
            //Pirate must not be jumping, picking up treasure, or attacking
            if (basePirateScript.Health <= 0 || (Input.GetButton("Interact") && canChangeCharacter && basePirateScript.Grounded && !doingRelevantAction))
            {
                //Set the target of the camera
                cam.Target = gameObject.transform.GetChild(0).transform; //This assumes that the child gameobject is always the first one under the parent;
                //Activate the parrot
                active = true;

                gm.Player = this.gameObject;
                gm.CurrentPlayerState = GameManager.PlayerState.PARROT;

                //TODO: update this with the AI
                if (basePirateScript != null)
                {
                    basePirateScript.RBody.velocity = Vector3.zero;
                    basePirateScript.transform.localEulerAngles = Vector3.zero;
                }

                //Disable the pirate scripts
                basePirateScript.PirateActive = false;
                buccScript = null;
                treasureHScript = null;
                basePirateScript = null;

                npcScript.Active = true;
                npcScript = null;

                StartCoroutine(ChangeTimer(2));
            }
        }

        return active;
    }
}