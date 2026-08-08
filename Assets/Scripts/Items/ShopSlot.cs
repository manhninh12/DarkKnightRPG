using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopSlot : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI priceText; // Bắt buộc phải là TextMeshPro như thiết kế
    public TextMeshProUGUI quantityText; // Số lượng (ví dụ x2, x4 ở góc dưới)
    
    private ItemData item;
    private bool isPlayerInventory; // true: nằm ở cột trái (để Bán), false: cột phải (để Mua)

    // Hàm gọi khi vẽ UI
    public void Setup(ItemData newItem, int quantity, bool isPlayerSlot)
    {
        item = newItem;
        isPlayerInventory = isPlayerSlot;
        
        if (item == null) return; // Tránh lỗi nếu nhét Item rỗng vào cửa hàng
        
        if (icon != null)
        {
            icon.sprite = item.icon;
            icon.enabled = true;
        }

        // Nếu số lượng > 1 thì hiện số, nếu = 1 thì ẩn chữ đi cho đẹp
        if (quantityText != null)
        {
            if (quantity > 1)
            {
                quantityText.text = quantity.ToString();
                quantityText.gameObject.SetActive(true);
            }
            else
            {
                quantityText.gameObject.SetActive(false);
            }
        }

        // Hiện giá tiền (Nếu là đồ của mình thì hiện giá Bán, đồ của Shop thì hiện giá Mua)
        int displayPrice = isPlayerInventory ? item.SellPrice : item.buyPrice;
        if (priceText != null)
        {
            priceText.text = displayPrice.ToString();
        }
    }

    public void ClearSlot()
    {
        item = null;
        if (icon != null)
        {
            icon.sprite = null;
            icon.enabled = false;
        }
        if (priceText != null) priceText.text = "";
        if (quantityText != null) quantityText.gameObject.SetActive(false);
    }

    // Gắn hàm này vào sự kiện OnClick của Button trên Prefab
    public void OnSlotClicked()
    {
        if (item != null)
        {
            if (isPlayerInventory)
            {
                // Nhấn vào đồ bên trái -> Bán
                ShopManager.instance.SellItem(item);
            }
            else
            {
                // Nhấn vào đồ bên phải -> Mua
                ShopManager.instance.BuyItem(item);
            }
        }
    }
}
