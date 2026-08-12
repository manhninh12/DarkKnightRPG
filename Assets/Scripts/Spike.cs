using UnityEngine;

public class Spike : MonoBehaviour
{
    [Header("Spike Settings")]
    [SerializeField] private int damageAmount = 2;
    [SerializeField] private float damageInterval = 1f; // Trừ máu mỗi 1 giây

    private float timer = 0f;
    private PlayerController playerInRange;

    private void Update()
    {
        // Nếu Player đang dẫm lên gai
        if (playerInRange != null)
        {
            timer += Time.deltaTime;
            
            // Cứ sau mỗi `damageInterval` (1 giây), trừ máu tiếp
            if (timer >= damageInterval)
            {
                playerInRange.TakeDamage(damageAmount);
                timer = 0f;
            }
        }
    }

    // --- Trường hợp dùng Collider bình thường (Solid) ---
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            if (player != null)
            {
                playerInRange = player;
                playerInRange.TakeDamage(damageAmount); // Trừ máu ngay lần chạm đầu tiên
                timer = 0f;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerInRange = null;
            timer = 0f;
        }
    }

    // --- Trường hợp dùng Collider dạng Trigger (Is Trigger được tích) ---
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            if (player != null)
            {
                playerInRange = player;
                playerInRange.TakeDamage(damageAmount); // Trừ máu ngay lần chạm đầu tiên
                timer = 0f;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerInRange = null;
            timer = 0f;
        }
    }
}
