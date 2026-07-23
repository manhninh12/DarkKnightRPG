using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    [SerializeField] private int coinValue = 1; // Giá trị của đồng xu này

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Kiểm tra xem object chạm vào có phải là Player không
        if (collision.CompareTag("Player"))
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            if (player != null)
            {
                player.AddCoins(coinValue);
                // Có thể thêm code phát âm thanh hoặc hiệu ứng particle ở đây
                
                // Hủy object đồng xu khỏi cảnh
                Destroy(gameObject);
            }
        }
    }
}
