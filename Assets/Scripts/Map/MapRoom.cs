using UnityEngine;

public class MapRoom : MonoBehaviour
{
    [Header("UI Map Settings")]
    [Tooltip("Tọa độ (X, Y) của căn phòng này trên bức ảnh Bản đồ UI (tính bằng Pixel)")]
    public Vector2 roomUICenter;

    [Tooltip("Tỉ lệ: Nhân vật đi 1 mét trong game thì Icon nhích bao nhiêu Pixel trên bản đồ?")]
    public float mapScale = 10f;

    // Vị trí của cái GameObject này chính là tâm của căn phòng trong Game
    public Vector2 RoomWorldCenter => transform.position;

    private void Start()
    {
        // Khi Scene chạy, tự động báo cho MapManager biết đây là phòng hiện tại
        if (MapManager.instance != null)
        {
            MapManager.instance.SetCurrentRoom(this);
        }
    }

    private void OnDrawGizmos()
    {
        // Vẽ một vòng tròn nhỏ tại vị trí của cục MapRoom này để bạn dễ căn tâm
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, 1f);
    }
}
