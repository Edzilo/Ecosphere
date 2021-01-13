using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;

    public float drag; //[0.0 , 1.0];

    public float jumpHeight;

    private SphereCollider col;

    private Vector3 savedVelocity;
    
    private Cooldown CollisionSoundCD; // en secondes

    public float collisionSoundMaxCD; // en secondes
    
    public float jumpMaxCD; // en secondes

    private Cooldown jumpCD;

    private bool jumpStickDownLast = false;

    private float offGroundTime;

    private bool isFallingBack;

    public LayerMask groundLayer;

    public Rigidbody rb;

    private bool offGround;
    public bool OffGround { get => offGround; set => offGround = value; }

    private Vector3 lastGroundPosition;

    public Vector3 FallBackPosition { get; set; }

    public AudioSource dirtImpact;

    public AudioSource rockImpact;

    public AudioSource waterImpact;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<SphereCollider>();
        transform.position = GameManager.Instance.currentCheckpoint.transform.position + new Vector3(0, 0.5f, -1.0f);
        FallBackPosition = transform.position;
        offGroundTime = 0.0f;

        jumpCD = new Cooldown(jumpMaxCD);
        CollisionSoundCD = new Cooldown(collisionSoundMaxCD);

    }

    void Update()
    {
        if (Input.GetAxis("Restart") != 0 && !GameManager.Instance.IsPaused())
        {
            FallBack();
        }

        jumpCD.Update();
        CollisionSoundCD.Update();


        if (OffGround)
        {
            Vector3 vel = rb.velocity;
            vel.x *= drag;
            vel.z *= drag;
            rb.velocity = vel;

            if (!GameManager.Instance.IsPaused())
            {
                offGroundTime += Time.deltaTime;
            }
            if (offGroundTime >= 2.0f
                && Physics.Raycast(transform.position, -Camera.main.transform.TransformDirection(Vector3.up), out RaycastHit hit)
                && hit.distance >= 2.0f && hit.collider.GetComponent<Renderer>() != null
                && !GameManager.Instance.jumpable.Contains(hit.collider.GetComponent<Renderer>().sharedMaterial))
            {
                FallBack();
            }
            else if (IsAboveJumpable())
            {
                offGroundTime = 0.0f;
            }
        }
        else
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
            if (!OffGround && !jumpStickDownLast && jumpCD.isReady)
            {

                jumpStickDownLast = true;
                jumpCD.Trigger();
                jump = Vector3.up;
                jump = Camera.main.transform.TransformDirection(jump);
                jump.x = 0;
                jump.z = 0;
                Vector3 jumpForce = jump.normalized * jumpHeight;
                rb.AddForce(jumpForce, ForceMode.Impulse);
            }
            jumpStickDownLast = true;
        }
        else
        {
            jumpStickDownLast = false;
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
        if (hit.collider.GetComponent<Renderer>().sharedMaterial.name == "background" ||
            hit.collider.GetComponent<Renderer>().sharedMaterial.name == "Water")
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

    private void OnCollisionEnter(Collision hit)
    {
        if (CollisionSoundCD.isReady)
        {
            CollisionSoundCD.Trigger();
            //HITTING DIRT
            if (GameManager.Instance.jumpable.Contains(hit.collider.GetComponent<Renderer>().sharedMaterial)
                 && (hit.relativeVelocity.magnitude > (0.50 * speed) ))
            {
                dirtImpact.volume = Mathf.Clamp((hit.relativeVelocity.magnitude / speed), 0.0f, 1.0f);
                dirtImpact.Play();
            }

            //HITTING ROCK
            if (GameManager.Instance.rocks.Contains(hit.collider.GetComponent<Renderer>().sharedMaterial)
                 && (hit.relativeVelocity.magnitude > 10))
            {
                rockImpact.volume = Mathf.Clamp((hit.relativeVelocity.magnitude / speed), 0.0f, 1.0f);
                rockImpact.Play();
            }
        }
    }

    public void FallBack(bool wait = false)
    {
        if(!isFallingBack)
            StartCoroutine(FallBAckCoroutine(wait));
    }

    /* TEST */

    IEnumerator FallBAckCoroutine(bool wait)
    {
        isFallingBack = true;
        if (wait)
            yield return new WaitForSeconds(0.25f);
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.drag = 0.5f;
        lastGroundPosition = FallBackPosition;
        transform.position = FallBackPosition;
        isFallingBack = false;


    }

    public bool IsAboveJumpable()
    {
        return Physics.Raycast(transform.position, -Camera.main.transform.TransformDirection(Vector3.up), out RaycastHit hit)
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
