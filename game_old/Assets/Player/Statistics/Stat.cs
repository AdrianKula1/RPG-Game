using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stat
{
    protected string name;

    public Stat(string name)
    {
        this.name = name;
    }

    public string GetStatName()
    {
        return name;
    }
}
