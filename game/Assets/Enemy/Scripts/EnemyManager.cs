using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public State currentState;
    public Player target;
    private Dictionary<string, EnemyStatistics> enemyStats;
    public Rigidbody2D rigidBody;
    public float detectionRadious = 5f;
    public float attackRadious = 1.5f;

    private void Awake()
    {
       
        rigidBody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        enemyStats = new Dictionary<string, EnemyStatistics>
        {
            { "Health", new EnemyStatistics("Health", 9f) },
            { "Damage", new EnemyStatistics("Damage", 5f) },
            { "Speed", new EnemyStatistics("Speed", 4f) },
            { "AttackRadious", new EnemyStatistics("AttackRadious", 1.5f) },
            { "DetectionRadious", new EnemyStatistics("DetectionRadious", 5f) }
        };
    }

    void Update()
    {
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
        float health = getStat("Health");//enemyStats["Health"].getValue();
        health -= dmgValue;

        if (health < 0f)
            Die();

        setStat("Health", health);
        //enemyStats["Health"].setValue(health);
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

    public float getStat(string statName)
    {
        return enemyStats[statName].getValue();
    }

    public void setStat(string statName, float value)
    {
        enemyStats[statName].setValue(value);
    }
}
