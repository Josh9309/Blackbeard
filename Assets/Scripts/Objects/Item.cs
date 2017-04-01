using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    #region Attributes
    private bool active; //If the item is active
    private CaptainPirate pirateScript;
    [SerializeField] private int damage;
    private int explosionDamage;
    [SerializeField]
    private GameObject firePrefab;
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
        active = false;
        explosionDamage = -1000;
	}

    private void OnCollisionEnter(Collision coll)
    {
        //Dropped items
        if (active && coll.gameObject.tag == "Terrain") //Colliding with the ground
        {
            if (gameObject.name.Contains("Bomb")) //If this is a bomb
            {
                Collider[] hitColliders = Physics.OverlapSphere(transform.position, 4);

                //For every object in the explosion radius apply damage
                foreach (Collider c in hitColliders)
                {
                    if (c.gameObject.tag == "Pirate")
                    {
                        pirateScript = c.gameObject.GetComponent<CaptainPirate>();
                    }
                }

                StartCoroutine(ExplosionTimer(.017f));
            }
            else if (gameObject.name.Contains("Lantern")) // if gameObject is lantern
            {
                // TODO: add stuff for spawning fire on the main island
                Destroy(gameObject);
            }
            else //If this is any other object
            {
                Destroy(gameObject);
            }
        }
        else if (active && coll.gameObject.name.Contains("Pirate")) //Colliding with a pirate
        {
            if (gameObject.name.Contains("Bomb")) //If this is a bomb
            {
                Collider[] hitColliders = Physics.OverlapSphere(transform.position, 4);

                //For every object in the explosion radius apply damage
                foreach (Collider c in hitColliders)
                {
                    if (c.gameObject.tag == "Pirate")
                    {
                        pirateScript = c.gameObject.GetComponent<CaptainPirate>();
                    }
                }

                StartCoroutine(ExplosionTimer(.017f));
            }
            else //If this is any other object
            {
                pirateScript = coll.gameObject.GetComponent<CaptainPirate>();

                Destroy(gameObject);
            }
        }
        else if (active && (coll.gameObject.tag == "IslandPlatform" || coll.gameObject.tag == "MovingPlatform" || coll.gameObject.tag == "RotatingPlatform"))
        {
            if (gameObject.name.Contains("Lantern"))
            {
                GameObject fire = GameObject.Instantiate(firePrefab, this.transform.position, Quaternion.identity);
                fire.GetComponent<Fire>().Ignite(coll.gameObject, transform.position.y);
                Destroy(gameObject);
            }
        }
    }

    //Make the bomb explode
    internal IEnumerator ExplosionTimer(float time)
    {
        yield return new WaitForSeconds(time);

        Destroy(gameObject);
    }
    #endregion
}