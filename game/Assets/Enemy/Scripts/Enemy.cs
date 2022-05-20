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
    private bool Knockedback = false;
    private bool dmgCooldown = false;
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
        enemyStats = new EnemyStatistics(stats[0], stats[1], stats[2], stats[3], stats[4], stats[5], stats[6], stats[7], path);
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

    public override void TakeDamage(float dmgValue, Vector3 knockback, float knockbackStrength, float knockbackDuration)
    {
        if (!dmgCooldown)
        {
            
            float health = enemyStats.GetStat(EnemyStatistics.Stat.Health);
            if (health < 0f)
                Die();
            else
            {
                health -= dmgValue;
                Knockback(knockback, knockbackStrength, knockbackDuration);
                dmgCooldown = true;
                StartCoroutine(DmgCooldown());
            }

            enemyStats.SetStat(EnemyStatistics.Stat.Health, health);
        }
    }

    public void Knockback(Vector3 knockback, float strength, float duration)
    {
        Knockedback = true;
        rigidBody.AddForce(knockback.normalized * strength, ForceMode2D.Impulse);
        StartCoroutine(KnockCo(duration));
    }

    private IEnumerator KnockCo(float duration)
    {
        yield return new WaitForSeconds(duration);
        Knockedback = false;
    }

    private IEnumerator DmgCooldown()
    {
        yield return new WaitForSeconds(0.3f);
        dmgCooldown = false;
    }

    private void Die()
    {
        Debug.Log("Enemy died");
        Destroy(gameObject);
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
