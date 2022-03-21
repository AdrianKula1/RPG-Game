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

    public Standard()
    { }
}
