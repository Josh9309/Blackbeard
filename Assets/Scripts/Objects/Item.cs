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

        if (gameObject.name.Contains("Bear_Trap"))
        {
            GetComponentInChildren<ParticleSystem>().Clear();
            GetComponentInChildren<ParticleSystem>().Pause();
        }

        rBody = GetComponent<Rigidbody>();
        previousActive = false;
        objectCollider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider coll)
    {
        if (gameObject.name.Contains("Bear_Trap"))
        {
            Debug.Log(gameObject.name);
            GetComponent<BearTrap>().enabled = true;
            
            this.enabled = false;
        }
    }

    private void OnCollisionEnter(Collision coll)
    {
        //Dropped items
        if (active && (coll.gameObject.tag == "Terrain" || coll.gameObject.tag == "IslandPlatform" || coll.gameObject.tag == "MovingPlatform" || coll.gameObject.tag == "RotatingPlatform")) //Colliding with the ground
        {
            if (gameObject.name.Contains("Lantern") && active == true) // if gameObject is lantern
            {
                // TODO: add stuff for spawning fire on the main island
                GameObject fire = GameObject.Instantiate(firePrefab, transform.position, Quaternion.identity);
                fire.GetComponent<Fire>().Ignite();
                Destroy(gameObject);
            }
            else if (gameObject.name.Contains("Bear_Trap"))
            {
                GetComponent<BearTrap>().enabled = true;
                enabled = false;
            }
            else //If this is any other object
            {
                Destroy(gameObject);
            }
        }
        else if (active && coll.gameObject.name.Contains("Captain")) //Colliding with a pirate
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
            desired = desired.normalized * 3;
            rBody.velocity = desired;

            if (previousActive == false)
            {
                //StartCoroutine(CoconutDecay(1f)); //Remove this to buff coconuts
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