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
    public bool dmgCooldown = false;
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
        MovingDirectionAnimation();
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

                if (transform.CompareTag("Slime"))
                    GameManager.ChangeAnimationState(GetComponent<Animator>(), "Slime_Idle", "Slime_TakeDamage");
                else if(transform.CompareTag("Ghost"))
                    GameManager.ChangeAnimationState(GetComponent<Animator>(), "Ghost_Idle", "Ghost_TakeDamage");

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
        path.canMove = false;
    }

    private IEnumerator KnockCo(float duration)
    {
        yield return new WaitForSeconds(duration);
        rigidBody.velocity = Vector2.zero;
        Knockedback = false;
        path.canMove = true;
    }

    private IEnumerator DmgCooldown()
    {
        yield return new WaitForSeconds(0.3f);
        dmgCooldown = false;
        if (transform.CompareTag("Slime"))
            GameManager.ChangeAnimationState(GetComponent<Animator>(), "Slime_TakeDamage", "Slime_Idle");
        else if (transform.CompareTag("Ghost"))
            GameManager.ChangeAnimationState(GetComponent<Animator>(), "Ghost_TakeDamage", "Ghost_Idle");
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
        if (transform.CompareTag("Ghost"))
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, values[4] * 2);
        }
    }

    public void MovingDirectionAnimation()
    {
        if (!dmgCooldown)
        {
            float velX = path.velocity.x;
            float velY = path.velocity.y;
            SpriteRenderer renderer = GetComponent<SpriteRenderer>();
            renderer.flipX = false;
            renderer.flipY = false;
            Animator anim = GetComponent<Animator>();
            Debug.Log("velx = "+velX.ToString()+", vely = " + velY.ToString());
            if (velX == 0 && velY == 0)
            {
                if (this.CompareTag("Ghost"))
                {
                    GameManager.ChangeAnimationState(anim, "", "Ghost_Idle");
                }
            }
            else
            {
                if (Mathf.Abs(velX) > Mathf.Abs(velY))
                {

                    if (this.CompareTag("Ghost"))
                    {
                        GameManager.ChangeAnimationState(anim, "Ghost_Idle", "Ghost_MoveHorizontal");
                        if (velX < 0)
                        {
                            renderer.flipX = true;
                        }
                    }
                }
                else
                {
                    if (this.CompareTag("Ghost"))
                    {

                        if (velY > 0)
                        {
                            GameManager.ChangeAnimationState(anim, "Ghost_Idle", "Ghost_MoveBackward");
                        }
                        else
                        {
                            GameManager.ChangeAnimationState(anim, "Ghost_MoveBackward", "Ghost_Idle");
                        }
                    }
                }
            }
        }
    }
}
