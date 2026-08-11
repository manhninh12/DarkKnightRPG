using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ShopUI : MonoBehaviour
{
    public static ShopUI instance;

    public GameObject shopPanel; // Khung to nhất chứa toàn bộ UI Shop
    
    [Header("Player Inventory UI")]
    public Transform playerItemsParent; // Layout chứa các Slot bên trái
    
    [Header("Shop Inventory UI")]
    public Transform shopItemsParent; // Layout chứa các Slot bên phải

    private ShopSlot[] playerSlots;
    private ShopSlot[] merchantSlots;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    void Start()
    {
        if (shopPanel != null)
            shopPanel.SetActive(false);

        // Lấy tất cả các Component ShopSlot nằm bên trong 2 khung
        playerSlots = playerItemsParent.GetComponentsInChildren<ShopSlot>(true);
        merchantSlots = shopItemsParent.GetComponentsInChildren<ShopSlot>(true);

        // Đăng ký sự kiện: Mỗi khi mua/bán xong thì hàm UpdateUI sẽ tự chạy
        if (ShopManager.instance != null)
        {
            ShopManager.instance.onShopUpdated += UpdateUI;
        }
    }

    private void OnDestroy()
    {
        if (ShopManager.instance != null)
            ShopManager.instance.onShopUpdated -= UpdateUI;
    }

    // Hàm gọi khi bấm vào chữ X hoặc NPC ra lệnh mở
    public void OpenShop()
    {
        shopPanel.SetActive(true);
        UpdateUI(); // Tải lại đồ ngay khi bật lên
    }

    // Hàm gọi khi bấm chữ X hoặc đi xa khỏi NPC
    public void CloseShop()
    {
        shopPanel.SetActive(false);
    }

    public void UpdateUI()
    {
        if (!shopPanel.activeSelf) return;

        UpdatePlayerInventoryUI();
        UpdateMerchantShopUI();
    }

    private void UpdatePlayerInventoryUI()
    {
        // Gộp các Item trùng nhau trong túi đồ thành từng cục để hiển thị số lượng (x2, x3)
        var groupedItems = Inventory.instance.items
            .GroupBy(item => item)
            .Select(group => new { Item = group.Key, Count = group.Count() })
            .ToList();

        for (int i = 0; i < playerSlots.Length; i++)
        {
            if (i < groupedItems.Count)
            {
                playerSlots[i].Setup(groupedItems[i].Item, groupedItems[i].Count, true);
            }
            else
            {
                playerSlots[i].ClearSlot();
            }
        }
    }

    private void UpdateMerchantShopUI()
    {
        var itemsForSale = ShopManager.instance.itemsForSale;

        for (int i = 0; i < merchantSlots.Length; i++)
        {
            if (i < itemsForSale.Count)
            {
                merchantSlots[i].Setup(itemsForSale[i].itemData, itemsForSale[i].quantity, false);
            }
            else
            {
                merchantSlots[i].ClearSlot();
            }
        }
    }
}
