using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleeState : State
{
    [SerializeField] public LayerMask layermask;
    public IdleState idleState;
    public bool isFarAway;
    private Vector3 randomPosition;
    public override State RunCurrentState(EnemyManager enemyManager)
    {
        //Szuka pozycji w obrêbie pierœcieniu miêdzy detectionRadious a jakimœ offsetem

        //idzie do tej pozycji

        //Id¹c sprawdza czy nie natrafi³ na przeszkodê
        
        //Jeœli tak, szuka nowej pozycji

        //Jeœli znajdzie siê 

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
