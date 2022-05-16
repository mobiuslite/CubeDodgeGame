using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField]
    [Range(1.0f, 10000.0f)]
    protected float maxHealth;
    protected float currentHealth;

    [SerializeField]
    LayerMask damageMask;

    protected BossStateMachine stateMachine;
    protected AudioDictionary audioDictionary;

    public Boss()
    {
        stateMachine = new BossStateMachine();
    }

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

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if(LayerTools.IsInLayerMask(collision.gameObject, damageMask))
        {
            currentHealth -= 10.0f;
            UIManager.Instance.SetBossHealth(currentHealth);

            if(currentHealth <= 0.0f)
            {
                currentHealth = 0.0f;
                OnDeath();
            }
        }
    }

    protected virtual void OnDeath()
    {

    }
}
