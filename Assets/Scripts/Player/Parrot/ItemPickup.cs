using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    #region Attributes
    private List<GameObject> items;
    private List<Rigidbody> itemsRB;
    private List<Item> itemsScripts;
    private List<float> itemHalfHeight;
    private GameObject carriedItem;
    private Rigidbody carriedItemRB;
    float carriedItemHalfHeight;
    private Item carriedItemScript;
    private Transform itemSlot;
    private RaycastHit hit;
    private int visionAngle;
    private bool buttonDown;
    #endregion

    #region Properties
    public GameObject CarriedItem
    {
        get
        {
            return carriedItem;
        }
    }
    #endregion

    #region InBuiltMethods
    void Start() //Use this for initialization
    {
        items = new List<GameObject>();
        itemsRB = new List<Rigidbody>();
        itemsScripts = new List<Item>();
        itemHalfHeight = new List<float>();

        GameObject[] foundItems = GameObject.FindGameObjectsWithTag("Item");
        for (int i = 0; i < foundItems.Length; i++) //Get all of the needed item componenets
        {
            items.Add(foundItems[i]); //The item
            itemsRB.Add(foundItems[i].GetComponent<Rigidbody>()); //The item's rigidbody
            itemsScripts.Add(foundItems[i].GetComponent<Item>()); //The script on the item
            itemHalfHeight.Add(foundItems[i].GetComponent<Collider>().bounds.size.y / 2); //Half of the Y value of the colider on the item
        }

        carriedItem = null;
        carriedItemRB = null;
        carriedItemHalfHeight = 0;
        itemSlot = GameObject.FindGameObjectWithTag("ItemSlot").transform;
        visionAngle = 45;
        buttonDown = false;
    }
    #endregion

    #region HelperMethods
    /// <summary>
    /// Let the parrot pick up treasure
    /// </summary>
    /// <param name="active">If the parrot is active or not</param>
    public void Pickup(bool active)
    {
        if (active)
        {
            //Checking if the items can be picked up or put down
            if (carriedItem == null) //If no item is currently carried, search for an item for the player to pick up
            {
                for (int i = 0; i < items.Count; i++)
                {
                    //If an item will been destroyed outside of this script
                    if (items[i] == null)
                    {
                        items.Remove(items[i]);
                        itemsRB.Remove(itemsRB[i]);
                        itemHalfHeight.Remove(itemHalfHeight[i]);
                        itemsScripts.Remove(itemsScripts[i]);
                    }

                    Vector3 direction = (items[i].transform.position + new Vector3(0, itemHalfHeight[i], 0)) - transform.position;

                    //Raycast to pick up the treasure
                    Physics.Raycast(transform.position, direction, out hit);

                    //TODO: update this with UI cues
                    if (direction.magnitude < 1.5f && Vector3.Dot(direction, transform.forward) > -.1f)
                    {
                        if (Input.GetButton("Attack"))
                        {
                            carriedItem = items[i];
                            carriedItemRB = itemsRB[i];
                            carriedItemHalfHeight = itemHalfHeight[i];
                            carriedItemScript = itemsScripts[i];
                            buttonDown = true; //Prevents immediate release of items
                            break;
                        }
                    }
                }
            }
            else if (Input.GetButtonUp("Attack")) //Lets items be released
                buttonDown = false;

            //Picking up and putting down items
            if (Input.GetButton("Attack") && !buttonDown && carriedItem != null) //Putting down items
            {
                carriedItemScript.Active = true; //Activate the items
                carriedItem.transform.rotation = Quaternion.AngleAxis(0, Vector3.zero); //Rotate the item for the drop

                //Drop the item
                carriedItemRB.useGravity = true;
                carriedItem.transform.parent = null;

                //Remove the items from the list and set them to null
                items.Remove(carriedItem);
                itemsRB.Remove(carriedItemRB);
                itemHalfHeight.Remove(carriedItemHalfHeight);
                itemsScripts.Remove(carriedItemScript);

                //Set all current items to null
                carriedItem = null;
                carriedItemRB = null;
                carriedItemHalfHeight = 0;
                carriedItemScript = null;
            }
            else if (carriedItem != null) //When trying to pick something up, make sure nothing is currently held
            {
                Debug.Log(-carriedItemHalfHeight);
                carriedItemRB.useGravity = false;

                //Set the item for when it is picked up
                carriedItem.transform.parent = itemSlot;
                carriedItem.transform.localPosition = new Vector3(0, -carriedItemHalfHeight, 0); //Make sure the item is in the right position below the parrot
                carriedItem.transform.rotation = itemSlot.transform.rotation;
            }
        }
    }
    #endregion
}