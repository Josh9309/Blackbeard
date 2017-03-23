using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthHud : MonoBehaviour
{
    NPC health;
    //max health
    public float totalHealth;
    public float currentHealth;

    public GameObject healthBar;
	// Use this for initialization
	void Start ()
    {
        health = GetComponent<NPC>();
        totalHealth = health.Health;
        currentHealth = health.Health;

        InvokeRepeating("decreaseHealth", .1f, .1f);
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    void decreaseHealth()
    {
       // currentHealth -= 2f;
        float calcHealth = currentHealth / totalHealth;
        SetHealthbar(calcHealth);
    }

    void SetHealthbar(float healthbar)
    {
        //healthbar needs to be between 0-1
        healthBar.transform.localScale = new Vector3(healthbar, healthBar.transform.localScale.y, healthBar.transform.localScale.z);
    }
}
