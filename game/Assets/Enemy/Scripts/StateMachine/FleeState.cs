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
            randomPos = FindPosition(stats.GetStat(EnemyStatistics.Stat.DetectionRadious));
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

    private UnityEngine.GameObject FindPosition(float radious)
    {
        float offset = radious + Random.Range(0f, 3f);  
        float randomAngle = Random.Range(0f, 359f);
        Vector3 newPosition = new Vector3(offset * Mathf.Cos(randomAngle), offset * Mathf.Sin(randomAngle));
        UnityEngine.GameObject temporaryObject = new UnityEngine.GameObject("Flee position");
        temporaryObject.transform.position = newPosition;
        return temporaryObject;
    }
}
