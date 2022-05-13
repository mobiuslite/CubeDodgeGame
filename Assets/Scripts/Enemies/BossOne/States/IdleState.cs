using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : BossState
{
    float idleTime;
    float elapsedIdleTime;

    public IdleState(float idleTime, Boss boss) : base("Idle State", boss)
    {
        this.idleTime = idleTime;
    }

    // Update is called once per frame
    public override void Update(float dt)
    {
        elapsedIdleTime += dt;
        if(elapsedIdleTime >= idleTime)
        {
            curCondition = 1;
        }
    }

    public override void OnEnter()
    {
        curCondition = 0;
        elapsedIdleTime = 0.0f;
    }
}
