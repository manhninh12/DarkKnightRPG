using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    [SerializeField] private int coinValue = 1; // Giá trị của đồng xu này
    [SerializeField] private float lifetime = 30f; // Thời gian tồn tại trước khi biến mất
    [SerializeField] private float pickupDelay = 1f; // Độ trễ trước khi cho phép nhặt
    [SerializeField] private float pickupRadius = 0.6f; // Vùng quét để nhặt tiền (rộng hơn hình ảnh một chút)

    private float spawnTime;

    private void Start()
    {
        spawnTime = Time.time;
        // Tự động hủy object này sau khoảng thời gian lifetime
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        // Quét tìm Player thay vì dựa vào event Va chạm của Unity (Vì Layer Matrix đã tắt)
        if (Time.time >= spawnTime + pickupDelay)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, pickupRadius);
            foreach (Collider2D coll in colliders)
            {
                if (coll.CompareTag("Player"))
                {
                    Collect(coll.GetComponent<PlayerController>());
                    break;
                }
            }
        }
    }

    private void Collect(PlayerController player)
    {
        if (player != null)
        {
            player.AddCoins(coinValue);
            Destroy(gameObject);
        }
    }

    // Vẽ vòng tròn trong scene để dễ căn chỉnh bán kính nhặt
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, pickupRadius);
    }
}
