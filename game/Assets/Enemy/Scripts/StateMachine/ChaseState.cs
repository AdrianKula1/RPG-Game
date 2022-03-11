using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : State
{
    public AttackState attackState;
    public bool inRange;
    public override State RunCurrentState(EnemyManager enemyManager)
    {
        Vector2 currentPosition = enemyManager.transform.position;
        Vector2 targetPosition = enemyManager.target.transform.position;
        float distance = Vector2.Distance(currentPosition, targetPosition);

        if (distance < enemyManager.detectionRadious)
        {
            if (distance > enemyManager.attackRadious)
            {
                enemyManager.transform.position = Vector2.MoveTowards(currentPosition, targetPosition, enemyManager.speed * Time.deltaTime);
            }
            else
            {
                inRange = true;
            }
        }


        if (inRange)
        {
            return attackState;
        }
        else
        {
            return this;
        }
    }
}
