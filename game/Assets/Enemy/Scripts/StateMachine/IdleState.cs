using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
    [SerializeField] public LayerMask layermask;
    public ChaseState chaseState;
    public bool canSeePlayer;
    public override State RunCurrentState(EnemyManager enemyManager)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(enemyManager.transform.position, enemyManager.detectionRadious, layermask.value);

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject.layer == GameManager.GetLayerNumber("Player"))
            {
                canSeePlayer = true;
                enemyManager.target = colliders[i].gameObject.transform.GetComponent<Player>();
            }
        }


        if (canSeePlayer)
        {
            return chaseState;
        }
        else
        {
            return this;
        }
    }
}
