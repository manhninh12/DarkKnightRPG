using UnityEngine;

public class PlayerPersistent : MonoBehaviour
{
    private void Awake()
    {
        // Tìm xem đã có Player nào khác tồn tại trong DontDestroyOnLoad chưa
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        if (players.Length > 1)
        {
            // Nếu đã có Player từ Scene trước sang, xóa bản sao này đi
            Destroy(this.gameObject);
        }
        else
        {
            // Giữ cho GameObject này không bị hủy khi load Scene mới
            DontDestroyOnLoad(this.gameObject);
        }
    }
}