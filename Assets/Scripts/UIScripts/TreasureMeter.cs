using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
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

    [SerializeField] private Image redRunner;
    [SerializeField] private Image blueRunner;

    [SerializeField] private Image redTreasureIcon;
    [SerializeField] private Image blueTreasureIcon;

    private float redRunnerStart;
    private float blueRunnerStart;

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
        redRunnerStart = redRunner.rectTransform.localPosition.x;
        blueRunnerStart = blueRunner.rectTransform.localPosition.x;
    }
	
	// Update is called once per frame
	void Update ()
    {
        //get current distance between players and treasure
        currentdistRed = Vector3.Distance(treasure.transform.position, redPly.transform.position);
        currentDistBlue = Vector3.Distance(treasure.transform.position, bluePly.transform.position);

        float redBarDist = (redRunnerStart - redTreasureIcon.rectTransform.localPosition.x);
        float blueBarDist = (blueRunnerStart - blueTreasureIcon.rectTransform.localPosition.x);

        //check if we arent on the treasure itself fo red
        if (treasure.transform.position != redPly.transform.position)
        {
            //update scale of red players bar
            redBar.transform.localScale = new Vector3(currentdistRed / totalDistRed, redBar.transform.localScale.y, redBar.transform.localScale.z);

            //move lil runner
            float runnerPos = (currentdistRed / totalDistRed) * redBarDist; //players pos times hte bars length
            redRunner.rectTransform.localPosition = new Vector2(runnerPos - 55, redRunner.rectTransform.localPosition.y); //update x - offset for size of runner
        }

        //check if we arent on the treasure itself for blue
        if (treasure.transform.position != bluePly.transform.position)
        {
            //update scale of blue players bar
            blueBar.transform.localScale = new Vector3(currentDistBlue / totalDistBlue, blueBar.transform.localScale.y, blueBar.transform.localScale.z);

            //move lil runner
            float runnerPos = (currentDistBlue / totalDistBlue) * blueBarDist; //players pos times hte bars length
            blueRunner.rectTransform.localPosition = new Vector2(runnerPos + 55, blueRunner.rectTransform.localPosition.y); //update x - offset for size of runner
        }
    }
    
}
