using UnityEngine;
using System.Collections;

public class DetectPlayer : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    /// <summary>
    /// detect if a player entrs the collider and assign it
    /// </summary>
    public void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.transform.tag == "Player")
        {
            Vector3 currDist = this.GetComponentInParent<AttackEnemy>().target.transform.position - this.GetComponentInParent<AttackEnemy>().transform.position;
            Vector3 newDist = coll.transform.position - this.GetComponentInParent<AttackEnemy>().transform.position;

            if (newDist.x < currDist.x && newDist.y < currDist.y)
            {
                this.GetComponentInParent<AttackEnemy>().target = coll.gameObject;
            }
        }
    }

    public void OnTriggerStay2D(Collider2D coll)
    {
        if (coll.transform.tag == "Player")
        {
            Vector3 currDist = this.GetComponentInParent<AttackEnemy>().target.transform.position - this.GetComponentInParent<AttackEnemy>().transform.position;
            Vector3 newDist = coll.transform.position - this.GetComponentInParent<AttackEnemy>().transform.position;

            if (newDist.x < currDist.x && newDist.y < currDist.y)
            {
                this.GetComponentInParent<AttackEnemy>().target = coll.gameObject;
            }
        }
    }
}
