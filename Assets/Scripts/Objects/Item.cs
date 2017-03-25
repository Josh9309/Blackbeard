using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    #region Attributes
    private bool active; //If the item is active
    private BasePirate basePirateScript;
    [SerializeField] private int damage;
    private int explosionDamage;
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
                        basePirateScript = c.gameObject.GetComponent<BasePirate>();
                    }
                }

                StartCoroutine(ExplosionTimer(.017f));
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
                        basePirateScript = c.gameObject.GetComponent<BasePirate>();
                    }
                }

                StartCoroutine(ExplosionTimer(.017f));
            }
            else //If this is any other object
            {
                basePirateScript = coll.gameObject.GetComponent<BasePirate>();

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