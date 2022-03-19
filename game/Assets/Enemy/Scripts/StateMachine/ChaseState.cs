using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : State
{
    [SerializeField] public LayerMask layermask;
    public AttackState attackState;
    public IdleState idleState;
    public bool inRange;
    public bool outOfRange;
    public override State RunCurrentState(EnemyManager enemyManager)
    {
        inRange = false;
        outOfRange = false;
        if (enemyManager.target != null)
        {
            Vector2 currentPosition = enemyManager.transform.position;
            Vector2 targetPosition = enemyManager.target.transform.position;
            float distance = Vector2.Distance(currentPosition, targetPosition);

            if (distance < enemyManager.detectionRadious)
            {
                if (distance > enemyManager.attackRadious)
                {
                    /*float speed = enemyManager.getStat("Speed");//enemyStats["Speed"].getValue();
                    Vector2 direction = (targetPosition - currentPosition).normalized;
                    enemyManager.rigidBody.velocity = direction * speed;*/
                    enemyManager.destinationSetter.target = enemyManager.target.transform;
                }
                else
                {
                    inRange = true;
                    //enemyManager.rigidBody.velocity = Vector2.zero;
                }
            }
            else
            {
                outOfRange = true;
                //enemyManager.rigidBody.velocity = Vector2.zero;
            }
        }


        if (inRange)
        {
            enemyManager.destinationSetter.target = null;
            return attackState;
        }
        else if (outOfRange)
        {
            return idleState;
        }
        else
        {
            return this;
        }
    }
}
