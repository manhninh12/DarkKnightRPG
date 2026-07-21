using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance; // Singleton để dễ truy cập

    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallback; // Sự kiện báo cho UI biết mỗi khi nhặt hoặc vứt đồ

    public int space = 16; // Số lượng ô trống tối đa trong túi
    public List<ItemData> items = new List<ItemData>();

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Cảnh báo: Có nhiều hơn 1 script Inventory trong Scene!");
            return;
        }
        instance = this;
    }

    // Hàm để nhặt đồ vào túi
    public bool Add(ItemData item)
    {
        if (items.Count >= space)
        {
            Debug.Log("Túi đồ đã đầy!");
            return false;
        }

        items.Add(item);

        // Cập nhật lại UI
        if (onItemChangedCallback != null)
            onItemChangedCallback.Invoke();

        return true;
    }

    // Hàm để vứt hoặc dùng đồ
    public void Remove(ItemData item)
    {
        items.Remove(item);

        // Cập nhật lại UI
        if (onItemChangedCallback != null)
            onItemChangedCallback.Invoke();
    }
}
