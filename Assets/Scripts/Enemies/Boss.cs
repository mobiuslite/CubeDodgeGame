using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
public abstract class Boss : MonoBehaviour
{
    protected Health health;

    [SerializeField]
    LayerMask takeDamageFromMask;

    protected BossStateMachine stateMachine;
    protected AudioDictionary audioDictionary;


    //Projectiles/Damage Objects
    [SerializeField]
    List<DamageObjectKey> damageObjectKeys;

    [Serializable]
    private struct DamageObjectKey
    {
        public GameObject obj;
        public string key;
    }


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
        //Add damage objects to state machine
        foreach (DamageObjectKey pair in damageObjectKeys)
        {
            stateMachine.AddDamageObject(pair.key, pair.obj);
        }


        audioDictionary = GetComponent<AudioDictionary>();
        health = GetComponent<Health>();

        //Update UI
        UIManager.Instance.SetBossMaxHealthUI(health.GetHealth());
        health.OnTakeDamage += (sender, args) =>
        {
            float curHealth = health.GetHealth();
            UIManager.Instance.SetBossHealthUI(curHealth);
        };

        health.OnDeath += (sender, args) =>
        {
            OnDeath();
        };
    }

    public Health GetHealth()
    {
        return health;
    }

    protected abstract void OnDeath();

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (LayerTools.IsInLayerMask(collision.gameObject, takeDamageFromMask))
        {
            health.TakeDamage(10.0f);
        }
    }

}
