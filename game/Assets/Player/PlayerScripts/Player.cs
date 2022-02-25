using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private bool immunity = false;
    public Dictionary<string, PlayerStatistic> statistics;
    

    private void Start()
    {
        statistics = new Dictionary<string, PlayerStatistic>();
        statistics.Add("Health", new PlayerStatistic("Health", 1, 100f));
        statistics.Add("Stamina", new PlayerStatistic("Stamina", 1, 100f));
        statistics.Add("Mana", new PlayerStatistic("Mana", 1, 100f));
    }

    public void SetImmunity(bool immunityCondition)
    {
        immunity = immunityCondition;
    }

    public void TakeDamage(float damageValue)
    {
        if (!immunity)
        {
            float health = statistics["Health"].GetValue();
            health -= damageValue;
            statistics["Health"].SetValue(health);
        }

        if (statistics["Health"].GetValue() <= 0)
            Die();
    }

    private void Die()
    {
        Debug.Log("Player died");
    }
}
