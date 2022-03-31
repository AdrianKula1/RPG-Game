using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D collision)
    {
        //S¹ sprawdzane przez konkretn¹ warstwê (layer) obiektu z jakim gracz koliduje
        if (collision.gameObject.layer == GameManager.GetLayerNumber("Player") ||
            collision.gameObject.layer == GameManager.GetLayerNumber("Enemy"))
        {
            Character character = collision.GetComponent<Character>();
            character.TakeDamage(10f);
        }

    }
}
