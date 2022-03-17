using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemStatistic
{
    protected string name;
    protected int value;

    public ItemStatistic(string name, int value)
    {
        this.name = name;
        this.value = value;
    }

    public string GetStatName()
    {
        return name;
    }
    public int GetStatValue()
    {
        return value;
    }
}