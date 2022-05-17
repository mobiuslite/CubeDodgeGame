using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
public abstract class Boss : MonoBehaviour
{
    protected Health health;

    [SerializeField]
    LayerMask damageMask;

    protected BossStateMachine stateMachine;
    protected AudioDictionary audioDictionary;

    public Boss()
    {
        stateMachine = new BossStateMachine();
    }

    /// <summary>
    /// Returns the name of the current state the boss is in
    /// </summary>
    /// <returns>string: current state name</returns>
    public string GetCurrentState()
    {
        if(stateMachine == null || stateMachine.GetState() == null)
        {
            return "None";
        }

        return stateMachine.GetState().stateName;
    }

    public AudioDictionary GetAudioDictionary()
    {
        return audioDictionary;
    }

    /// <summary>
    /// Call this method on children "Start"
    /// </summary>
    protected void Init()
    {
        audioDictionary = GetComponent<AudioDictionary>();
        health = GetComponent<Health>();

        //Update UI
        UIManager.Instance.SetBossMaxHealth(health.GetHealth());
        health.OnTakeDamage += (sender, args) =>
        {
            float curHealth = health.GetHealth();
            UIManager.Instance.SetBossHealth(curHealth);

            if (curHealth <= 0.0f)
            {
                OnDeath();
            }
        };
    }

    protected abstract void OnDeath();

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (LayerTools.IsInLayerMask(collision.gameObject, damageMask))
        {
            health.TakeDamage(10.0f);
        }
    }

}
