using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Enemy : Character
{
    public State currentState;
    public Player target;
    public AIPath path;
    public AIDestinationSetter destinationSetter;
    private EnemyStatistics enemyStats;
    public Rigidbody2D rigidBody;
    public ParticleSystem particles;
    //public float detectionRadious = 5f;
    //public float attackRadious = 1.5f;

    private void Awake()
    {
        path = GetComponent<AIPath>();
        destinationSetter = GetComponent<AIDestinationSetter>();       
        rigidBody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        float[] stats = GameManager.GetEnemyTypeByTag(this.tag).GetTypeBaseStats();
        enemyStats = new EnemyStatistics(stats[0], stats[1], stats[2], stats[3], stats[4], stats[5], path);
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

    public override void TakeDamage(float dmgValue, Vector3 knockback)
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
    public bool lowHp()
    {
        return enemyStats.GetStat(EnemyStatistics.Stat.Health) < 10f;
    }

    public EnemyStatistics GetEnemyStats()
    {
        return enemyStats;
    }

    private void OnDrawGizmosSelected()
    {
        float[] values = new Slime().GetTypeBaseStats();
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, values[4]);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, values[5]);
    }
}
