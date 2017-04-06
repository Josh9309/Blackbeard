using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParrotCam : MonoBehaviour {
    [SerializeField] private GameObject target;
    [SerializeField] private float dist;
    [SerializeField] private float heightOffset;
    [SerializeField] private int playerNum;
    [SerializeField] private float xSpeed = 120.0f;
    [SerializeField] private float ySpeed = 120.0f;
    [SerializeField] private float rotationSpeed = 3.0f;
    [SerializeField] private float postitionSpeed = 2.0f;
    public float yMinLimit = -20f;
    public float yMaxLimit = 80f;

    public float distanceMin = .5f;
    public float distanceMax = 15f;

    private Vector3 targetPos;
    private float x = 0.0f;
    private float y = 0.0f;

    private PlayerInput pInput;
	// Use this for initialization
	void Start () {
        //get input manager
        switch (playerNum)
        {
            case 1:
                pInput = GameManager.Instance.P1Input;
                break;

            case 2:
                pInput = GameManager.Instance.P2Input;
                break;
        }

        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;
    }
	
	// Update is called once per frame
    void Update()
    {
        if(Input.GetAxis(pInput.HORIZONTAL_AXIS) > 0.5f)
        {
            x += 1;
        }
        if(Input.GetAxis(pInput.HORIZONTAL_AXIS) < -0.5f)
        {
            x -= 1;
        }
    }

	void LateUpdate () {
        if (target) //make sure there is a target
        {

            if (Mathf.Abs(Input.GetAxis(pInput.HORIZONTAL_AXIS)) > 0.5 || Mathf.Abs(Input.GetAxis(pInput.VERTICAL_AXIS)) > 0.5)
            {
                Recenter();
            }
            else
            {
                if (Input.GetButtonDown(pInput.R3_AXIS))
                {
                    Recenter();
                }
                x += Input.GetAxis(pInput.CAM_HORIZONTAL_AXIS) * xSpeed * dist * 0.02f;
                y -= Input.GetAxis(pInput.CAM_VERTICAL_AXIS) * ySpeed * 0.02f;

                y = ClampAngle(y, yMinLimit, yMaxLimit);

                Quaternion rotation = Quaternion.Euler(y, x, 0);

                dist = Mathf.Clamp((dist - 1f) * 5, distanceMin, distanceMax);

                RaycastHit hit;
                if (Physics.Linecast(target.transform.position, transform.position, out hit))
                {
                    dist -= hit.distance;
                }
                Vector3 negDistance = new Vector3(0.0f, 0.0f, -dist);
                Vector3 position = rotation * negDistance + target.transform.position;

                transform.rotation = rotation;
                transform.position = position;

                //transform.rotation = Quaternion.Lerp(transform.rotation, rotation,postitionSpeed *Time.deltaTime);
                //transform.position = Vector3.Lerp(transform.position, position, rotationSpeed*Time.deltaTime);
            }
        }
    }

    /// <summary>
    /// This method will recenter the camera behind the player
    /// </summary>
    public void Recenter()
    {

        y = ClampAngle(y, yMinLimit, yMaxLimit);

        Quaternion rotation = target.transform.rotation;

        dist = Mathf.Clamp((dist - 1f) * 5, distanceMin, distanceMax);

        RaycastHit hit;
        if (Physics.Linecast(target.transform.position, transform.position, out hit))
        {
            dist -= hit.distance;
        }
        Vector3 negDistance = new Vector3(0.0f, 0.0f, -dist);
        Vector3 position = rotation * negDistance + target.transform.position;

        transform.rotation = rotation;
        transform.position = position;

        x = rotation.eulerAngles.y;
        y = rotation.eulerAngles.z;
    }

    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360F)
            angle += 360F;
        if (angle > 360F)
            angle -= 360F;
        return Mathf.Clamp(angle, min, max);
    }
}
