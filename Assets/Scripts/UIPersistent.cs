using UnityEngine;

public class UIPersistent : MonoBehaviour
{
    private void Awake()
    {
        // Tìm xem đã có UI nào khác tồn tại trong DontDestroyOnLoad chưa
        GameObject[] uis = GameObject.FindGameObjectsWithTag("UI");

        if (uis.Length > 1)
        {
            // Nếu đã có UI từ Scene trước sang, xóa bản sao này đi
            Destroy(this.gameObject);
        }
        else
        {
            // Giữ cho GameObject này không bị hủy khi load Scene mới
            DontDestroyOnLoad(this.gameObject);
        }
    }
}
