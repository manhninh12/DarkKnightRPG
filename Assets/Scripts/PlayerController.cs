using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;
    private Animator animator;
    private bool isGrounded;
    private Rigidbody2D rb;
    private float lastAttackTime;
    public int maxHealth = 5;
    public int currentHealth;
    public Text health;
    [SerializeField] private float comboResetTime = 0.5f;
    [SerializeField] private float attackCooldown = 0.25f; // Thời gian của mỗi đòn đánh
    private int comboStep = 0;

    public Transform attackPoint;
    public float attackRadius = 1f;
    public LayerMask attackLayer; 

    [Header("Dash Settings")]
    [SerializeField] private float dashSpeed = 15f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashCooldown = 1f;
    private bool isDashing;
    private float lastDashTime = -Mathf.Infinity;

    private void Awake()
    {
        animator= GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {   
        if (currentHealth <= 0)
        {
            Die();
            return;
        }
        health.text = currentHealth.ToString();
        
        if (isDashing) return;

        HandleMovement();
        HandleJump();
        HandleAttack();
        HandleDash();
        UpdateAnimation();
    }
    private void HandleMovement()
    {
        float moveInput = 0f;
        if (Keyboard.current != null)
        {
            if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) moveInput += 1f;
            if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) moveInput -= 1f;
        }

        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
        if (moveInput > 0) transform.localScale = new Vector3(1, 1, 1);
        else if (moveInput < 0) transform.localScale = new Vector3(-1, 1, 1);
    }
    private void HandleJump()
    {
        if (groundCheck != null)
        {
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        }

        if (isGrounded && Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    private void HandleAttack()
    {
        // Reset combo if time has passed
        if (Time.time - lastAttackTime > comboResetTime)
        {
            comboStep = 0;
        }

        if (Keyboard.current != null && Keyboard.current.jKey.wasPressedThisFrame) // Default J key for attack
        {
            // Ngăn chặn bấm nút liên tục trước khi đòn đánh kết thúc (0.25s)
            if (comboStep > 0 && Time.time - lastAttackTime < attackCooldown) 
                return;

            lastAttackTime = Time.time;
            comboStep++;

            if (comboStep == 1)
            {
                animator.ResetTrigger("Attack2");
                animator.SetTrigger("Attack1");
            }
            else if (comboStep == 2)
            {
                animator.ResetTrigger("Attack1");
                animator.SetTrigger("Attack2");
                comboStep = 0; // Reset after max combo
            }
        }
    }

    private void HandleDash()
    {
        if (Keyboard.current != null && Keyboard.current.leftShiftKey.wasPressedThisFrame) 
        {
            if (Time.time >= lastDashTime + dashCooldown && !isDashing)
            {
                StartCoroutine(Dash());
            }
        }
    }

    private IEnumerator Dash()
    {
        isDashing = true;
        animator.SetTrigger("PlayerDash"); // Gọi animation PlayerDash
        
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f; // Bỏ qua trọng lực khi đang lướt
        
        float dashDirection = transform.localScale.x;
        rb.linearVelocity = new Vector2(dashDirection * dashSpeed, 0f);

        yield return new WaitForSeconds(dashDuration);

        rb.gravityScale = originalGravity;
        isDashing = false;
        lastDashTime = Time.time;
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }

        if (attackPoint != null)
        {
            Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
        }
    }
    private void UpdateAnimation()
    {
        bool isRunning = Mathf.Abs(rb.linearVelocity.x) > 0.1f;
        bool isJumping = !isGrounded;
        animator.SetBool("isRunning", isRunning);
        animator.SetBool("isJumping", isJumping);
    }

    public void Attack()
    { 
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRadius,attackLayer);
        foreach(Collider2D enemy in hitEnemies)
        {
            if (enemy.gameObject.GetComponent<SlimeController>() != null) {
                Debug.Log("Hit enemy");
                enemy.gameObject.GetComponent<SlimeController>().TakeDamage(1); 
            }
        }
    }


    
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
            return;
        }
    }
    void Die()
    {         // Xử lý khi nhân vật chết (ví dụ: phát animation, load lại scene, v.v.)
        Debug.Log("Player has died.");
    }
}
