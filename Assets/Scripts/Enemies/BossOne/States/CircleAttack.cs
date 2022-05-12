using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleAttack : BossState
{
    int numBullets;
    int numAttacks;
    float timeBwtAttacks;

    float elapsedStateTime;
    float elapsedAttackTime;

    public CircleAttack(int numBullets, int numberAttacks, float timeBetweenAttacks, float stateTime) : base("Circle Attack State")
    {
        this.numBullets = numBullets;
        numAttacks = numberAttacks;
        timeBwtAttacks = timeBetweenAttacks;

        float angleBetweenBullet = numBullets / 360.0f;

        float angleBetweenAttacks = angleBetweenBullet / numAttacks;
    }

    // Update is called once per frame
    public override void Update(float dt)
    {
        //Object.Instantiate(parent.GetProjectile());
    }

    public override void OnEnter()
    {
        curCondition = 0;
    }
}
