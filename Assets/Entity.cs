using System;
using System.Collections;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Entity : MonoBehaviour
{
    protected Animator animator;
    protected Rigidbody2D rb;
    protected Collider2D collider2D;
    protected SpriteRenderer spriteRenderer;

    [Header("Health")]
    [SerializeField] private int maxHealth = 1;
    [SerializeField] private int currentHealth;
    [SerializeField] private Material damageMaterial;
    [SerializeField] private float damageFeedbackDuration = .1f;
    private Coroutine damageFeedbackCoroutine;

    [Header("Attack details")]
    [SerializeField] protected float attackRadius;
    [SerializeField] protected Transform attackPoint;
    [SerializeField] protected LayerMask whatIsTarget;

    [Header("Collision details")]
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask whatIsGround;
    protected bool isGrounded;

    // Facing direction details
    protected bool canMove = true;
    protected int facingDir = 1;
    protected bool facingright = true;


    protected virtual void Awake()
    {        
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        collider2D = GetComponent<Collider2D>();
        currentHealth = maxHealth;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    protected virtual void Update()
    {
        HandleCollision();
        HandleMovement();
        HandleAnimation();
        HandleFLip();
    }

    public void DamageTargets()
    {
        Collider2D[] enemyColliders = Physics2D.OverlapCircleAll(attackPoint.position, attackRadius, whatIsTarget);
        foreach (var enemy in enemyColliders)
        {
            Entity entityTarget = enemy.GetComponent<Entity>();
            entityTarget.TakeDamage();
        }
    }

    private void TakeDamage()
    {
        currentHealth = currentHealth - 1;

        PlayDamageFeedback();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void PlayDamageFeedback()
    {
        if (damageFeedbackCoroutine != null)
        {
            StopCoroutine(DamageFeedbackCoroutine());
        }            
        StartCoroutine(DamageFeedbackCoroutine());
    }

    private IEnumerator DamageFeedbackCoroutine()
    {
        Material originalMaterial = spriteRenderer.material;
        spriteRenderer.material = damageMaterial;

        yield return new WaitForSeconds(damageFeedbackDuration);

        spriteRenderer.material = originalMaterial;
    }

    protected virtual void Die()
    {
        animator.enabled = false;
        collider2D.enabled = false;
        rb.gravityScale = 12;
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 15);
        Destroy(gameObject,3);
    }

    public virtual void EnableMovement(bool enable)
    {
        canMove = enable;
    }

    protected void HandleAnimation()
    {
        animator.SetBool("isGrounded", isGrounded);
        animator.SetFloat("xVelocity", rb.linearVelocity.x);
        animator.SetFloat("yVelocity", rb.linearVelocity.y);
    }

    protected virtual void HandleAttack()
    {
        if (isGrounded) 
        {
            animator.SetTrigger("attack");
        }
    }

    protected virtual void HandleMovement()
    {
    }

    protected virtual void HandleFLip()
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

    protected void Flip()
    {
        transform.Rotate(0, 180, 0);
        facingright = !facingright;
        facingDir = facingDir = -1;
    }

    protected virtual void HandleCollision()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
    }

    // This is not neccesary. This only draws the collision line
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(0, groundCheckDistance));
        if (attackPoint != null)
        {
            Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
        }  
    }
}
