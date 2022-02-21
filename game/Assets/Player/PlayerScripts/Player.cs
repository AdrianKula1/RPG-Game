using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Transform playerTransform;
    private Rigidbody2D rb2D;
    private bool dashCooldown;
    private static Player instance;
    private float speed = 4f;
    private float stamina = 100f;
    private float health = 100f;
    private void Start()
    {
        instance = this;
        playerTransform = GetComponent<Transform>();
        rb2D = GetComponent<Rigidbody2D>();
    }
    
    private void Update()
    {
        if (stamina < 100f)
            stamina += 0.01f;
        else
            stamina = 100f;
    }

    public void Move(Vector3 direction, bool sprint, bool dash)
    {
        float finalSpeed = speed;
        bool dashed = false;

        if (sprint && stamina > 0f)
        {
            finalSpeed = speed * 2;
        }
        else if (dash && !dashCooldown && stamina > 30f)
        {
            finalSpeed = 600f;
            dashed = true;
        }

        if(TryMove(direction, finalSpeed * Time.deltaTime))
        {
            if (sprint)
            {
                stamina -= 0.05f;
            }

            if (dashed)
            {
                stamina -= 30f;
                StartCoroutine(DashCoolDown());
            }
        }
    }
    
    public static int getLayerNumber(string layerName)
    {
        int layerNumber = 0;
        int layer = LayerMask.GetMask(layerName);
        while (layer > 0)
        {
            layer >>= 1;
            layerNumber++;
        }
        return layerNumber -1;
    }

    private bool CanMove(Vector3 direction, float distance)
    {
        return Physics2D.Raycast(playerTransform.position, direction, distance, getLayerNumber("Obstacle")).collider == null;
    }

    private bool TryMove(Vector3 baseMoveDirection, float distance)
    {
        Vector3 moveDirection = baseMoveDirection;
        bool canMove = CanMove(moveDirection, distance);
        if (!canMove)
        {
            moveDirection = new Vector3(baseMoveDirection.x, 0f).normalized;
            canMove = moveDirection.x != 0f && CanMove(moveDirection, distance);
            if (!canMove)
            {
                moveDirection = new Vector3(0f, baseMoveDirection.y).normalized;
                canMove = moveDirection.y != 0f && CanMove(moveDirection, distance);
            }
        }

        if (canMove)
        {
            playerTransform.position += moveDirection * distance;
            //rb2D.MovePosition(rb2D.position + new Vector2(moveDirection.x, moveDirection.y) * distance * Time.deltaTime);
            return true;
        }
        else
        {
            return false;
        }
    }

    public void TakeDamage(float damageValue)
    {
        health -= damageValue;

        if (health <= 0)
            Die();
    }    

    private void Die()
    {
        Debug.Log("Player died");
    }

    public static Player getInstance()
    {
        return instance;
    }

    IEnumerator DashCoolDown()
    {
        dashCooldown = true;
        yield return new WaitForSeconds(3f);
        dashCooldown = false;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == getLayerNumber("Spiketrap"))
        {
            TakeDamage(0.1f);
        }
    }

}
