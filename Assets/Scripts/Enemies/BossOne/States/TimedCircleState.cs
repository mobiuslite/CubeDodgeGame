using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedCircleState : BossState
{
    float timeAttacking;
    float timeBwtAttacks;

    float elapsedAttackingTime;
    float elapsedBetweenTime;

    PlayerMovement player;

    public TimedCircleState(float timeAttacking, float timeBetweenAttacks,  Boss boss) : base("Timed Circle State", boss)
    {
        this.timeAttacking = timeAttacking;
        this.timeBwtAttacks = timeBetweenAttacks;
        player = null;
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

            const float futureTime = 0.33f;
            Vector3 futurePlayerPosition = (Vector2)player.transform.position + (player.GetVelocity() * futureTime);
            Attack(futurePlayerPosition);
        }
    }

    public override void OnEnter()
    {
        curCondition = 0;
        elapsedAttackingTime = 0.0f;
        elapsedBetweenTime = 0.0f;

        if(player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
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
