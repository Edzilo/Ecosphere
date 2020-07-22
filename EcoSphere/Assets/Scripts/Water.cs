using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>() != null)
        {
            other.GetComponent<Player>().waterImpact.Play();
            other.attachedRigidbody.drag = 3.0f;
            other.GetComponent<Player>().FallBack(true);

            //other.GetComponent<Player>().waterImpact.volume = Mathf.Clamp((other.relativeVelocity.magnitude / speed), 0.0f, 1.0f);
            
        }
    }
}
