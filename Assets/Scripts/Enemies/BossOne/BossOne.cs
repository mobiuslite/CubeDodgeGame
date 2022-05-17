using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossOne : Boss
{
    [SerializeField]
    GameObject projectilePrefab;

    SteeringBehaviour steeringBehaviour;

    bool awake = false;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;

        audioDictionary = GetComponent<AudioDictionary>();

        steeringBehaviour = GetComponent<SteeringBehaviour>();
        steeringBehaviour.SetActive(false);

        //Update UI
        UIManager.Instance.SetBossMaxHealth(maxHealth);

        //Boss States
        CircleAttackState circleState = new CircleAttackState(10, 3, 0.1f, this);
        IdleState idleState = new IdleState(4.0f, this);

        circleState.AddTransition(1, idleState);
        idleState.AddTransition(1, circleState);

        stateMachine.AddState(idleState);
        stateMachine.AddState(circleState);  

        stateMachine.SetProjectile(projectilePrefab);       
    }

    // Update is called once per frame
    void Update()
    {
        stateMachine.Update(Time.deltaTime);
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);     
    }

    protected override void OnDeath()
    {
        Debug.Log("Death!");
        Destroy(gameObject);
    }

    protected override void TakeDamage(float amount)
    {
        base.TakeDamage(amount);

        if (!awake)
        {
            steeringBehaviour.SetActive(true);
            stateMachine.Start();
            awake = true;
        }
    }
}
