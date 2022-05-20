using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackArea : MonoBehaviour
{
    private Player player;
    public void OnTriggerStay2D(Collider2D collision)
    {
        player = GetComponentInParent<Player>();
        if (collision.gameObject.layer == GameManager.GetLayerNumber("Enemy"))
        { 
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            Vector3 knockback = enemy.transform.position - player.transform.position;
            enemy.TakeDamage(5, knockback, 5f, 0.2f);
        }
        
    }
}
