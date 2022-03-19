using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleeState : State
{
    [SerializeField] public LayerMask layermask;
    public IdleState idleState;
    public bool isFarAway;
    [SerializeField] private GameObject randomPos;
    public override State RunCurrentState(EnemyManager enemyManager)
    {
        isFarAway = false;
        if (randomPos == null)
        {
            randomPos = FindPosition(enemyManager.detectionRadious);
            enemyManager.destinationSetter.target = randomPos.transform;
        }

        if (enemyManager.path.reachedDestination)
        {
            Object.Destroy(randomPos);
            randomPos = null;
            isFarAway = true;
        }

        if (isFarAway)
        {
            enemyManager.destinationSetter.target = null;
            return idleState;
        }
        else
        {
            return this;
        }
    }

    private GameObject FindPosition(float radious)
    {
        float offset = radious + Random.Range(0f, 3f);  
        float randomAngle = Random.Range(0f, 359f);
        Vector3 newPosition = new Vector3(offset * Mathf.Cos(randomAngle), offset * Mathf.Sin(randomAngle));
        GameObject temporaryObject = new GameObject();
        temporaryObject.transform.position = newPosition;
        return temporaryObject;
    }

    /*private bool MoveToPositon(EnemyManager enemyManager)
    {
        float speed = enemyManager.getStat("Speed");
        if (Vector3.Distance(enemyManager.transform.position, randomPos) > 0.5f)
        {
            Vector3 direction = (randomPos - enemyManager.transform.position).normalized;
            enemyManager.rigidBody.velocity = direction * speed * 1.5f;
            return false;
        }
        else
        {
            enemyManager.rigidBody.velocity = Vector3.zero;
            randomPos = Vector3.zero;
            return true;
        }
    }*/

    private void CheckCollisions(EnemyManager enemyManager)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(enemyManager.transform.position, enemyManager.attackRadious, layermask.value);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject.layer == GameManager.GetLayerNumber("Obstacle"))
            {
                randomPos = FindPosition(enemyManager.detectionRadious);
            }
        }
    }
}
