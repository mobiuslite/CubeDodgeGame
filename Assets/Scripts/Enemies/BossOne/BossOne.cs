using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossOne : Boss
{
    SteeringBehaviour steeringBehaviour;
    bool awake = false;

    // Start is called before the first frame update
    void Start()
    {
        base.Init();

        steeringBehaviour = GetComponent<SteeringBehaviour>();
        steeringBehaviour.SetActive(false);

        //Set on damage code specific to this boss
        health.OnTakeDamage += (sender, args) =>
        {
            if (!awake)
            {
                steeringBehaviour.SetActive(true);
                stateMachine.Start();
                awake = true;
            }
        };

        //Boss States
        CircleAttackState circleState = new CircleAttackState(10, 3, 0.1f, this);
        IdleState idleState = new IdleState(2.0f, this);
        TimedCircleState timedState = new TimedCircleState(5.0f, .75f, this);

        circleState.AddTransition(1, idleState);

        idleState.AddTransition(1, circleState);
        idleState.AddTransition(2, timedState);

        timedState.AddTransition(1, circleState);

        stateMachine.AddState(idleState);
        stateMachine.AddState(circleState);
        stateMachine.AddState(timedState);
    }

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
        Debug.Log("Death of Boss One!");
        Destroy(gameObject);
    }
}
