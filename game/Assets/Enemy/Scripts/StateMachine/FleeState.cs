using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleeState : State
{
    public IdleState idleState;
    public bool isFarAway;
    [SerializeField] private UnityEngine.GameObject randomPos = null;
    public override State RunCurrentState(Enemy enemy)
    {
        EnemyStatistics stats = enemy.GetEnemyStats();
        stats.ApplyEffect(new Flee());
        isFarAway = false;
        if (randomPos == null)
        {
            randomPos = GameManager.FindPosition(stats.GetStat(EnemyStatistics.Stat.DetectionRadious));
            enemy.destinationSetter.target = randomPos.transform;
        }

        float distance = GameManager.GetDistance(enemy);

        if (distance > stats.GetStat(EnemyStatistics.Stat.DetectionRadious))
        {
            Object.Destroy(randomPos);
            isFarAway = true;
        }

        if (isFarAway)
        {
            enemy.destinationSetter.target = null;
            stats.ApplyEffect(new Standard());
            return idleState;
        }
        else
        {
            return this;
        }
    }
}
