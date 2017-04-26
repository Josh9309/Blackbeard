using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : BaseTrap
{
    private float height; //The height of this object's collider
    private float originalYPosition; //The original Y position of this object
    private bool activatedAtLeastOnce; //If the trap has been activated at least once
    [SerializeField] private bool invert;

    protected override void Start() //Use this for initialization
    {
        base.Start();

        height = GetComponent<Collider>().bounds.size.y - .1f;
        originalYPosition = transform.position.y;
        activatedAtLeastOnce = false;
    }

    protected override void Update() //Update is called once per frame
    {
        base.Update();

        if (activated && invert)
        {
            transform.position = Vector3.Slerp(transform.position, new Vector3(transform.position.x, originalYPosition - height, transform.position.z), Time.deltaTime * 2);
        }
        else if (activated)
        {
            transform.position = Vector3.Slerp(transform.position, new Vector3(transform.position.x, originalYPosition + height, transform.position.z), Time.deltaTime * 2);
        }
        else if (!activated && activatedAtLeastOnce)
        {
            transform.position = Vector3.Slerp(transform.position, new Vector3(transform.position.x, originalYPosition, transform.position.z), Time.deltaTime * 2);
        }
    }

    public override void Activate()
    {
        activated = true;
        activatedAtLeastOnce = true;
        //trapAnim.Play("Activate");
    }

    public override void Deactivate()
    {
        activated = false;
        //trapAnim.Play("Deactivate");
    }

    public override void Reset()
    {
        activated = false;
        triggered = false;
        //trapAnim.Play("Deactivate");
    }

    public override void Trigger(GameObject pirate)
    {
        triggered = true;
        //trapAnim.Play("Activate");
    }
}