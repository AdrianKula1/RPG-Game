using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State
{
    public FleeState fleeState;
    public IdleState idleState;
    public ChaseState chaseState;
    private bool attackCooldown;
    public override State RunCurrentState(EnemyManager enemyManager)
    {
        bool playerAlive = enemyManager.target.IsPlayerAlive();

        if (enemyManager.target != null)
        {
            Vector2 currentPosition = enemyManager.transform.position;
            Vector2 targetPosition = enemyManager.target.transform.position;
            float distance = Vector2.Distance(currentPosition, targetPosition);

            if (distance > enemyManager.attackRadious)
            {
                return chaseState;
            }
        }

        if (!attackCooldown && playerAlive)
        {
            enemyManager.target.TakeDamage(enemyManager.enemyStats["Damage"].getValue());
            StartCoroutine(AttackCooldown());
            Debug.Log("Attacked Player");
        }

        if (!playerAlive)
        {
            return idleState;
        }
        else if (enemyManager.enemyStats["Health"].getValue() < 10f)
        {
            return fleeState;
        }
        else
        {
            return this;
        }
    }

    private IEnumerator AttackCooldown()
    {
        attackCooldown = true;
        yield return new WaitForSeconds(1f);
        attackCooldown = false;
    }
}

