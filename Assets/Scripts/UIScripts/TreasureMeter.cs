using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Joel @ Satch: learn to comment bud
public class TreasureMeter : MonoBehaviour
{
    [SerializeField]
    GameObject redBar;
    [SerializeField]
    GameObject blueBar;
    [SerializeField]
    GameObject treasure;
    [SerializeField]
    GameObject redPly;
    [SerializeField]
    GameObject bluePly;

    float currentdistRed;
    float totalDistRed;
    float currentDistBlue;
    float totalDistBlue;

    // Use this for initialization
    void Start ()
    {
        //get total distance between players and treasure
        totalDistRed = Vector3.Distance(treasure.transform.position, redPly.transform.position);
        totalDistBlue = Vector3.Distance(treasure.transform.position, bluePly.transform.position);
    }
	
	// Update is called once per frame
	void Update ()
    {
        //get current distance between players and treasure
        currentdistRed = Vector3.Distance(treasure.transform.position, redPly.transform.position);
        currentDistBlue = Vector3.Distance(treasure.transform.position, bluePly.transform.position);

        //check if we arent on the treasure itself fo red
        if (treasure.transform.position != redPly.transform.position)
        {
            //update scale of red players bar
               redBar.transform.localScale = new Vector3(currentdistRed / totalDistRed, redBar.transform.localScale.y, redBar.transform.localScale.z);
        }

        //check if we arent on the treasure itself for blue
        if (treasure.transform.position != bluePly.transform.position)
        {
            //update scale of blue players bar
            blueBar.transform.localScale = new Vector3(currentDistBlue / totalDistBlue, blueBar.transform.localScale.y, blueBar.transform.localScale.z);
        }
    }
    
}
