using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatistic
{
    //Prosta klasa przechowuj¹ca wartoœæ danej statystyki
    private float value;
    private float modifier;

    public EnemyStatistic(float value)
    {
        this.value = value;
        modifier = 1f;
    }

    public void setValue(float newValue)
    {
        value = newValue;
    }

    public void setModifier(float newModifier)
    {
        modifier = newModifier;
    }
    //Inaczej jak u gracza, u przeciwnika do ró¿nych statystyk
    //stosowany jest modifier, który w zale¿noœci od danego efektu na przeciwniku
    //ma zmieniaæ wartoœæ statystyki
    public float getValue()
    {
        return value * modifier;
    }
}
