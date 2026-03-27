using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;

    [Header("Attack details")]
    [SerializeField] private float attackRadius;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private LayerMask whatIsEnemy;

    [Header("Movement details")]
    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private float jumpForce = 8;
    private float xinput;
    private bool facingright = true;
    private bool canMove = true;
    private bool canJump = true;

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

    public void DamageEnemies()
    {
        Collider2D[] enemyColliders = Physics2D.OverlapCircleAll(attackPoint.position, attackRadius, whatIsEnemy);
        foreach (var enemy in enemyColliders)
        {
            enemy.GetComponent<Enemy>().TakeDamage();
        }
    }
    public void EnableJumpAndMovement(bool enable)
    {
        canJump = enable;
        canMove = enable;
    }

    private void HandleAnimation()
    {
        animator.SetBool("isGrounded", isGrounded);
        animator.SetFloat("xVelocity", rb.linearVelocity.x);
        animator.SetFloat("yVelocity", rb.linearVelocity.y);
    }

    private void HandleInput()
    {
        xinput = Input.GetAxisRaw("Horizontal");
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            TryToAttack();
        }
    }

    private void TryToAttack()
    {
        if (isGrounded) 
        {
            animator.SetTrigger("attack");
        }
    }

    private void HandleMovement()
    {
        if (canMove)
        {
            rb.linearVelocity = new Vector2(xinput * moveSpeed, rb.linearVelocityY);
        } else 
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        }
    }

    private void Jump()
    {
        if (isGrounded && canJump)
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
        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
    }
}
