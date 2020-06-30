using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;

    public float drag; //[0.0 , 1.0];

    public float jumpHeight;

    public float jumpCD; // en secondes

    private bool jumpReloading = false;

    private float currentJumpCD; // en secondes

    private float offGroundTime;

    public LayerMask groundLayer;

    public SphereCollider col;

    private Rigidbody rb;

    private bool offGround;

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
        if (Input.GetAxis("Restart") != 0)
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
        if (offGround)
        {
            //TODO remplacer par un drag 'maison" pour éviter un ralentissement en y (vers le bas)
            Vector3 vel = rb.velocity;
            vel.x *= drag;
            vel.z *= drag;
            rb.velocity = vel;
            //rb.drag = 1.5f;
            offGroundTime += Time.deltaTime;
            if(offGroundTime >= 2.0f 
                && Physics.Raycast(transform.position, -Camera.main.transform.TransformDirection(Vector3.up), out hit)
                && hit.distance >= 2.0f && hit.collider.GetComponent<Renderer>() != null 
                && !GameManager.Instance.jumpable.Contains(hit.collider.GetComponent<Renderer>().sharedMaterial))
            {
                FallBack();
            } else if (Physics.Raycast(transform.position, -Camera.main.transform.TransformDirection(Vector3.up), out hit) 
                && hit.collider.GetComponent<Renderer>() != null
                && GameManager.Instance.jumpable.Contains(hit.collider.GetComponent<Renderer>().sharedMaterial))
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

        if (!offGround && Input.GetAxis("Jump") != 0 && !jumpReloading)
        {
            jumpReloading = true;
            jump = Vector3.up;
            jump = Camera.main.transform.TransformDirection(jump);
            jump.x = 0;
            jump.z = 0;
            Vector3 jumpForce = jump.normalized * jumpHeight;
            rb.AddForce(jumpForce, ForceMode.Impulse);
            //rb.drag = 12.5f;
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
            offGround = false;
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
            offGround = true;
        }
    }

    void FallBack()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        lastGroundPosition = FallBackPosition;
        transform.position = FallBackPosition;
    }
}
