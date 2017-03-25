using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthHud : MonoBehaviour
{
    MeleeNPC health;
    //max health
   //public float totalHealth;
    float currentHealth;
    public GameObject healthBar;
	// Use this for initialization
	void Start ()
    {
        health = GetComponent<MeleeNPC>();
    }

    // Update is called once per frame
    void Update()
    {
        currentHealth = health.Health;
        // Debug.Log(currentHealth);
        // decreaseHealth();
        SetHealthbar(currentHealth / 150.0f);
    }

    void SetHealthbar(float healthbar)
    {
        //healthbar needs to be between 0-1
        healthBar.transform.localScale = new Vector3(healthbar, healthBar.transform.localScale.y, healthBar.transform.localScale.z);
       // Debug.Log(healthbar);
    }
}
