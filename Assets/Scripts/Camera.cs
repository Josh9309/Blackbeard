using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour {
    #region Attributes
    public GameObject target;
    private float camXInput = 0;
    private float camYInput = 0;

    private float inputDelay = 0.3f;

    [SerializeField] private float rotSpeed = 1f;
    #endregion

    #region Properties
    #endregion

    // Use this for initialization
    void Start () {
        transform.position = target.transform.position;
    }
	
	// Update is called once per frame
	void Update () {
        transform.position = target.transform.position;

        camXInput = Input.GetAxis("Cam_Horizontal");
        camYInput = Input.GetAxis("Cam_Vertical");

        if (Mathf.Abs(camXInput) > inputDelay)
        {
            transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);
            if (camXInput > 0)
            {
                transform.Rotate(0, rotSpeed, 0);
            }
            else
            {
                transform.Rotate(0, -rotSpeed, 0);
            }
        }
        else if (Mathf.Abs(camYInput) > inputDelay)
        {
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, 0, 0);
            if (camYInput > 0)
            {
                transform.Rotate(rotSpeed, 0, 0);
            }
            else
            {
                transform.Rotate(-rotSpeed, 0, 0);
            }
        }
    }
}
