using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelStatistic : PlayerStatistic
{
    private int Level;

    public LevelStatistic(int level)
    {
        Level = level;
    }

    public override int GetLevel()
    {
        return Level;
    }

    public override void LevelUp()
    {
        Level++;
    }
}
