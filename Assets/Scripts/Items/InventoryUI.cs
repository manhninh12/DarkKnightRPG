using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public Transform itemsParent; // Nơi chứa toàn bộ các Slot (thường là 1 Grid Layout Group)
    public GameObject inventoryUI; // Panel tổng của giao diện túi đồ (để bật/tắt)

    Inventory inventory;
    InventorySlot[] slots; // Mảng chứa tất cả các ô trong túi

    void Start()
    {
        inventory = Inventory.instance;
        if (inventory != null)
        {
            // Đăng ký sự kiện: Cứ mỗi khi Inventory.items bị thay đổi thì gọi hàm UpdateUI
            inventory.onItemChangedCallback += UpdateUI;
        }

        // Tự động tìm tất cả các script InventorySlot nằm bên trong itemsParent (true để tìm cả những cái đang ẩn)
        slots = itemsParent.GetComponentsInChildren<InventorySlot>(true);

        // Gọi hàm UpdateUI ngay lập tức để đồng bộ nếu đã nhặt đồ trước khi mở túi
        UpdateUI();
    }

    void Update()
    {
        // Bấm phím I (hoặc Tab) để Mở/Đóng túi đồ
        if (Input.GetKeyDown(KeyCode.I) || Input.GetKeyDown(KeyCode.Tab))
        {
            inventoryUI.SetActive(!inventoryUI.activeSelf);
        }
    }

    // Hàm này tự động làm mới giao diện
    void UpdateUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < inventory.items.Count) // Nếu vị trí này có Item
            {
                slots[i].AddItem(inventory.items[i]);
            }
            else // Nếu không có Item (chỗ trống)
            {
                slots[i].ClearSlot();
            }
        }
    }
}
