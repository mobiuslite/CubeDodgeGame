using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedCircleState : BossState
{
    float timeAttacking;
    float timeBwtAttacks;

    float elapsedAttackingTime;
    float elapsedBetweenTime;

    Transform playerTransform;

    public TimedCircleState(float timeAttacking, float timeBetweenAttacks,  Boss boss) : base("Timed Circle State", boss)
    {
        this.timeAttacking = timeAttacking;
        this.timeBwtAttacks = timeBetweenAttacks;
        playerTransform = null;
    }

    public override void Update(float dt)
    {
        elapsedAttackingTime += dt;
        if (elapsedAttackingTime >= timeAttacking)
        {
            curCondition = 1;
        }

        Health bossHealth = boss.GetHealth();
        float usedTimeBwtAttack = (bossHealth.GetHealth() < bossHealth.GetMaxHealth() * 0.5f) ? timeBwtAttacks * 0.5f : timeBwtAttacks;

        elapsedBetweenTime += dt;
        if (elapsedBetweenTime >= usedTimeBwtAttack)
        {
            elapsedBetweenTime = 0.0f;
            Attack(playerTransform.position);
        }
    }

    public override void OnEnter()
    {
        curCondition = 0;
        elapsedAttackingTime = 0.0f;
        elapsedBetweenTime = 0.0f;

        if(playerTransform == null)
        {
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    void Attack(Vector3 pos)
    {
        if (parent.TryGetDamageObject("timed_attack", out GameObject timedAttackPrefab))
        {
            TimedCircleAttack circleAtk = Object.Instantiate(timedAttackPrefab, pos, Quaternion.identity).GetComponent<TimedCircleAttack>();
            circleAtk.StartAttack();
        }
    }
}
