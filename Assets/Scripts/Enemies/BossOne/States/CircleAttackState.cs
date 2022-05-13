using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleAttackState : BossState
{
    int numBullets;
    int numAttacks;
    float attackTime;

    float numAttacksCompleted;
    float elapsedAttackTime;

    float curAngle = 0.0f;

    float angleBetweenAttacks;
    float angleBetweenBullet;

    public CircleAttackState(int numBullets, int numberAttacks, float timeBetweenAttacks, Boss boss) : base("Circle Attack State", boss)
    {
        this.numBullets = numBullets;
        numAttacks = numberAttacks;
        attackTime = timeBetweenAttacks;

        angleBetweenBullet = 360.0f / numBullets;
        angleBetweenAttacks = 3.5f;
    }

    // Update is called once per frame
    public override void Update(float dt)
    {
        elapsedAttackTime += dt;
        if(elapsedAttackTime >= attackTime)
        {
            elapsedAttackTime = 0.0f;

            Attack(curAngle);

            numAttacksCompleted++;
            curAngle += angleBetweenAttacks;
        }

        if(numAttacksCompleted == numAttacks)
        {
            curCondition = 1;
        }
    }

    public override void OnEnter()
    {
        curCondition = 0;
        numAttacksCompleted = 0;
        elapsedAttackTime = 0.0f;

        curAngle = 0;
    }

    void Attack(float curAngle)
    {
        for (int i = 0; i <= numBullets; i++)
        {
            //x = r * sin(0), y = r * cos(0)
            const float radius = 1.0f;
            const float bulletSpeed = 15.0f;

            float x = radius * Mathf.Sin(curAngle);
            float y = radius * Mathf.Cos(curAngle);

            Vector3 dir = (boss.transform.position + new Vector3(x, y, 0.0f)) - boss.transform.position;
            dir.Normalize();

            GameObject newBullet = Object.Instantiate(parent.GetProjectile(), boss.transform.position, Quaternion.identity);
            newBullet.GetComponent<Rigidbody2D>().velocity = dir * bulletSpeed;

            curAngle += angleBetweenBullet;
        }
    }
}