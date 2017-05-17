using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    #region Attributes
    private bool active; //If the item is active
    private bool previousActive;
    private CaptainPirate pirateScript;
    [SerializeField] private int damage;
    private int explosionDamage;
    [SerializeField]
    private GameObject firePrefab;
    private Rigidbody rBody;
    private Collider objectCollider;
    //private TrailRenderer tRend;
    #endregion

    #region Properties
    public bool Active
    {
        get
        {
            return active;
        }
        set
        {
            active = value;
        }
    }
    #endregion

    #region InBuiltMethods
    void Start() //Use this for initialization
    {
        //active = true;

        explosionDamage = -1000;

        rBody = GetComponent<Rigidbody>();
        previousActive = false;
        objectCollider = GetComponent<Collider>();

        //if (name.Contains("Coconut"))
        //{
        //    tRend = GetComponentInChildren<TrailRenderer>(); //Get the trail renderer
        //    tRend.enabled = false;
        //}
    }

    //private void Update()
    //{
        //if (active && tRend)
        //    tRend.enabled = true;
        //else if (!active && tRend)
        //    tRend.enabled = false;
    //}

    private void OnTriggerEnter(Collider coll)
    {
        if (gameObject.name.Contains("Bear_Trap"))
        {
            GetComponent<BearTrap>().enabled = true;
            
            enabled = false;
        }
    }

    private void OnCollisionEnter(Collision coll)
    {
        //Dropped items
        if (active && (coll.gameObject.tag == "Terrain" || coll.gameObject.tag == "IslandPlatform" || coll.gameObject.tag == "MovingPlatform" || coll.gameObject.tag == "RotatingPlatform" || coll.gameObject.tag == "CheckPointPlatform")) //Colliding with the ground
        {
            if (gameObject.name.Contains("Lantern") && active == true) // if gameObject is lantern
            {
                GameObject fire;
                // TODO: add stuff for spawning fire on the main island
                if (coll.gameObject.tag == "MovingPlatform" || coll.gameObject.tag == "RotatingPlatform")
                    fire = GameObject.Instantiate(firePrefab, transform.position, Quaternion.identity, coll.transform);
                else
                    fire = GameObject.Instantiate(firePrefab, transform.position, Quaternion.identity);
                fire.GetComponent<Fire>().Ignite();
                Destroy(gameObject);
            }
            else if (gameObject.name.Contains("Bear_Trap"))
            {
                GetComponent<BearTrap>().enabled = true;
                //GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
                Destroy(GetComponent<Rigidbody>()); //Destroy the rigidbody

                if (coll.gameObject.tag == "MovingPlatform")
                {
                    transform.parent = coll.gameObject.transform;
                    transform.rotation = Quaternion.Euler(0, transform.rotation.y, 0); //Stop bad rotation
                    transform.localScale = new Vector3(transform.localScale.x * transform.parent.lossyScale.x, transform.localScale.y * transform.parent.lossyScale.y, transform.localScale.z * transform.parent.lossyScale.z); //Rescale the trap
                }
                else if (coll.gameObject.tag == "RotatingPlatform") //Really hacky rescale
                {
                    transform.parent = coll.gameObject.transform;
                    transform.rotation = transform.parent.parent.rotation; //Alright, I'm real cheesed off about this one

                    //if (transform.parent.localScale.z > 1)
                    //    transform.localScale = new Vector3((transform.localScale.x * transform.parent.lossyScale.x) / 2, transform.localScale.y, (transform.localScale.z / transform.parent.lossyScale.z) * 2); //Rescale the trap
                    //else
                    //    transform.localScale = new Vector3((transform.localScale.x * transform.parent.lossyScale.x) / 2, transform.localScale.y, (transform.localScale.z * transform.parent.lossyScale.z) / 2); //Rescale the trap

                    if (transform.parent.localScale.z < 1)
                        transform.localScale = new Vector3(4.5f / transform.parent.lossyScale.x, 4.5f, 4.5f * transform.parent.lossyScale.z); //Rescale the trap
                    else
                        transform.localScale = new Vector3(4.5f / transform.parent.lossyScale.x, 4.5f, 4.5f / transform.parent.lossyScale.z); //Rescale the trap
                }

                enabled = false;
            }
            else //If this is any other object
            {
                Destroy(gameObject);
            }
        }
		else if (active && coll.gameObject.name.Contains("Captain") && !name.Contains("Bear_Trap")) //Colliding with a pirate
            Destroy(gameObject);
    }

    /// <summary>
    /// Makes sure items clipping the ground when they are dropped won't just sit there
    /// </summary>
    /// <param name="coll"></param>
    private void OnCollisionStay(Collision coll)
    {
        //Bear trap shouldn't need these
        if (active && !name.Contains("Bear_Trap"))
        {
            //CHEESE IT
            objectCollider.enabled = false;
            objectCollider.enabled = true;
        }
    }

    /// <summary>
    /// Heat seeking coconuts, will fail if time runs out
    /// </summary>
    /// <param name="coll"></param>
    private void OnTriggerStay(Collider coll)
    {
        if (name.Contains("Coconut") && coll.name.Contains("Captain") && active)
        {
            Vector3 desired = new Vector3(coll.transform.position.x, coll.transform.position.y + 1.5f, coll.transform.position.z) - transform.position;
            desired = desired.normalized * 3.25f;
            rBody.velocity = desired;

            if (previousActive == false)
            {
                StartCoroutine(CoconutDecay(3f)); //Remove this to buff coconuts
                previousActive = true;
            }
        }
    }


    //Make the coconut shrink
    internal IEnumerator CoconutDecay(float time)
    {
        yield return new WaitForSeconds(time);

        transform.position = new Vector3(0, -1000, 0); //Move the object off screen, don't destroy it yet because it's still needed for reference

        yield return new WaitForSeconds(3);

        previousActive = false;
        Destroy(gameObject);
    }
    #endregion
}