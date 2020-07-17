using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//EN SECONDES

public class Cooldown
{
    public bool isReady;

    private float currentCD;
    private float maxCD = 0.0f;

    public Cooldown(float max)
    {
        maxCD = max;
        isReady = true;
    }

    public void Update()
    {
        if (!isReady)
        {
            currentCD += (float)(Time.deltaTime % 3600) % 60;
            isReady = currentCD >= maxCD;
            if (isReady)
            {
                currentCD = 0.0f;
            }
        }    
    }

    public void Trigger()
    {
        isReady = false;
        currentCD = 0.0f;
    }
}
