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

    public int maxHealth = 3;
    public int currentHealth;

    public Transform attackPoint;
    public LayerMask attackLayer;

    [Header("Drop Settings")]
    public GameObject coinPrefab;

    private bool isAttacking = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }

    void Update() {
        if (currentHealth <= 0) {
            Die();
            return;
        }
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

    public void TakeDamage(int damage) {
        currentHealth -= damage;
        if(currentHealth <= 0) {
            Die();
        }
    }

    void Die() {
        Debug.Log(this.transform.name + " Died");
        
        // Rớt coin ngẫu nhiên từ 3 đến 6 đồng
        if (coinPrefab != null)
        {
            int dropCount = Random.Range(3, 7); // Random từ 3 đến 6
            for (int i = 0; i < dropCount; i++)
            {
                // Rải đều coin xung quanh một chút
                Vector3 randomOffset = new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(0f, 0.5f), 0);
                GameObject droppedCoin = Instantiate(coinPrefab, transform.position + randomOffset, Quaternion.identity);
                
                // Thêm lực nảy để tạo cảm giác rớt ra (nếu coin có Rigidbody2D)
                Rigidbody2D coinRb = droppedCoin.GetComponent<Rigidbody2D>();
                if (coinRb != null)
                {
                    coinRb.AddForce(new Vector2(Random.Range(-2f, 2f), Random.Range(3f, 5f)), ForceMode2D.Impulse);
                }
            }
        }

        // Kích hoạt animation Die
        animator.SetTrigger("Die");

        // Dừng di chuyển và vô hiệu hóa trọng lực để không bị rơi xuyên qua đất
        rb.linearVelocity = Vector2.zero;
        rb.gravityScale = 0f;

        // Tắt va chạm để không bị tấn công hay cản đường nữa
        Collider2D coll = GetComponent<Collider2D>();
        if (coll != null) {
            coll.enabled = false;
        }

        // Tắt script này để Slime không tiếp tục chạy các hàm Update
        this.enabled = false;

        // Phá hủy GameObject sau 5.5 giây (5 giây giữ nguyên frame cuối + khoảng 0.5s để chạy animation)
        Destroy(this.gameObject, 5.5f);
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
        


        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
    }
}
