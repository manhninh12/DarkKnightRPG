using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
public class SlimeController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float detectionRadius = 5f;
    [SerializeField] private float attackRadius = 1f;
    [SerializeField] private float attackCooldown = 1f;

    private Transform player;
    private Animator animator;
    private Rigidbody2D rb;

    public Transform attackPoint;
    public LayerMask attackLayer;

    private bool isAttacking = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }

    void Update()
    {
        if (player == null) return;
        if (isAttacking) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= attackRadius)
        {
            Attack();
        }
        else if (distance <= detectionRadius)
        {
            MoveTowardsPlayer();
        }
        else
        {
            StopMoving();
        }
    }

    private void MoveTowardsPlayer()
    {
        animator.SetBool("IsRunning", true);
        
        // Calculate new position
        Vector2 targetPosition = new Vector2(player.position.x, transform.position.y);
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        
        // Flip sprite to face player
        if (player.position.x > transform.position.x)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (player.position.x < transform.position.x)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }

    private void StopMoving()
    {
        animator.SetBool("IsRunning", false);
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
    }

    private void Attack()
    {
        isAttacking = true;
        animator.SetBool("IsRunning", false);
        animator.SetTrigger("Attack");
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);

        if (attackPoint != null)
        {
            Collider2D collInfo = Physics2D.OverlapCircle(attackPoint.position, attackRadius, attackLayer);
            if (collInfo)
            {
                Debug.Log(collInfo.transform.name);
                PlayerController playerController = collInfo.GetComponent<PlayerController>();
                if (playerController != null)
                {
                    playerController.TakeDamage(1);
                }
            }
        }

        // Reset attack state after cooldown
        Invoke(nameof(ResetAttack), attackCooldown);
    }

    private void ResetAttack()
    {
        isAttacking = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isAttacking)
        {
            Attack();
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isAttacking)
        {
            Attack();
        }
    }

    private void OnDrawGizmosSelected()
    { 
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);

        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
    }
}
