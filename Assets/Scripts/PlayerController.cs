using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

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
    [SerializeField] private float comboResetTime = 0.5f;
    [SerializeField] private float attackCooldown = 0.25f; // Thời gian của mỗi đòn đánh
    private int comboStep = 0;

    private void Awake()
    {
        animator= GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
        HandleJump();
        HandleAttack();
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

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
    private void UpdateAnimation()
    {
        bool isRunning = Mathf.Abs(rb.linearVelocity.x) > 0.1f;
        bool isJumping = !isGrounded;
        animator.SetBool("isRunning", isRunning);
        animator.SetBool("isJumping", isJumping);
    }
}
