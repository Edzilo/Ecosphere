﻿using UnityEngine;

public class Range : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<Player>() != null && !other.GetComponent<Player>().OffGround)
        {
            transform.parent.GetComponent<Checkpoint>().EnterInRange(this);
        }
    }
}
