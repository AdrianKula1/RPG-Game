using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
    [SerializeField] public LayerMask layermask;
    public ChaseState chaseState;
    public FleeState fleeState;
    public bool canSeePlayer;
    public override State RunCurrentState(Enemy enemy)
    {

        canSeePlayer = CanSeePlayer(enemy);

        if (canSeePlayer)
        {
            if (enemy.lowHp())
            {
                return fleeState;
            }
            else
            {
                return chaseState;
            }
        }
        else
        {
            return this;
        }
    }

    private bool CanSeePlayer(Enemy enemy)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(enemy.transform.position, enemy.GetEnemyStats().GetStat(EnemyStatistics.Stat.DetectionRadious), layermask.value);

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject.layer == GameManager.GetLayerNumber("Player"))
            {

                enemy.target = colliders[i].gameObject.transform.GetComponent<Player>();
                if (enemy.target.IsPlayerAlive())
                    return true;
            }
        }

        return false;
    }
}
