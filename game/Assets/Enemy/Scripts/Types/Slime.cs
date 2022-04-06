using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : EnemyType
{
    public override float[] GetTypeBaseStats()
    {
        //Health, Speed, Damage, AttackSpeed, DetectionRadious, AttackRadious
        float[] baseStats = { 20f, 2f, 3f, 1f, 3.5f, 0.5f };
        return baseStats;
    }

    public Slime()
    {

    }
}
