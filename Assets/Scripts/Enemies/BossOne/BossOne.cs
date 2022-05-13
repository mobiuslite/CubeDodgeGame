using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossOne : Boss
{
    [SerializeField]
    GameObject projectilePrefab;

    // Start is called before the first frame update
    void Start()
    {
        CircleAttackState circleState = new CircleAttackState(10, 3, 0.1f, this);
        IdleState idleState = new IdleState(4.0f, this);

        circleState.AddTransition(1, idleState);
        idleState.AddTransition(1, circleState);

        stateMachine.AddState(idleState);
        stateMachine.AddState(circleState);  

        stateMachine.SetProjectile(projectilePrefab);

        stateMachine.Start();
    }

    // Update is called once per frame
    void Update()
    {
        stateMachine.Update(Time.deltaTime);
    }
}
