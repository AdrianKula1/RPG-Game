using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : State
{
    [SerializeField] public LayerMask layermask;
    public AttackState attackState;
    public IdleState idleState;
    public bool inRange;
    public bool outOfRange;
    public override State RunCurrentState(Enemy enemy)
    {
        EnemyStatistics stats = enemy.GetEnemyStats();
        inRange = false;
        outOfRange = false;
        if (enemy.target != null)
        {
            float distance = GameManager.GetDistance(enemy);

            if (distance < stats.GetStat(EnemyStatistics.Stat.DetectionRadious))
            {
                if (distance > stats.GetStat(EnemyStatistics.Stat.AttackRadious))
                {
                    enemy.destinationSetter.target = enemy.target.transform;
                }
                else
                {
                    inRange = true;
                }
            }
            else
            {
                outOfRange = true;
            }
        }


        if (inRange)
        {
            enemy.destinationSetter.target = null;
            return attackState;
        }
        else if (outOfRange)
        {
            enemy.destinationSetter.target = null;
            return idleState;
        }
        else
        {

            return this;
        }
    }
}
