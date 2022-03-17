using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleeState : State
{
    [SerializeField] public LayerMask layermask;
    public IdleState idleState;
    public bool isFarAway;
    [SerializeField] private Vector3 randomPos;
    public override State RunCurrentState(EnemyManager enemyManager)
    {
        isFarAway = false;
        //Szuka pozycji w obrêbie pierœcienia miêdzy detectionRadious a jakimœ offsetem
        randomPos = (randomPos == Vector3.zero) ? FindPosition(enemyManager.detectionRadious) : randomPos;
        //idzie do tej pozycji
        if (!MoveToPositon(enemyManager))
        {
            /*Collider2D[] colliders = Physics2D.OverlapCircleAll(enemyManager.transform.position, enemyManager.attackRadious, layermask.value);
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].gameObject.layer == GameManager.GetLayerNumber("Obstacle"))
                {
                    randomPos = FindPosition(enemyManager.detectionRadious);
                }
            }*/
            CheckCollisions(enemyManager);
        }
        else
        {
            isFarAway = true;
        }


        if (isFarAway)
        {
            return idleState;
        }
        else
        {
            return this;
        }
    }

    private Vector3 FindPosition(float radious)
    {
        float offset = radious + Random.Range(0f, 3f);  
        float randomAngle = Random.Range(0f, 359f);
        Vector3 newPosition = new Vector3(offset * Mathf.Cos(randomAngle), offset * Mathf.Sin(randomAngle));
        return newPosition;
    }

    private bool MoveToPositon(EnemyManager enemyManager)
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
    }

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
