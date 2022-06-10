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
    private EnemyAnimations enemyAnims;
    private Animator animator;
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
        enemyAnims = new EnemyAnimations(tag);
        path.maxSpeed = enemyStats.GetStat(EnemyStatistics.Stat.Speed);
        animator = GetComponent<Animator>();
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
                animator.Play(enemyAnims.GetAnimation(EnemyAnimations.Animation.TakeDamage));
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
        animator.Play(enemyAnims.GetAnimation(EnemyAnimations.Animation.Idle));
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
        float[] values = { };
        if (transform.CompareTag("Slime"))
            values = new Slime().GetTypeBaseStats();
        else if (transform.CompareTag("Ghost"))
            values = new Ghost().GetTypeBaseStats();
        else if (transform.CompareTag("LavaSlime"))
            values = new LavaSlime().GetTypeBaseStats();
        else if (transform.CompareTag("MudSlime"))
            values = new MudSlime().GetTypeBaseStats();
        else if (transform.CompareTag("VoidSlime"))
            values = new VoidSlime().GetTypeBaseStats();

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
            if (velX == 0 && velY == 0)
            {
                if (this.CompareTag("Ghost"))
                {
                    animator.Play(enemyAnims.GetAnimation(EnemyAnimations.Animation.Idle));
                }
            }
            else
            {
                if (Mathf.Abs(velX) > Mathf.Abs(velY))
                {

                    if (this.CompareTag("Ghost"))
                    {
                        animator.Play(enemyAnims.GetAnimation(EnemyAnimations.Animation.MoveRight));
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
                            animator.Play(enemyAnims.GetAnimation(EnemyAnimations.Animation.MoveBackward));
                        }
                        else
                        {
                            animator.Play(enemyAnims.GetAnimation(EnemyAnimations.Animation.Idle));
                        }
                    }
                }
            }
        }
    }
}
