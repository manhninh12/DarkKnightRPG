using UnityEngine;

public class MapManager : MonoBehaviour
{
    public static MapManager instance;

    public MapRoom currentRoom { get; private set; }

    private void Awake()
    {
        // Đảm bảo chỉ có 1 MapManager duy nhất tồn tại xuyên suốt game
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SetCurrentRoom(MapRoom room)
    {
        currentRoom = room;
        Debug.Log("Cập nhật Bản đồ - Đã vào khu vực: " + room.gameObject.scene.name);
    }
    
    // Thuật toán cốt lõi: Quy đổi tọa độ 3D của Player thành tọa độ 2D trên bức ảnh UI
    public Vector2 GetPlayerUIPosition(Transform playerTransform)
    {
        if (currentRoom == null || playerTransform == null) return Vector2.zero;

        // Khoảng cách thực tế từ Player đến tâm căn phòng (theo hệ trục tọa độ Unity)
        Vector2 offsetFromCenter = (Vector2)playerTransform.position - currentRoom.RoomWorldCenter;

        // Quy đổi ra khoảng cách trên bức ảnh (nhân với tỉ lệ mapScale)
        Vector2 uiOffset = offsetFromCenter * currentRoom.mapScale;

        // Vị trí cuối cùng trên UI = Vị trí phòng (trên UI) + Khoảng cách chênh lệch
        return currentRoom.roomUICenter + uiOffset;
    }
}
