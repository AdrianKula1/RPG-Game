using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playerv2 : MonoBehaviour
{
    private Rigidbody2D rigidBody;
    private Vector3 moveDir;
    private float speed;//= 6f;
    private bool isDash = false;
    private bool isSprint = false;
    private bool dashCooldown = false;
    private float stamina = 100f;
    private float health = 100f;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        moveDir = Vector3.zero;
    }

    public void Move(Vector3 direction, bool sprint, bool dash)
    {
        moveDir = direction;

        if (dash && !dashCooldown && stamina >= 30f)
        {
            isDash = true;
        }
        else if (sprint && stamina >= 0)
        {
            speed = 12f;
            isSprint = true;
        }
        else
        {
            speed = 6f;
        }

    }
    private void FixedUpdate()
    {
        rigidBody.velocity = moveDir * speed;

        if (moveDir != Vector3.zero)
        {
            if (isSprint)
            {
                stamina -= 0.6f;
                isSprint = false;
            }


            if (isDash)
            {
                float dashVelocity = 3f;
                Vector3 dashPosition = transform.position + moveDir * dashVelocity;
                RaycastHit2D raycast = Physics2D.Raycast(transform.position, moveDir, dashVelocity, LayerMask.GetMask("Obstacle"));
                if (raycast.collider != null)
                {
                    dashPosition = raycast.point;
                }

                rigidBody.MovePosition(dashPosition);
                stamina -= 30f;
                StartCoroutine(DashCoolDown());
                isDash = false;
            }
        }
    }

    private void Update()
    {
        if (stamina < 100f)
            stamina += 0.01f;
    }

    private IEnumerator DashCoolDown()
    {
        dashCooldown = true;
        yield return new WaitForSeconds(3f);
        dashCooldown = false;
    }

    public void TakeDamage(float damageValue)
    {
        if (!isDash)
            health -= damageValue;

        if (health <= 0)
            Die();
    }

    private void Die()
    {
        Debug.Log("Player died");
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == GameManager.getLayerNumber("Spiketrap"))
            TakeDamage(0.1f);
        
    }
}
