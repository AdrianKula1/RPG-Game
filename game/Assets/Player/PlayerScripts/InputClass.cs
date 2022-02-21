using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputClass : MonoBehaviour
{
    private float lastClickedTimeW;
    private float lastClickedTimeS;
    private float lastClickedTimeA;
    private float lastClickedTimeD;
    void Update()
    {
        Player playerInstance = Player.getInstance();
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
        
        if(Input.GetKey(KeyCode.S))
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
            playerInstance.Move(direction, sprint, dash);
    }
}
