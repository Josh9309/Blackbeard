using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public abstract class BaseTrap : MonoBehaviour {

    #region Attributes
    protected bool activated; //used to tell if the trap is activated
    protected bool triggered; //used to tell if the trap was triggered by the player
    [SerializeField] protected float resetTime; //how long till the trap can reset 

    protected Animator trapAnim;
    protected GameManager gm;
    protected ParticleSystem particle;
    #endregion

    #region Properties
    public bool Activated
    {
        get { return activated; }
    }

    public bool Triggered
    {
        get { return triggered; }
    }
    #endregion

    // Use this for initialization
    protected virtual void Start () {
        trapAnim = gameObject.GetComponent<Animator>();

        //make sure trap is deactivated
        Deactivate();

        triggered = false;

        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        particle = GetComponentInChildren<ParticleSystem>();
        particle.Pause();
    }
	
	// Update is called once per frame
	protected virtual void Update () {
        if (triggered)
        {
            //starts the reset timer
            StartCoroutine(ResetTimer());
        }
        if (!activated && gm.CurrentPlayer1State == GameManager.PlayerState.PARROT && gm.CurrentPlayer2State == GameManager.PlayerState.PARROT)
        {
            particle.startColor = new Color32(244, 215, 50, 255);
            particle.Play();
        }
        else if (activated && gm.CurrentPlayer1State == GameManager.PlayerState.PARROT && gm.CurrentPlayer2State == GameManager.PlayerState.PARROT)
        {
            particle.startColor = Color.red;
            particle.Play();
        }
        else
        {
            particle.Clear();
            particle.Pause();
        }
            
    }

    /// <summary>
    /// Used to check if a gameobject has entered the trigger radius
    /// </summary>
    /// <param name="col">The collider of the gameobject that entered</param>
    protected virtual void OnTriggerEnter(Collider col)
    {
        //make sure gameobject that entered was a collider
        if(col.tag == "Pirate" && activated)
        {
            //call the trigger Method for the trap
            Trigger(col.gameObject);
        }
        
    }

    /// <summary>
    /// Activate is used to Activate the trap and arm it. 
    /// </summary>
    public abstract void Activate();

    /// <summary>
    /// Deactivate is used to deactivate the trap and disarm it
    /// </summary>
    public abstract void Deactivate();

    /// <summary>
    /// Trigger is used to do something when the player triggers or hits the trap
    /// </summary>
    /// <param name="pirate"></param>
    public abstract void Trigger(GameObject pirate);

    /// <summary>
    /// Reset is used to reset the trap after it has been triggered.
    /// </summary>
    public abstract void Reset();

    /// <summary>
    /// Corroutine method that will wait the reset time and then it will reset the trap.
    /// </summary>
    /// <returns></returns>
    protected IEnumerator ResetTimer()
    {
        yield return new WaitForSeconds(resetTime);

        //calls the reset trap method
        Reset();
    }
}
