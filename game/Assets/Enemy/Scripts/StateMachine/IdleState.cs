using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
    [SerializeField] public LayerMask layermask;
    public ChaseState chaseState;
    public FleeState fleeState;
    public RoamState roamstate;
    public bool canSeePlayer;
    public override State RunCurrentState(Enemy enemy)
    {
        canSeePlayer = false;
        if (enemy.CompareTag("Ghost"))
        {
            canSeePlayer = CanSeePlayer(enemy, enemy.GetEnemyStats().GetStat(EnemyStatistics.Stat.DetectionRadious) * 2);
        }
        else
        {
            canSeePlayer = CanSeePlayer(enemy, enemy.GetEnemyStats().GetStat(EnemyStatistics.Stat.DetectionRadious));
        }

        if (canSeePlayer)
        {
            if (enemy.lowHp())
            {
                return fleeState;
            }
            else
            {
                if (enemy.CompareTag("Ghost"))
                {
                    return roamstate;
                }
                else
                {
                    return chaseState;
                }
            }
        }
        else
        {
            return this;
        }
    }

    private bool CanSeePlayer(Enemy enemy, float range)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(enemy.transform.position, range, layermask.value);

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject.layer == GameManager.GetLayerNumber("Player"))
            {

                enemy.target = colliders[i].gameObject.GetComponent<Player>();
                return true;
            }
        }
        enemy.target = null;
        return false;
    }
}
