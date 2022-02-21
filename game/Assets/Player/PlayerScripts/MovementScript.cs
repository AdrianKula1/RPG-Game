using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementScript : MonoBehaviour
{
    //Transform playerTransform;
    private float speed = 4f;
    private float stamina = 100f;
    private bool dashCooldown = false;

    private float lastClickedTimeW;
    private float lastClickedTimeS;
    private float lastClickedTimeA;
    private float lastClickedTimeD;

    //private void Start()
    //{
        
    //}

    void Update()
    {

        bool sprint = false;
        bool dash = false;
        Vector3 direction = Vector3.zero;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            sprint = true;
        }

        if (Input.GetKey(KeyCode.W))
        {
            direction += Vector3.up;
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            float timeSinceLastClick = Time.time - lastClickedTimeW;
            lastClickedTimeW = Time.time;
            if (timeSinceLastClick <= 0.2f)
                dash = true;
        }

        if (Input.GetKey(KeyCode.S))
        {
            direction += Vector3.down;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            float timeSinceLastClick = Time.time - lastClickedTimeS;
            lastClickedTimeS = Time.time;
            if (timeSinceLastClick <= 0.2f)
                dash = true;
        }

        if (Input.GetKey(KeyCode.A))
        {
            direction += Vector3.left;
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            float timeSinceLastClick = Time.time - lastClickedTimeA;
            lastClickedTimeA = Time.time;
            if (timeSinceLastClick <= 0.2f)
                dash = true;
        }

        if (Input.GetKey(KeyCode.D))
        {
            direction += Vector3.right;
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            float timeSinceLastClick = Time.time - lastClickedTimeD;
            lastClickedTimeD = Time.time;
            if (timeSinceLastClick <= 0.2f)
                dash = true;
        }

        if (direction != Vector3.zero)
            Move(direction, sprint, dash);

        if (stamina < 100f)
            stamina += 0.01f;
        else
            stamina = 100f;
    }

    public void Move(Vector3 direction, bool sprint, bool dash)
    {
        float finalSpeed = speed;

        if (sprint && stamina > 0f)
        {
            finalSpeed = speed * 2;
        }

        if (dash && !dashCooldown && stamina > 30f)
        {
            finalSpeed = 600f;
        }

        if (TryMove(direction, finalSpeed * Time.deltaTime))
        {
            if (sprint)
            {
                stamina -= 0.05f;
            }

            if (dash)
            {
                stamina -= 30f;
                StartCoroutine(DashCoolDown());
            }
        }
    }

    private bool CanMove(Vector3 direction, float distance)
    {
        return Physics2D.Raycast(Player.getInstance().transform.position, direction, distance, Player.getLayerNumber("Obstacle")).collider == null;
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
            Player.getInstance().transform.position += moveDirection * distance;
            return true;
        }
        else
        {
            return false;
        }
    }

    IEnumerator DashCoolDown()
    {
        dashCooldown = true;
        yield return new WaitForSeconds(3f);
        dashCooldown = false;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == Player.getLayerNumber("Spiketrap"))
        {
            Player.getInstance().TakeDamage(0.1f);
        }
    }
}
