﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;

    //public float maxSpeed;

    public float jumpHeight;

    private float offGroundTime;

    public LayerMask groundLayer;

    public SphereCollider col;

    private Rigidbody rb;
    private bool offGround;

    private Vector3 lastGroundPosition;

    public Vector3 FallBackPosition { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<SphereCollider>();
        transform.position = GameManager.Instance.currentCheckpoint.transform.position + new Vector3(0, 0.5f, -1.0f);
        FallBackPosition = transform.position;
        offGroundTime = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;

        if (offGround)
        {
            offGroundTime += Time.deltaTime;
            if(offGroundTime >= 1.0f && Physics.Raycast(transform.position, -Camera.main.transform.TransformDirection(Vector3.up), out hit)
                && !GameManager.Instance.jumpable.Contains(hit.collider.GetComponent<Renderer>().sharedMaterial))
            {
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                lastGroundPosition = FallBackPosition;
                transform.position = FallBackPosition;

            } else if (Physics.Raycast(transform.position, -Camera.main.transform.TransformDirection(Vector3.up), out hit) && hit.collider.GetComponent<Renderer>() != null
                && GameManager.Instance.jumpable.Contains(hit.collider.GetComponent<Renderer>().sharedMaterial))
            {
                offGroundTime = 0.0f;
            }
        } else
        {
            offGroundTime = 0.0f;
        }
        print("offground time " + offGroundTime);

        /*if (Physics.Raycast(transform.position, -Camera.main.transform.TransformDirection(Vector3.up), out hit))
        {

            print("Found an object - distance: " + (hit.distance - GetComponent<SphereCollider>().radius) + " of type " + hit.collider.GetComponent<Renderer>().sharedMaterial.name);
            
        }*/

        /*float fallDistance = 0.0f;
        if (offGround)
        {
            fallDistance = lastGroundPosition.y - transform.position.y;
        } else
        {
            lastGroundPosition = transform.position;
        }
        //print("fallDistance " + fallDistance);
        if (fallDistance >= 2.0f)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            lastGroundPosition = FallBackPosition;
            transform.position = FallBackPosition;
        }*/
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
        Vector3 jump = Vector3.zero;

        movement.x = Input.GetAxis("Horizontal");
        movement.z = Input.GetAxis("Vertical");
        movement.y = 0;
        
        if ( !offGround && Input.GetAxis("Jump") != 0 )//Input.GetKeyDown(KeyCode.Space) )
        {
            jump = Vector3.up;
            jump = Camera.main.transform.TransformDirection(jump);
            jump.x = 0;
            jump.z = 0;
            Vector3 jumpForce = jump.normalized * jumpHeight;//CalculateJumpVerticalSpeed();
            rb.AddForce(jumpForce, ForceMode.Impulse);
            rb.drag = 1.0f;
        }

        movement = Camera.main.transform.TransformDirection(movement);
        movement.y = 0;
        Vector3 movementForce = movement.normalized * speed;
        rb.AddForce(movementForce);
    }

    private bool isFalling()
    {
        return transform.position.y <= -2.0f;
    }

    /*// Detect collision with floor
    void OnCollisionEnter(Collision hit)
    {
        if (hit.gameObject.layer == 0 && hit.collider.GetComponent<Renderer>().sharedMaterial.name == "Ground")
        {
            print("The material I'm touching is " + hit.collider.GetComponent<Renderer>().sharedMaterial.name);
            rb.drag = 0.5f;
            offGround = false;
        }
    }*/

    private void OnCollisionStay(Collision hit)
    {
        //print("The material I'm touching is " + hit.collider.GetComponent<Renderer>().sharedMaterial.name);

        if ( GameManager.Instance.jumpable.Contains(hit.collider.GetComponent<Renderer>().sharedMaterial)) /*hit.collider.GetComponent<Renderer>().sharedMaterial.name != "Falaise"*/
        {
            rb.drag = 0.5f;
            offGround = false;
        }


        /*if (hit.gameObject.layer == 0 && hit.collider.GetComponent<Renderer>().sharedMaterial.name == "Ground")
        {
            print("The material I'm touching is " + hit.collider.GetComponent<Renderer>().sharedMaterial.name);
            rb.drag = 0.5f;
            offGround = false;
        } else
        {
            print("Else The material I'm touching is " + hit.collider.GetComponent<Renderer>().sharedMaterial.name);
        }*/
    }

    // Detect collision exit with floor
    void OnCollisionExit(Collision hit)
    {
        if (hit.gameObject.layer == 0)
        {
            offGround = true;
        }
    }
}
