using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private float health = 100f;
    private bool immunity = false;
    

    private void Awake()
    {
    }

    public void SetImmunity(bool immunityCondition)
    {
        immunity = immunityCondition;
    }

    public void TakeDamage(float damageValue)
    {
        if (!immunity)
            health -= damageValue;

        if (health <= 0)
            Die();
    }

    private void Die()
    {
        Debug.Log("Player died");
    }
}
