using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public delegate void EventHandler(object sender, EventArgs args);
    public event EventHandler OnTakeDamage = delegate { };
    public event EventHandler OnDeath = delegate { };

    [SerializeField]
    bool takeHits;
    [SerializeField]
    int numHits;
    int currentHits;

    [SerializeField]
    [Range(1.0f, 10000.0f)]
    float maxHealth;
    float currentHealth;

    private void Awake()
    {
        currentHealth = maxHealth;
        currentHits = numHits;
    }

    public void TakeDamage(float amount)
    {
        if (takeHits)
        {
            currentHits -= 1;   
            if(currentHits <= 0 )
            {
                OnDeath(this, new EventArgs());
            }
        }
        else
        {
            currentHealth -= amount;
            if(currentHealth <= 0)
            {
                OnDeath(this, new EventArgs());
            }
        }

        OnTakeDamage(this, new EventArgs());
    }

    public float GetHealth()
    {      
        return takeHits ? currentHits : currentHealth;
    }

    public float GetMaxHealth()
    {
        return takeHits ? numHits : maxHealth;
    }

}
