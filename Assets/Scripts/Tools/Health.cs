using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public delegate void EventHandler(object sender, EventArgs args);
    public event EventHandler OnTakeDamage = delegate { };

    [SerializeField]
    [Range(1.0f, 10000.0f)]
    float maxHealth;
    float currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        OnTakeDamage(this, new EventArgs());
    }

    public float GetHealth()
    {
        return currentHealth;
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }

}
