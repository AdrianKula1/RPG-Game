using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelStatistic : PlayerStatistic
{
    private int Level;
    private float TotalValue;
    private float Modifier;
    public LevelStatistic(int level, float totalValue)
    {
        Level = level;
        TotalValue = totalValue;
        Modifier = 1f;
    }

    public override int GetLevel()
    {
        return Level;
    }

    public override void LevelUp()
    {
        Level++;
    }

    public void SetTotalValue(float newValue)
    {
        TotalValue = newValue;
    }

    public float GetTotalValue()
    {
        return TotalValue * Modifier;
    }

    public void SetModifier(float modifierValue)
    {
        Modifier = modifierValue;
    }
}
