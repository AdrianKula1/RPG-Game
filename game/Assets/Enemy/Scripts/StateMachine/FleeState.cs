using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleeState : State
{
    [SerializeField] public LayerMask layermask;
    public IdleState idleState;
    public bool isFarAway;
    [SerializeField] private GameObject randomPos;
    public override State RunCurrentState(Enemy enemyManager)
    {
        isFarAway = false;
        if (randomPos == null)
        {
            randomPos = FindPosition(enemyManager.detectionRadious);
            enemyManager.destinationSetter.target = randomPos.transform;
        }

        if (enemyManager.path.reachedDestination)
        {
            randomPos = null;
            Object.Destroy(randomPos);
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
}
