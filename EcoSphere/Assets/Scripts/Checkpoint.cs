using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public int number;
    private bool activated;

    public Range range;

    public bool Activated {
        get => activated;
        set
        {
            activated = value;
            if(value)GameManager.Instance.CurrentCheckpoint = number;
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

