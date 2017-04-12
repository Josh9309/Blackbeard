using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PendulumTrap : BaseTrap {
    //Attributes
    [SerializeField] private float stunTime = 2.0f;
    [SerializeField] private bool pause = false;
    
    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }


    public void Pause()
    {
        Debug.Log("Paused");
        if (pause)
        {
            trapAnim.SetBool("pause", true);
            StartCoroutine(PauseTime());
        }
        
    }

    public override void Activate()
    {
        activated = true;
        trapAnim.SetBool("trapActive", true); //sets trapActive to true
        trapAnim.Play("Activate");

        if (pause)
        {
            trapAnim.SetBool("pause", true);
            StartCoroutine(PauseTime());
        }
    }

    public override void Deactivate()
    {
        activated = false;
        trapAnim.SetBool("trapActive", false);
        trapAnim.Play("Deactivate");
    }

    public override void Trigger(GameObject pirate)
    {
        triggered = true;
        Vector3 knockbackForce = pirate.transform.forward * -500;
        pirate.gameObject.GetComponent<Rigidbody>().AddForce(knockbackForce);
        StartCoroutine(pirate.GetComponent<CaptainPirate>().Stun(stunTime));
    }

    public override void Reset()
    {
        activated = false;
        triggered = false;
        trapAnim.Play("Deactivate");
    }

    public IEnumerator PauseTime()
    {
        Debug.Log("pause");
        yield return new WaitForSeconds(resetTime);
        trapAnim.SetBool("pause", false);
    }
}
