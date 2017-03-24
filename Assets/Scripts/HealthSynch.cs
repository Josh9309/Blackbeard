using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSynch : MonoBehaviour {
    #region Attributes

    int currentHealth;
    int maxHealth;

    //pirate variables
    NPC aiPirate;
    BasePirate playerPirate;
    #endregion

    #region Properties
    public int MaxHealth
    {
        get { return maxHealth; }
    }

    #endregion

    // Use this for initialization
    void Start () {
        //get the two pirate scripts
        aiPirate = GetComponent<NPC>();
        playerPirate = GetComponent<BasePirate>();

        //determine which script is active
        if (aiPirate.Active)
        {
            maxHealth = aiPirate.Health; //sets max health equal to ai pirate's initial health
            currentHealth = aiPirate.Health; //current health is equal to AI Pirate's health
            playerPirate.Health = currentHealth; //make sure playerPirate is in synch with AI Pirates Health
        }
        else
        {
            maxHealth = playerPirate.Health; //sets max health equal to player pirate's initial health
            currentHealth = playerPirate.Health; //current health is equal to player Pirate's health
            aiPirate.Health = currentHealth; //make sure AI Pirate's Health is in synch with player Pirate's Health
        }

        maxHealth = currentHealth; //set maxHealth to be the intial health at the start

    }
	
	// Update is called once per frame
	void Update () {
        if (aiPirate.Active) //if ai script is active
        {
            currentHealth = aiPirate.Health; //update current health to be ai pirate's health
            playerPirate.Health = currentHealth; //set player pirates health to be current health
        }
        else //if player script is active
        {
            currentHealth = playerPirate.Health; //updates current health to be player pirate's Health
            aiPirate.Health = currentHealth; //set ai pirate's health to be current health
        }

        //check the status of the pirates health
        CheckHealth();
	}

    /// <summary>
    /// This method allows for the pirate script that is not currently active to be changed and it will update the other pirate script accordingly.
    /// </summary>
    /// <param name="updateAI">tells method whether to update ai pirate or player pirate</param>
    public void UpdateHealth(bool updateAI)
    {
        if (updateAI)
        {
            currentHealth = playerPirate.Health;
            aiPirate.Health = currentHealth;
        }
        else
        {
            currentHealth = aiPirate.Health;
            playerPirate.Health = currentHealth;
        }
    }

    /// <summary>
    /// CheckHealth will check to see what the status is of the health. If health is below zero It will call the death Method and it will not let health go above max health.
    /// </summary>
    protected void CheckHealth()
    {
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth; //resets the health to max health if health is over max health.
            aiPirate.Health = playerPirate.Health = currentHealth; //sets the pirates health to be the current health
        }
        else if (currentHealth < maxHealth && currentHealth > 0) //For visual damage feedback
        {
            //TODO: visual feedback
        }
        else if (currentHealth <= 0)
        {
            currentHealth = 0;
            aiPirate.Health = playerPirate.Health = currentHealth; //sets the pirates health to be the current health
            //Dead(); //calls pirates dead method if health is 0 or bellow
        }

        //make sure pirates health are in synch
        if(aiPirate.Health != currentHealth || currentHealth != playerPirate.Health)
        {
            aiPirate.Health = currentHealth;
            playerPirate.Health = currentHealth;
        }
    }
}
