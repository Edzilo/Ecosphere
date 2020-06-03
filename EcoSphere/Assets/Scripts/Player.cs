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

        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        movement = new Vector3(moveHorizontal, 0, moveVertical);

        if (/*IsGrounded()*/ !offGround && Input.GetKeyDown(KeyCode.Space) )
        {
            rb.AddForce(Vector3.up * CalculateJumpVerticalSpeed(), ForceMode.Impulse);
        }
        
        rb.AddForce(movement * speed);
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
