using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flee : Effect
{
    public override bool SetEffect(Dictionary<EnemyStatistics.Stat, EnemyStatistic> dict)
    {
        dict[EnemyStatistics.Stat.Speed].setModifier(1.5f);
        return true;
    }

    public Flee()
    { }
}