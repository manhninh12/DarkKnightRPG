using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class ShopItem
{
    public ItemData itemData;
    public int quantity;
}

public class ShopManager : MonoBehaviour
{
    public static ShopManager instance;

    public List<ShopItem> itemsForSale = new List<ShopItem>();
    
    // Sự kiện để UI biết đường cập nhật lại khi có giao dịch
    public Action onShopUpdated;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        instance = this;
    }

    // Hàm gọi khi người chơi bấm Mua (từ ô bên phải)
    public void BuyItem(ItemData item)
    {
        PlayerController player = FindAnyObjectByType<PlayerController>();
        if (player == null) return;

        // Tìm vật phẩm trong cửa hàng
        ShopItem shopItem = itemsForSale.Find(x => x.itemData == item);
        if (shopItem != null && shopItem.quantity > 0)
        {
            if (player.SpendCoins(item.buyPrice)) // Kiểm tra và trừ tiền
            {
                if (Inventory.instance.Add(item)) // Thêm vào túi đồ
                {
                    shopItem.quantity--;
                    if (shopItem.quantity <= 0)
                    {
                        itemsForSale.Remove(shopItem);
                    }
                    onShopUpdated?.Invoke();
                    Debug.Log("Mua thành công: " + item.itemName);
                }
                else
                {
                    // Trả lại tiền nếu túi đầy
                    player.AddCoins(item.buyPrice);
                    Debug.Log("Túi đồ đã đầy!");
                }
            }
            else
            {
                Debug.Log("Không đủ tiền để mua!");
            }
        }
    }

    // Hàm gọi khi người chơi bấm Bán (từ ô bên trái)
    public void SellItem(ItemData item)
    {
        PlayerController player = FindAnyObjectByType<PlayerController>();
        if (player == null) return;

        // Xóa khỏi túi đồ
        Inventory.instance.Remove(item);
        
        // Cộng tiền cho người chơi
        player.AddCoins(item.SellPrice);

        // Đưa món đồ vừa bán vào lại danh sách hàng của cửa hàng
        ShopItem shopItem = itemsForSale.Find(x => x.itemData == item);
        if (shopItem != null)
        {
            shopItem.quantity++;
        }
        else
        {
            itemsForSale.Add(new ShopItem { itemData = item, quantity = 1 });
        }
        
        onShopUpdated?.Invoke();
        Debug.Log("Bán thành công: " + item.itemName + " nhận " + item.SellPrice);
    }
}
