using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public static Action OnMinuteChanged;
    public static Action OnHourChanged;
    public static int Minute { get; private set; }
    public static int Hour { get; private set; }
    private float minuteToRealTime = 2f;
    private float timer;

    private static EnemyTypes enemyTypes = new EnemyTypes();

    private void Start()
    {
        Minute = 0;
        Hour = 12;
        timer = minuteToRealTime;
    }

    private void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            Minute++;
            OnMinuteChanged?.Invoke();

            if (Minute >= 60)
            {
                Hour++;
                OnHourChanged?.Invoke();

                Minute = 0;
            }

            if (Hour >= 24)
            {
                Hour = 0;
            }

            timer = minuteToRealTime;
        }
    }

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
