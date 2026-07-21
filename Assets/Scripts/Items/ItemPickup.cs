using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [Header("Item Configuration")]
    public ItemData itemData; // Kéo thả ScriptableObject (ví dụ: Health Potion) vào đây

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Kiểm tra xem đối tượng chạm vào có phải là Player không
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();

            if (player != null && itemData != null)
            {
                // Thêm item vào Inventory
                bool wasPickedUp = Inventory.instance.Add(itemData);

                if (wasPickedUp)
                {
                    Debug.Log("Đã nhặt: " + itemData.itemName);
                    Destroy(gameObject); // Phá hủy đồ rơi nếu đã nhặt thành công
                }
            }
        }
    }
}
