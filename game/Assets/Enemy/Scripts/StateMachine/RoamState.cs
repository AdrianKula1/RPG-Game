using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoamState : State
{
    public IdleState idleState;
    public ChaseState chaseState;
    public bool isClose = false;
    public bool isFar = false;
    [SerializeField] private UnityEngine.GameObject randomPos = null;
    public override State RunCurrentState(Enemy enemy)
    {
        isClose = false;
        isFar = false;
        EnemyStatistics stats = enemy.GetEnemyStats();
        if (randomPos == null)
        {
            randomPos = GameManager.FindPosition(stats.GetStat(EnemyStatistics.Stat.DetectionRadious));
            enemy.destinationSetter.target = randomPos.transform;
        }

        float distance = GameManager.GetDistance(enemy);

        if (distance <= stats.GetStat(EnemyStatistics.Stat.DetectionRadious))
        {
            Object.Destroy(randomPos);
            isClose = true;
        }
        else if (distance > stats.GetStat(EnemyStatistics.Stat.DetectionRadious) * 2)
        {
            Object.Destroy(randomPos);
            isFar = true;
        }

        if (isClose)
        {
            enemy.destinationSetter.target = null;
            return chaseState;
        }
        else if (isFar)
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
