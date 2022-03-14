using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleeState : State
{
    public IdleState idleState;
    public bool isFarAway;
    public override State RunCurrentState(EnemyManager enemyManager)
    {
        if (isFarAway)
        {
            return idleState;
        }
        else
        {
            return this;
        }
    }
}
