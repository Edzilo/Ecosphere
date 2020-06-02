﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;

    //public float maxSpeed;

    public float jumpHeight;

    public LayerMask groundLayer;

    public SphereCollider col;

    private Rigidbody rb;

    private Vector3 fallBackPosition;
   

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<SphereCollider>();
        fallBackPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //ReadInputs();
        if (isFalling() )
        {
            transform.position = fallBackPosition;
        }
    }

    void FixedUpdate()
    {
        MoveCharacter();
        if (rb.velocity.magnitude > speed)
        {
            rb.velocity = rb.velocity.normalized * speed;
        }
    }

    void MoveCharacter()
    {
        Vector3 movement = Vector3.zero;

        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        movement = new Vector3(moveHorizontal, 0, moveVertical);
        if (IsGrounded() && Input.GetKeyDown(KeyCode.Space) )
        {
            rb.AddForce(Vector3.up * CalculateJumpVerticalSpeed(), ForceMode.Impulse);
        }
        
        rb.AddForce(movement * speed);
    }

    private bool IsGrounded()
    {
        return Physics.CheckCapsule(col.bounds.center, new Vector3(col.bounds.center.x, col.bounds.min.y,
            col.bounds.center.z), col.radius * .9f, groundLayer);
    }

    private float CalculateJumpVerticalSpeed()
    {
        // From the jump height and gravity we deduce the upwards speed 
        // for the character to reach at the apex.
        return Mathf.Sqrt(2.0f * jumpHeight * (-1.0f * Physics.gravity.y) );
    }

    private bool isFalling()
    {
        return transform.position.y <= -1.0f;
    }
}
