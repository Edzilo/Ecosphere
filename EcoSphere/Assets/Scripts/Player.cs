using System.Collections;
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

    private bool offGround;

    public Vector3 FallBackPosition { get => fallBackPosition; set => fallBackPosition = value; }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<SphereCollider>();
        transform.position = GameManager.Instance.currentCheckpoint.transform.position + new Vector3(0, 0.5f, -1.0f);
        FallBackPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (isFalling() )
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            transform.position = FallBackPosition;
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



            //rb.AddForce(Vector3.up * CalculateJumpVerticalSpeed(), ForceMode.Impulse);
        }

        movement = Camera.main.transform.TransformDirection(movement);
        movement.y = 0;
        Vector3 force = movement.normalized * speed;
        rb.AddForce(force);



    }

    private bool IsGrounded()
    {
        return Physics.CheckCapsule(col.bounds.center, new Vector3(col.bounds.center.x, col.bounds.min.y,
            col.bounds.center.z), col.radius * 1.0f, groundLayer);
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

    // Detect collision with floor
    void OnCollisionEnter(Collision hit)
    {
        if (hit.gameObject.layer == 0)
        {
            offGround = false;
        }
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
