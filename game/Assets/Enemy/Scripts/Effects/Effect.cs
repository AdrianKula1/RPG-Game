using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Effect
{
    public abstract bool SetEffect(Dictionary<EnemyStatistics.Stat, EnemyStatistic> dict);
    public abstract bool SetEffectOnPlayer(Dictionary<PlayerStatistics.Stat, PlayerStatistic> dict);
}
