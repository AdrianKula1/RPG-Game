using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarStatistic : PlayerStatistic
{
    private float Value;
    private float MaxValue;
    private int Level;
    public BarStatistic(float maxValue)
    {
        MaxValue = maxValue;
        Level = 0;
        Value = maxValue;
    }

    public float GetMaxValue()
    {
        return MaxValue;
    }

    public float GetValue()
    {
        return Value;
    }

    public void SetValue(float value)
    {
        Value = value;

        if (Value > MaxValue)
            Value = MaxValue;
    }

    public override int GetLevel()
    {
        return Level;
    }

    public override void LevelUp()
    {
        Level++;
        MaxValue += Level * 10f;
    }
}
