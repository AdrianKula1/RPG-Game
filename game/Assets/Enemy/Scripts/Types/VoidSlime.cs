using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoidSlime : EnemyType
{
    public override string[] GetTypeAnimationNames()
    {
        string[] animations = { "VoidSlime_Idle", "VoidSlime_TakeDamage" };
        return animations;
    }

    public override float[] GetTypeBaseStats()
    {
        //Health, Speed, Damage, AttackSpeed, DetectionRadious, AttackRadious, Knockback Strength, Knockback Duration
        float[] baseStats = { 90f, 7f, 10f, 1f, 5f, 0.6f, 10f, 0.4f };
        return baseStats;
    }

    public VoidSlime()
    {

    }
}
