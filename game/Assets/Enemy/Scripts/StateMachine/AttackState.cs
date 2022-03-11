using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State
{
    public FleeState fleeState;
    public IdleState idleState;
    public bool lowHp;
    private bool attackCooldown;
    public override State RunCurrentState(EnemyManager enemyManager)
    {
        bool playerAlive = enemyManager.target.isPlayerAlive();

        if (!attackCooldown && playerAlive)
        {
            enemyManager.target.TakeDamage(10f);
            StartCoroutine(AttackCooldown());
            Debug.Log("Attacked Player");
        }

        if (!playerAlive)
        {
            return idleState;
        }
        else if (lowHp)
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

