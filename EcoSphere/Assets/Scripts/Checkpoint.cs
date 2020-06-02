using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public int number;
    private bool activated;

    public GameObject model_on;
    public GameObject model_off;

    public Range range;

    private void Start()
    {
        model_off.SetActive(true);
        model_on.SetActive(false);
    }

    public bool Activated {
        get => activated;
        set
        {
            activated = value;
            if (value)
            {
                print("Checkpoint " + number + " activated");
                GameManager.Instance.CurrentCheckpoint = this;
                model_off.SetActive(false);
                model_on.SetActive(true);

            } else
            {
                model_off.SetActive(true);
                model_on.SetActive(false);
            }
        }
    }

    public void EnterInRange(Range range)
    {      
        if (!Activated)
        {
            Activated = true;
        }
    }
}

