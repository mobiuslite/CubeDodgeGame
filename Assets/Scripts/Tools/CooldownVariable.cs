using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
A custom variable type that handles cooldowns and ability activation.
Has events on ability enter and exit. Make sure to be calling update on this variable every frame
*/
public class CooldownAbility
{
    public delegate void EventHandler(object sender, EventArgs args);
    public event EventHandler OnAbilityEnter = delegate { };
    public event EventHandler OnAbilityExit = delegate { };

    [SerializeField]
    float cooldownLength;
    [SerializeField]
    float abilityLength;

    float elapsedCooldown;
    float elapsedAbility;

    bool cooldownActive;
    bool abilityActive;

    public CooldownAbility(float cooldownLength, float abilityLength)
    {
        cooldownActive = false;
        abilityActive = false;

        elapsedAbility = 0.0f;
        elapsedCooldown = 0.0f;

        this.cooldownLength = cooldownLength;
        this.abilityLength = abilityLength;
    }

    public void Use()
    {
        abilityActive = true;

        OnAbilityEnter(this, new EventArgs());
    }

    public void CancelAbility()
    {
        if (abilityActive)
        {
            abilityActive = false;
            cooldownActive = true;
            elapsedAbility = 0.0f;

            OnAbilityExit(this, new EventArgs());
        }
    }

    public bool AbilityIsActive()
    {
        return abilityActive;
    }

    public bool CooldownIsActive()
    {
        return cooldownActive;
    }

    public bool CanUseAbility()
    {
        return !cooldownActive && !abilityActive;
    }

    public float AbilityValue()
    {
        return elapsedAbility / abilityLength;
    }

    public float CooldownValue()
    {
        return elapsedCooldown / cooldownLength;
    }

    // Update is called once per frame
    public void Update(float dt)
    {
        if (abilityActive)
        {
            elapsedAbility += dt;

            //Ability is finished, enable cooldown
            if(elapsedAbility >= abilityLength)
            {
                abilityActive = false;
                cooldownActive = true;
                elapsedAbility = 0.0f;

                OnAbilityExit(this, new EventArgs());
            }
        }

        if (cooldownActive)
        {
            elapsedCooldown += dt;

            //Cooldown is finished
            if(elapsedCooldown >= cooldownLength)
            {
                cooldownActive = false;
                elapsedCooldown = 0.0f;
            }
        }
    }
}
