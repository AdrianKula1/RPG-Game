using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MudSlime : EnemyType
{
    public override string[] GetTypeAnimationNames()
    {
        string[] animations = { "MudSlime_Idle", "MudSlime_TakeDamage" };
        return animations;
    }

    public override float[] GetTypeBaseStats()
    {
        //Health, Speed, Damage, AttackSpeed, DetectionRadious, AttackRadious, Knockback Strength, Knockback Duration
        float[] baseStats = { 60f, 1f, 15f, 5f, 2f, 0.6f, 7f, 0.2f };
        return baseStats;
    }
}
