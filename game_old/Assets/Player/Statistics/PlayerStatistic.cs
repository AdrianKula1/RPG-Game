using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatistic : Stat
{
    //Opcjonalnie, byæ mo¿e trzeba bêdzie rozdzieliæ
    //klasê na statystyki zwi¹zane z levelem i te maj¹ce wartoœæ
    //jak hp, stamina i mana
    private float value;

    private int level;
    public PlayerStatistic(string name, int level, float value) : base(name)
    {
        this.level = level;
        this.value = value;
    }

    public float GetValue()
    {
        return value;
    }

    public void SetValue(float value)
    {
        this.value = value;
    }
}
