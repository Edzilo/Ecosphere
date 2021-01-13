using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//EN SECONDES

public class Cooldown
{
    public bool isReady;

    private float maxCD = 0.0f;

    private MonoBehaviour mono;

    public Cooldown(float max , MonoBehaviour monoBehaviour)
    {
        maxCD = max;
        mono = monoBehaviour;
        isReady = true;
    }

    public void Trigger()
    {
        isReady = false;
        mono.StartCoroutine(UpdateCD());     
    }

    IEnumerator UpdateCD()
    {
        var elapsedTime = 0.0f;
        while (!isReady)
        {
            elapsedTime += Time.deltaTime;
            isReady = elapsedTime >= maxCD;
        }
        yield return 0;
    }
}
