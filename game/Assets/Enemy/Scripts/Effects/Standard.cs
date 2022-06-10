using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Standard : Effect
{
    public override bool SetEffect(Dictionary<EnemyStatistics.Stat, EnemyStatistic> dict)
    {
        foreach(KeyValuePair<EnemyStatistics.Stat, EnemyStatistic> entry in dict)
        {
            entry.Value.setModifier(1f);
        }

        return true;
    }

    public override bool SetEffectOnPlayer(Dictionary<PlayerStatistics.Stat, PlayerStatistic> dict)
    {
        foreach (KeyValuePair<PlayerStatistics.Stat, PlayerStatistic> entry in dict)
        {
            if (entry.Key != PlayerStatistics.Stat.Health && entry.Key != PlayerStatistics.Stat.Mana && entry.Key != PlayerStatistics.Stat.Stamina)
            {
                LevelStatistic statistic = (LevelStatistic)entry.Value;
                statistic.SetModifier(1f);
            }
        }

        return true;
    }

    public Standard()
    { }
}
