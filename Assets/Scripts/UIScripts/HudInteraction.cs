using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HudInteraction : MonoBehaviour
{


    // [SerializeField]Camera parrotCam;
   
    [SerializeField]
    GameObject trap;
    [SerializeField]
    Canvas HUD;
    // Use this for initialization
    void Start()
    {

        HUD.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
        if(Vector3.Distance(GameManager.Instance.ParrotP1.transform.position, trap.transform.position) < 50 || 
            (Vector3.Distance(GameManager.Instance.ParrotP2.transform.position, trap.transform.position) < 50))
        {
            HUD.enabled = true;
        }
       else
        {
            HUD.enabled = false;
        }
    }
}

