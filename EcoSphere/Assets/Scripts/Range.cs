using UnityEngine;

public class Range : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        transform.parent.GetComponent<Checkpoint>().EnterInRange(this);
    }
}
