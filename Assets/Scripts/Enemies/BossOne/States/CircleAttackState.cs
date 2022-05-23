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

    float angleBetweenBullet;

    const float angleBetweenAttacks = 3.5f;

    public CircleAttackState(int numBullets, int numberAttacks, float timeBetweenAttacks, Boss boss) : base("Circle Attack State", boss)
    {
        this.numBullets = numBullets;
        numAttacks = numberAttacks;
        attackTime = timeBetweenAttacks;

        angleBetweenBullet = 360.0f / numBullets;
    }

    // Update is called once per frame
    public override void Update(float dt)
    {

        //Increase number of attacks at low health
        int usedNumAttacks = boss.GetHealth().GetHealth() < boss.GetHealth().GetMaxHealth() * 0.5f ? numAttacks * 4 : numAttacks;
        bool atLowHealth = usedNumAttacks != numAttacks;

        //Attack the number of times given, and wait the amount of time given between attacks
        elapsedAttackTime += dt;
        if(elapsedAttackTime >= attackTime)
        {
            elapsedAttackTime = 0.0f;

            Attack(curAngle);

            numAttacksCompleted++;

            curAngle += angleBetweenAttacks;
        }

        
        if(numAttacksCompleted == usedNumAttacks)
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

    //TODO: Turn into object pooling for increased performance 
    void Attack(float curAngle)
    {
        for (int i = 0; i <= numBullets; i++)
        {
            //x = r * sin(0), y = r * cos(0)
            //Find a point on a circle using the angle given, and get the direction to that point.
            //Use that direction as the bu
            const float radius = 1.0f;
            const float bulletSpeed = 15.0f;

            float x = radius * Mathf.Sin(curAngle);
            float y = radius * Mathf.Cos(curAngle);

            Vector3 dir = (boss.transform.position + new Vector3(x, y, 0.0f)) - boss.transform.position;
            dir.Normalize();

            if (parent.TryGetDamageObject("projectile", out GameObject bulletPrefab)) {

                GameObject newBullet = Object.Instantiate(bulletPrefab, boss.transform.position, Quaternion.identity);
                newBullet.GetComponent<Rigidbody2D>().velocity = dir * bulletSpeed;

                curAngle += angleBetweenBullet;
            }
        }

        boss.GetAudioDictionary().PlaySound("projectile_fire");
    }
}
