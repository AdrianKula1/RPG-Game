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
    private bool isAttacking = false;
    public Bar HealthBar;
    public Bar ManaBar;
    public Bar StaminaBar;
    public Transform attackAnim;
   
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

        inventory = new Inventory(UseItem);
        uiInventory.SetInventory(inventory);
        uiInventory.SetPlayer(this);
        animator = GetComponent<Animator>();
        movement = GetComponent<MovementScript>();
        uiInventory.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!uiInventory.gameObject.active)
            {
                Attack();
            }
            
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            if (!uiInventory.gameObject.active)
            {
                uiInventory.gameObject.SetActive(true);
            }
            else
            {
                uiInventory.gameObject.SetActive(false);
            }

        }
        HealthBar.UpdateBar(PlayerStats.GetValue(PlayerStatistics.Stat.Health), PlayerStats.GetMaxValue(PlayerStatistics.Stat.Health));
        HealthBar.UpdateBar(PlayerStats.GetValue(PlayerStatistics.Stat.Mana), PlayerStats.GetMaxValue(PlayerStatistics.Stat.Mana));
        StaminaBar.UpdateBar(PlayerStats.GetValue(PlayerStatistics.Stat.Stamina), PlayerStats.GetMaxValue(PlayerStatistics.Stat.Stamina));

    }

    public void SetImmunity(bool immunityCondition)
    {
        immunity = immunityCondition;
    }
    //Otrzymywanie obra�e�, immunity ma dzia�a� jak cooldown tak�e
    //�eby nie otrzyma� 1000 uderze� w jednej sekundzie gdy przeciwnik zaatakuje
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

    }

    public void Heal(float healingPoints)
    {
        float health = PlayerStats.GetValue(PlayerStatistics.Stat.Health);
        health += healingPoints;
        float maxHealth = PlayerStats.GetMaxValue(PlayerStatistics.Stat.Health);
        //Cooldown na uyżywanie potek?

        if(health >= maxHealth)
        {
            PlayerStats.SetValue(PlayerStatistics.Stat.Health, maxHealth);
        }
        else
        {
            PlayerStats.SetValue(PlayerStatistics.Stat.Health, health);
        }

        


    }


    public void ChangeStamina(float value)
    {
        float stamina = PlayerStats.GetValue(PlayerStatistics.Stat.Stamina);
        float maxStamina = PlayerStats.GetMaxValue(PlayerStatistics.Stat.Stamina);

        stamina += value;
        PlayerStats.SetValue(PlayerStatistics.Stat.Stamina, stamina);
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

    public void Attack()
    {
        if (!isAttacking)
        {
            isAttacking = true;
            //animation on i po chwili off
            SpriteRenderer animRenderer = attackAnim.GetComponent<SpriteRenderer>();
            GameObject ob = animRenderer.gameObject;
            ob.SetActive(true); 
            AttackArea area = FlipAnimation(animRenderer);
            area.gameObject.SetActive(true);
            StartCoroutine(AttackCooldown(ob, area.gameObject));
        }
    }

    private AttackArea FlipAnimation(SpriteRenderer animRenderer)
    {
        AttackArea direction;
        animRenderer.flipY = false;
        animRenderer.flipX = false;
        if (movement.direction == MovementScript.Direction.Forward)
        {
            //x = -0.035 y = -0.58 rot z = -90 flip - y, order = 1
            attackAnim.position = transform.position + new Vector3(-0.035f, -0.58f);
            attackAnim.eulerAngles = Vector3.forward * -90;
            animRenderer.flipY = true;
            animRenderer.sortingOrder = 2;
            direction = transform.GetChild(3).GetComponent<AttackArea>();
        }
        else if (movement.direction == MovementScript.Direction.Right)
        {
            //x = 0.5 y = -0.05 flip , order = 1
            attackAnim.position = transform.position + new Vector3(0.5f, -0.05f);
            attackAnim.rotation = new Quaternion(0, 0, 0, 0);
            animRenderer.sortingOrder = 2;
            direction = transform.GetChild(1).GetComponent<AttackArea>();
        }
        else if (movement.direction == MovementScript.Direction.Left)
        {
            //x = -0.5 y = -0.03 flip - x , order = 0
            attackAnim.position = transform.position + new Vector3(-0.5f, -0.03f);
            attackAnim.rotation = new Quaternion(0, 0, 0, 0);
            animRenderer.flipX = true;
            animRenderer.sortingOrder = 0;
            direction = transform.GetChild(2).GetComponent<AttackArea>();
        }
        else
        {
            //x = 0.09 y = 0.445 rot z = 75 flip, order = 0
            attackAnim.position = transform.position + new Vector3(0.09f, 0.445f);
            attackAnim.eulerAngles = Vector3.forward * 75;
            animRenderer.sortingOrder = 0;
            direction = transform.GetChild(4).GetComponent<AttackArea>();
        }

        return direction;
    }

    private IEnumerator AttackCooldown(GameObject ob, GameObject area)
    {
        yield return new WaitForSeconds(0.2f);
        isAttacking = false;
        ob.SetActive(false);
        area.SetActive(false);
    }   

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ItemWorld itemWorld = collision.GetComponent<ItemWorld>();
        if (itemWorld != null)
        {
            inventory.AddItem(itemWorld.GetItem());
            itemWorld.DestroySelf();
        }
    }

    private void UseItem(Item item)
    {
        switch (item.itemType)
        {
            case Item.ItemType.healthPotion:
                Debug.Log("Item used");
                Debug.Log("Life before: " + PlayerStats.GetValue(PlayerStatistics.Stat.Health));
                Heal(20);
                Debug.Log("Life after: " + PlayerStats.GetValue(PlayerStatistics.Stat.Health));
                inventory.RemoveItem(new Item { itemType = Item.ItemType.healthPotion, amount=1});
                break;

            case Item.ItemType.manaPotion:
                Debug.Log("Item used");
                inventory.RemoveItem(new Item { itemType = Item.ItemType.manaPotion, amount = 1 });
                break;
        }
    }

    public Vector3 GetPosition()
    {
        return this.transform.position;
    }
    
}
