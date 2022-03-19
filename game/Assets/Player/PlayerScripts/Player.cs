using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private bool immunity = false;
    private bool isAlive = true;
    private Dictionary<string, PlayerStatistic> playerStats;

    private Inventory inventory;
    [SerializeField] private UI_Inventory uiInventory;
    //Inicjuje statystyki gracza
    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        playerStats = new Dictionary<string, PlayerStatistic>
        {
            { "Health", new PlayerStatistic("Health", 1, 100f) },
            { "Stamina", new PlayerStatistic("Stamina", 1, 100f) },
            { "Mana", new PlayerStatistic("Mana", 1, 100f) }
        };

        inventory = new Inventory();
        uiInventory.SetInventory(inventory);
    }



    public void SetImmunity(bool immunityCondition)
    {
        immunity = immunityCondition;
    }
    //Otrzymywanie obra¿eñ, immunity ma dzia³aæ jak cooldown tak¿e
    //¯eby nie otrzymaæ 1000 uderzeñ w jednej sekundzie gdy przeciwnik zaatakuje
    public void TakeDamage(float damageValue)
    {
        float health = getStat("Health");

        if (!immunity)
        {
            health -= damageValue;
            //playerStats["Health"].SetValue(health);
            StartCoroutine(TakeDamageCooldown());
        }

        if (health <= 0)
        {
            Die();
            isAlive = false;
        }

        setStat("Health", health);
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

    public float getStat(string statName)
    {
        return playerStats[statName].GetValue();
    }

    public void setStat(string statName, float value)
    {
        playerStats[statName].SetValue(value);
    }
}
