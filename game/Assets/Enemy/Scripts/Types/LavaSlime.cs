using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaSlime : EnemyType
{
    public override float[] GetTypeBaseStats()
    {
        //Health, Speed, Damage, AttackSpeed, DetectionRadious, AttackRadious, Knockback Strength, Knockback Duration
        float[] baseStats = { 40f, 4f, 6f, 0.5f, 5f, 0.6f, 2f, 0.2f };
        return baseStats;
    }

    public override string[] GetTypeAnimationNames()
    {
        string[] animations = { "LavaSlime_Idle", "LavaSlime_TakeDamage" };
        return animations;
    }

    public LavaSlime()
    {

    }
}
