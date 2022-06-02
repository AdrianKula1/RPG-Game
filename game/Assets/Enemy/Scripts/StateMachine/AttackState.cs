using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State
{
    public FleeState fleeState;
    public IdleState idleState;
    public ChaseState chaseState;
    private bool attackCooldown;
    public override State RunCurrentState(Enemy enemy)
    {
        EnemyStatistics stats = enemy.GetEnemyStats();
        bool playerAlive = enemy.target.IsPlayerAlive();

        if (!playerAlive)
        {
            return idleState;
        }
        else if (enemy.lowHp())
        {
            return fleeState;
        }
        else
        {
            if (enemy.target != null)
            {
                float distance = GameManager.GetDistance(enemy);

                if (distance > stats.GetStat(EnemyStatistics.Stat.AttackRadious))
                {
                    return chaseState;
                }
            }

            if (!attackCooldown && playerAlive)
            {
                Attack(enemy, stats);
            }

            return this;
        }
    }

    private void Attack(Enemy enemy, EnemyStatistics stats)
    {
        if (!enemy.dmgCooldown)
        {
            Vector3 knockback = enemy.target.transform.position - enemy.transform.position;
            enemy.target.TakeDamage(stats.GetStat(EnemyStatistics.Stat.Damage), knockback, stats.GetStat(EnemyStatistics.Stat.KnockbackStrength),
                                    stats.GetStat(EnemyStatistics.Stat.KnockbackDuration));
            StartCoroutine(AttackCooldown(stats.GetStat(EnemyStatistics.Stat.AttackSpeed)));
            Debug.Log("Attacked Player");
        }
    }

    private IEnumerator AttackCooldown(float cooldown)
    {
        attackCooldown = true;
        yield return new WaitForSeconds(cooldown);
        attackCooldown = false;
    }
}

