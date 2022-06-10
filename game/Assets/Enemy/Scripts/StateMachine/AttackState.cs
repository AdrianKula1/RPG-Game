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
        if (!attackCooldown)
        {
            Vector3 knockback = enemy.target.transform.position - enemy.transform.position;
            enemy.target.TakeDamage(stats.GetStat(EnemyStatistics.Stat.Damage), knockback, stats.GetStat(EnemyStatistics.Stat.KnockbackStrength),
                                    stats.GetStat(EnemyStatistics.Stat.KnockbackDuration));
            AttackEffect(enemy);
            StartCoroutine(AttackCooldown(stats.GetStat(EnemyStatistics.Stat.AttackSpeed)));
            Debug.Log("Attacked Player");
        }
    }

    private void AttackEffect(Enemy enemy)
    {
        if (enemy.CompareTag("MudSlime"))
        {
            int random = Random.Range(1, 8);
            if (random <= 3)
            {
                enemy.target.GetStats().ApplyEffect(new Slowdown());
                StartCoroutine(EffectDuration(10f, enemy.target));
            }
        }
    }

    private IEnumerator EffectDuration(float time, Player target)
    {
        yield return new WaitForSeconds(time);
        target.GetStats().ApplyEffect(new Standard());

    }

    private IEnumerator AttackCooldown(float cooldown)
    {
        attackCooldown = true;
        yield return new WaitForSeconds(cooldown);
        attackCooldown = false;
    }
}

