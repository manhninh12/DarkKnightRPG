using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Image icon; // Thành phần Image trên UI để hiển thị Icon của vật phẩm

    ItemData item; // Dữ liệu của Item đang nằm trong ô này

    // Hàm này được gọi từ InventoryUI khi đưa item vào ô
    public void AddItem(ItemData newItem)
    {
        item = newItem;
        
        icon.sprite = item.icon;
        icon.enabled = true; // Bật icon lên
    }

    // Xóa item khỏi ô (khi dùng xong hoặc vứt đi)
    public void ClearSlot()
    {
        item = null;
        
        icon.sprite = null;
        icon.enabled = false; // Tắt icon đi (chỉ còn khung viền)
    }

    // Hàm này sẽ được gọi khi người chơi Bấm (Click) vào ô đồ trên giao diện
    public void UseItem()
    {
        if (item != null)
        {
            // Kiểm tra nếu là vật phẩm tiêu hao (bình máu)
            if (item is ConsumableItemData consumable)
            {
                PlayerController player = FindAnyObjectByType<PlayerController>();
                if (player != null)
                {
                    consumable.Use(player); // Hồi máu
                    
                    // Xóa item đó khỏi Inventory (do đã uống)
                    Inventory.instance.Remove(item);
                }
            }
        }
    }
}
