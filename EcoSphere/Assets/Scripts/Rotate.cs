using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{

    private float x;
    public float rotationSpeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /*x += Time.deltaTime * rotationSpeed;
        if (x > 360.0f)
        {
            x = 0.0f;
        }*/
        transform.RotateAround(transform.position, transform.up, Time.deltaTime * rotationSpeed);
        //transform.localRotation = Quaternion.Euler(0, x, 0);

    }
}
