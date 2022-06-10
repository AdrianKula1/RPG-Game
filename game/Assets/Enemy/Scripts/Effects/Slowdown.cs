using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slowdown : Effect
{
    public override bool SetEffect(Dictionary<EnemyStatistics.Stat, EnemyStatistic> dict)
    {
        dict[EnemyStatistics.Stat.Speed].setModifier(0.1f);
        return true;
    }

    public override bool SetEffectOnPlayer(Dictionary<PlayerStatistics.Stat, PlayerStatistic> dict)
    {
        LevelStatistic statistic = (LevelStatistic)dict[PlayerStatistics.Stat.TotalSpeed];
        statistic.SetModifier(0.1f);

        return true;
    }
    public Slowdown()
    { }
}
