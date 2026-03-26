using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;

    [Header("Movement details")]
    private float xinput;
    private bool facingright = true;
    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private float jumpForce = 8;

    [Header("Collision details")]
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask whatIsGround;
    private bool isGrounded;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        HandleCollision();
        HandleInput();
        HandleMovement();
        HandleAnimation();
        HandleFLip();
    }

    private void HandleAnimation()
    {
        bool isMoving = rb.linearVelocity.x != 0;
        animator.SetBool("isMoving", isMoving);
    }

    private void HandleInput()
    {
        xinput = Input.GetAxisRaw("Horizontal");
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
    }

    private void HandleMovement()
    {
        rb.linearVelocity = new Vector2(xinput * moveSpeed, rb.linearVelocityY);
    }

    private void Jump()
    {
        if (isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    private void HandleFLip()
    {
        if (rb.linearVelocity.x > 0 && !facingright)
        {
            Flip();
        }
        else if (rb.linearVelocity.x < 0 && facingright)
        {
            Flip();
        }
    }

    private void Flip()
    {
        transform.Rotate(0, 180, 0);
        facingright = !facingright;
    }

    private void HandleCollision()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
    }

    // This is not neccesary. This only draws the collision line
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(0, groundCheckDistance));
    }
}
