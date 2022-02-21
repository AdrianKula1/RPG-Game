using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cooldown
{
    private float coolDownTime;
    private float counter;
    private bool ready;
    public Cooldown(float time)
    {
        coolDownTime = time;
        counter = 0;
        ready = true;
    }

    void Update()
    {
        if (counter < coolDownTime && !ready)
        {
            counter += 0.1f;
        }
        else
        {
            ready = true;
            counter = 0f;
        }
    }

    private void Reset()
    {
        while (counter < coolDownTime)
            counter++;

        if (counter >= coolDownTime)
        {
            counter = 0f;
            ready = true;
        }

    }

    public void Use()
    {
        ready = false;
    }

    public bool isReady()
    {
        return ready;
    }
}
