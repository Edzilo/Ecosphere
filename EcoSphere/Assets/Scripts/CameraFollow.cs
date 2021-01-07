using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 0.125f;
    public Vector3 offset;
    private Vector3 velocity = Vector3.zero;

    //TRANSITIONS
    public float offsetTransitionTime = 2.0f;
    public float rotationTransitionTime = 2.0f;

    private void LateUpdate()
    {
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothSpeed);
        transform.position = smoothedPosition;
    }

    public IEnumerator ChangeOffset(Vector3 targetOffset)
    {
        Vector3 initialOffset = offset;
        float elapsedTime = 0.0f;
        float maxTime = offsetTransitionTime;

        while (offset != targetOffset)
        {
            elapsedTime += Time.deltaTime;
            offset = Vector3.Lerp(initialOffset, targetOffset, elapsedTime / maxTime);
            yield return new WaitForEndOfFrame();
        }

        yield return 0;
    }

    public IEnumerator ChangeRotation(Quaternion targetRotation)
    {
        Quaternion initialRotation = Camera.main.transform.rotation;
        float elapsedTime = 0.0f;
        float maxTime = offsetTransitionTime;

        while (initialRotation != targetRotation)
        {
            elapsedTime += Time.deltaTime;
            Camera.main.transform.rotation = Quaternion.Slerp(initialRotation, targetRotation, elapsedTime / maxTime);
            yield return new WaitForEndOfFrame();
        }

        yield return 0;
    }
}
