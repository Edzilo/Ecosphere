using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;

    public float drag; //[0.0 , 1.0];

    public float jumpHeight;

    public float jumpCD; // en secondes

    public SphereCollider col;

    private bool jumpReloading = false;

    private Vector3 savedVelocity;

    private float currentJumpCD; // en secondes

    //private bool jumpStickDownLast = false;

    private float offGroundTime;

    public LayerMask groundLayer;

    public Rigidbody rb;

    private bool offGround;
    public bool OffGround { get => offGround; set => offGround = value; }


    private Vector3 lastGroundPosition;

    public Vector3 FallBackPosition { get; set; }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<SphereCollider>();
        transform.position = GameManager.Instance.currentCheckpoint.transform.position + new Vector3(0, 0.5f, -1.0f);
        FallBackPosition = transform.position;
        offGroundTime = 0.0f;
    }

    void Update()
    {
        if (Input.GetAxis("Restart") != 0 && !GameManager.Instance.IsPaused())
        {
            FallBack();
        }

        RaycastHit hit;
        if (jumpReloading)
        {
            currentJumpCD += (float)(Time.deltaTime % 3600) % 60;
            jumpReloading = (currentJumpCD < jumpCD);
            if (!jumpReloading)
            {
                currentJumpCD = 0.0f;
            }
        }

        if (OffGround)
        {
            Vector3 vel = rb.velocity;
            vel.x *= drag;
            vel.z *= drag;
            rb.velocity = vel;

            offGroundTime += Time.deltaTime;
            if(offGroundTime >= 2.0f 
                && Physics.Raycast(transform.position, -Camera.main.transform.TransformDirection(Vector3.up), out hit)
                && hit.distance >= 2.0f && hit.collider.GetComponent<Renderer>() != null 
                && !GameManager.Instance.jumpable.Contains(hit.collider.GetComponent<Renderer>().sharedMaterial))
            {
                FallBack();
            } else if (IsAboveJumpable())
            {
                offGroundTime = 0.0f;
            }
        } else
        {
            offGroundTime = 0.0f;
        }
    }

    void FixedUpdate()
    {
        if (!GameManager.Instance.IsPaused())
        {

            MoveCharacter();
        } 
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

        if (Input.GetAxis("Jump") != 0)
        {
            if (!OffGround  && !jumpReloading && GameManager.Instance.LeavingPause() /*!GameManager.Instance.submitStickDownLast && !jumpStickDownLast*/)
            {

                //jumpStickDownLast = true;
                jumpReloading = true;
                jump = Vector3.up;
                jump = Camera.main.transform.TransformDirection(jump);
                jump.x = 0;
                jump.z = 0;
                Vector3 jumpForce = jump.normalized * jumpHeight;
                rb.AddForce(jumpForce, ForceMode.Impulse);
            }
            //jumpStickDownLast = true;
        }
        else
        {
            //jumpStickDownLast = false;
        }
       
        movement = Camera.main.transform.TransformDirection(movement);
        movement.y = 0;
        Vector3 movementForce = movement.normalized * speed;
        rb.AddForce(movementForce);
    }

    private void OnCollisionStay(Collision hit)
    {
        if ( GameManager.Instance.jumpable.Contains(hit.collider.GetComponent<Renderer>().sharedMaterial))
        {
            rb.drag = 0.5f;
            OffGround = false;
        }
        if (hit.collider.GetComponent<Renderer>().sharedMaterial.name == "background")
        {
            FallBack();
        }
    }

    void OnCollisionExit(Collision hit)
    {
        if (hit.gameObject.layer == 0)
        {
            OffGround = true;
        }
    }

    void FallBack()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        lastGroundPosition = FallBackPosition;
        transform.position = FallBackPosition;
    }

    public bool IsAboveJumpable()
    {
        RaycastHit hit;
        return Physics.Raycast(transform.position, -Camera.main.transform.TransformDirection(Vector3.up), out hit)
                && hit.collider.GetComponent<Renderer>() != null
                && GameManager.Instance.jumpable.Contains(hit.collider.GetComponent<Renderer>().sharedMaterial);
    }

    public void SaveVelocity()
    {
        savedVelocity = rb.velocity;
    }

    public void RecoverVelocity()
    {
        rb.velocity = savedVelocity;
    }
}
