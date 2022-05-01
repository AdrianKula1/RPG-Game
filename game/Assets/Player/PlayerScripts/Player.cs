using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    private bool immunity = false;
    private bool isAlive = true;
    private PlayerStatistics PlayerStats;
    private Dictionary<Animation, string> Animations;
    private MovementScript movement;
    public Bar HealthBar;
    public Bar ManaBar;
    public Bar StaminaBar;
   
    public Animator animator;
    private string currentState;

    private Inventory inventory;
    [SerializeField] private UI_Inventory uiInventory;


    //Animacje
    public enum Animation
    {
        IdleFront,
        IdleRight,
        IdleLeft,
        IdleBackward,
        MoveForward,
        MoveRight,
        MoveLeft,
        MoveBackward,
        SprintForward,
        SprintRight,
        SprintLeft,
        SprintBackward,
        DashForward,
        DashLeft,
        DashRight,
        DashBackward
    }

    //Inicjuje statystyki gracza
    private void Start()
    {

        PlayerStats = new PlayerStatistics(100f, 100f, 100f);

        Animations = new Dictionary<Animation, string>
        {
            {Animation.IdleFront, "PlayerIdleFront" },
            {Animation.IdleRight, "PlayerIdleRight" },
            {Animation.IdleLeft, "PlayerIdleLeft" },
            {Animation.IdleBackward, "PlayerIdleBack" },
            {Animation.MoveForward, "PlayerWalkForward" },
            {Animation.MoveRight, "PlayerWalkRight" },
            {Animation.MoveLeft, "PlayerWalkLeft" },
            {Animation.MoveBackward, "PlayerWalkBack" },
            {Animation.SprintForward, "PlayerSprintForward" },
            {Animation.SprintRight, "PlayerSprintRight" },
            {Animation.SprintLeft, "PlayerSprintLeft" },
            {Animation.SprintBackward, "PlayerSprintBack" },
            {Animation.DashForward, "PlayerDashForward" },
            {Animation.DashRight, "PlayerDashRight" },
            {Animation.DashLeft, "PlayerDashLeft" },
            {Animation.DashBackward, "PlayerDashBack" },
        };

        inventory = new Inventory();
        uiInventory.SetInventory(inventory);
        animator = GetComponent<Animator>();
        movement = GetComponent<MovementScript>();
    }

    

    public void SetImmunity(bool immunityCondition)
    {
        immunity = immunityCondition;
    }
    //Otrzymywanie obra¿eñ, immunity ma dzia³aæ jak cooldown tak¿e
    //¯eby nie otrzymaæ 1000 uderzeñ w jednej sekundzie gdy przeciwnik zaatakuje
    public override void TakeDamage(float damageValue, Vector3 knockback, float knockbackStrength, float knockbackDuration)
    {
        float health = PlayerStats.GetValue(PlayerStatistics.Stat.Health);
        float maxHealth = PlayerStats.GetMaxValue(PlayerStatistics.Stat.Health);

        if (!immunity && health > 0)
        {
            health -= damageValue;
            StartCoroutine(TakeDamageCooldown());
            movement.Knockback(knockback, knockbackStrength, knockbackDuration);
        }

        if (health <= 0)
        {
            Die();
            isAlive = false;
        }

        PlayerStats.SetValue(PlayerStatistics.Stat.Health, health);
        HealthBar.UpdateBar(health, maxHealth);
    }

    public void ChangeStamina(float value)
    {
        float stamina = PlayerStats.GetValue(PlayerStatistics.Stat.Stamina);
        float maxStamina = PlayerStats.GetMaxValue(PlayerStatistics.Stat.Stamina);

        stamina += value;
        PlayerStats.SetValue(PlayerStatistics.Stat.Stamina, stamina);
        StaminaBar.UpdateBar(stamina, maxStamina);
    }

    private void Die()
    {
        Debug.Log("Player died");
    }

    public bool IsPlayerAlive()
    {
        return isAlive;
    }    

    public IEnumerator TakeDamageCooldown()
    {
        immunity = true;
        yield return new WaitForSeconds(1f);
        immunity = false;
    }

    public string getAnimationName(Animation anim)
    {
        return Animations[anim];
    }

    public void ChangeAnimationState(string newState)
    {
        if (currentState == newState)
            return;

        animator.Play(newState);

        currentState = newState;
    }

    public PlayerStatistics GetStats()
    {
        return PlayerStats;
    }
}
