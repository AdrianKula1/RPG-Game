using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatistics 
{
    public enum Stat
    {
        Health,
        Speed,
        Damage,
        AttackSpeed,
        DetectionRadious,
        AttackRadious
    }

    private Dictionary<Stat, EnemyStatistic> Statistics;

    public EnemyStatistics(float health, float speed, float damage, float attackSpeed, float detectionRadious, float attackRadious)
    {
        Statistics = new Dictionary<Stat, EnemyStatistic>
        {
            {Stat.Health, new EnemyStatistic(health)},
            {Stat.Speed, new EnemyStatistic(speed)},
            {Stat.Damage, new EnemyStatistic(damage)},
            {Stat.AttackSpeed, new EnemyStatistic(attackSpeed)},
            {Stat.DetectionRadious, new EnemyStatistic(detectionRadious)},
            {Stat.AttackRadious, new EnemyStatistic(attackRadious)}
        };
    }

    public float GetStat(Stat statType)
    {
        return Statistics[statType].getValue();
    }

    public void SetStat(Stat statType, float value)
    {
        Statistics[statType].setValue(value);
    }

    //TODO
    public void ApplyEffect()
    {

    }
}
