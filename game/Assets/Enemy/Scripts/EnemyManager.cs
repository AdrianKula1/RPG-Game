using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public State currentState;
    public Player target;
    public Dictionary<string, EnemyStatistics> enemyStats;
    public Rigidbody2D rigidBody;
    public float detectionRadious = 5f;
    public float attackRadious = 1.5f;

    private void Awake()
    {
       
        rigidBody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        enemyStats = new Dictionary<string, EnemyStatistics>();
        enemyStats.Add("Health", new EnemyStatistics("Health", 100f));
        enemyStats.Add("Damage", new EnemyStatistics("Damage", 5f));
        enemyStats.Add("Speed", new EnemyStatistics("Speed", 4f));
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
        float health = enemyStats["Health"].getValue();
        health -= dmgValue;

        if (health < 0f)
            Die();

        enemyStats["Health"].setValue(health);
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
}
