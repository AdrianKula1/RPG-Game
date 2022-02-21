using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputClassv2 : MonoBehaviour
{
    private float lastClickedTimeW;
    private float lastClickedTimeS;
    private float lastClickedTimeA;
    private float lastClickedTimeD;
    private Playerv2 player;

    private void Awake()
    {
        player = GetComponent<Playerv2>();
    }

    void Update()
    {
        float moveY = 0f;
        float moveX = 0f;
        bool sprint = false;
        bool dash = false;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            sprint = true;
        }

        if (Input.GetKey(KeyCode.W))
        {
            moveY = 1f;
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
            moveY = -1f;
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
            moveX = -1f;
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
            moveX = 1f;
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            float timeSinceLastClick = Time.time - lastClickedTimeD;
            lastClickedTimeD = Time.time;
            if (timeSinceLastClick <= 0.2f)
                dash = true;
        }

        player.Move(new Vector3(moveX, moveY).normalized, sprint, dash);
    }
}
