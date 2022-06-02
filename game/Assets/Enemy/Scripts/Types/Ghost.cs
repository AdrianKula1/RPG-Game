using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : EnemyType
{
    public override float[] GetTypeBaseStats()
    {
        //Health, Speed, Damage, AttackSpeed, DetectionRadious, AttackRadious, Knockback Strength, Knockback Duration
        float[] baseStats = { 80f, 3f, 7.5f, 2f, 4.5f, 0.5f, 1f, 0.2f };
        return baseStats;
    }

    public Ghost()
    {

    }
}
