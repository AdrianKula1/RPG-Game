using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Enemy : MonoBehaviour
{
    public State currentState;
    public Player target;
    public AIPath path;
    public AIDestinationSetter destinationSetter;
    private EnemyStatistics enemyStats;
    public Rigidbody2D rigidBody;
    public float detectionRadious = 5f;
    public float attackRadious = 1.5f;

    private void Awake()
    {
        path = GetComponent<AIPath>();
        destinationSetter = GetComponent<AIDestinationSetter>();       
        rigidBody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        enemyStats = new EnemyStatistics(9f, 4f, 10f, 1f, 5f, 1.5f, path);
        path.maxSpeed = enemyStats.GetStat(EnemyStatistics.Stat.Speed);
    }

    void Update()
    {
        path.maxSpeed = enemyStats.GetStat(EnemyStatistics.Stat.Speed);
        RunStateMachine();
    }

    private void RunStateMachine()
    {
        State nextState = currentState?.RunCurrentState(this);

        if (nextState != null)
        {
            SwitchToNextState(nextState);
        }
    }

    private void SwitchToNextState(State nextState)
    {
        currentState = nextState;
    }

    public void TakeDamage(float dmgValue)
    {
        float health = enemyStats.GetStat(EnemyStatistics.Stat.Health);
        health -= dmgValue;

        if (health < 0f)
            Die();

        enemyStats.SetStat(EnemyStatistics.Stat.Health, health);
    }

    private void Die()
    {
        Debug.Log("Enemy died");
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, detectionRadious);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadious);
    }


    public bool lowHp()
    {
        return enemyStats.GetStat(EnemyStatistics.Stat.Health) < 10f;
    }

    public EnemyStatistics GetEnemyStats()
    {
        return enemyStats;
    }
}
