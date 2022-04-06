using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static EnemyTypes enemyTypes = new EnemyTypes();
    public static int GetLayerNumber(string layerName)
    {
        int layerNumber = 0;
        int layer = LayerMask.GetMask(layerName);
        while (layer > 0)
        {
            layer >>= 1;
            layerNumber++;
        }
        return layerNumber -1;
    }

    public static float GetDistance(Enemy enemy)
    {
        Vector2 currentPosition = enemy.transform.position;
        Vector2 targetPosition = enemy.target.transform.position;
        return Vector2.Distance(currentPosition, targetPosition);
    }

    public static void ChangeAnimationState(Animator animator, string currentState, string newState)
    {
        if (currentState == newState)
            return;

        animator.Play(newState);

        currentState = newState;
    }

    public static EnemyType GetEnemyTypeByTag(string tag)
    {
        return enemyTypes.GetType(tag);
    }

    public static void PlayParticles(ParticleSystem particles)
    {
        particles.Play();
    }

    public static void StopParticles(ParticleSystem particles)
    {
        particles.Stop();
    }
}
