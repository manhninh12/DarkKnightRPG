using UnityEngine;

public class MinimapUI : MonoBehaviour
{
    [Tooltip("Khung chứa bức ảnh bản đồ (RectTransform) sẽ trượt qua lại bên trong Mask")]
    public RectTransform mapImageContent;

    private Transform playerTransform;

    private void Start()
    {
        // Tìm player ngay khi bắt đầu game
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
    }

    private void Update()
    {
        if (playerTransform == null || MapManager.instance == null) return;

        // Lấy tọa độ ảo của Player trên ảnh bản đồ
        Vector2 targetUIPos = MapManager.instance.GetPlayerUIPosition(playerTransform);

        // Bí quyết của Minimap: Bạn ghim cái Icon người chơi cố định ở giữa (0,0) của Minimap.
        // Sau đó, bạn bắt bức ảnh bản đồ trượt ngược lại với tọa độ của Player.
        // Hiệu ứng tạo ra sẽ là bản đồ tự động cuộn (Scroll) theo nhân vật!
        if (mapImageContent != null)
        {
            mapImageContent.localPosition = -targetUIPos;
        }
    }
}
