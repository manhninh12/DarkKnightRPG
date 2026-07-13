using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTeleporter : MonoBehaviour
{
    [Header("Teleport Settings")]
    [Tooltip("Tên của Scene muốn chuyển đến")]
    public string sceneToLoad;

    [Tooltip("Mã ID của cổng đích ở Scene tiếp theo (Ví dụ: GateB)")]
    public string destinationGateId;

    [Header("Gate Identity")]
    [Tooltip("Mã ID của cổng hiện tại (Ví dụ: GateA)")]
    public string currentGateId;

    [Tooltip("Vị trí lệch ra một chút để Player không dẫm lại vào cổng khi vừa chuyển map")]
    public Vector2 spawnOffset = new Vector2(1f, 0f); // Tọa độ lệch x=1, y=0

    // Biến static để lưu trữ cổng đích truyền giữa các Scene
    public static string targetSpawnId;

    private void Start()
    {
        // Khi load Scene, kiểm tra xem người chơi có đang muốn dịch chuyển đến cổng này không
        if (!string.IsNullOrEmpty(targetSpawnId) && targetSpawnId == currentGateId)
        {
            // Tìm nhân vật
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                // Đặt vị trí nhân vật ngay cạnh cổng này (cộng thêm offset để không dẫm đè lên trigger)
                player.transform.position = new Vector3(transform.position.x + spawnOffset.x, transform.position.y + spawnOffset.y, player.transform.position.z);
                
                // Xóa targetSpawnId sau khi đã chuyển xong để tránh lỗi ở những lần sau
                targetSpawnId = "";
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Kiểm tra xem object chạm vào có phải là Player hay không
        if (collision.CompareTag("Player"))
        {
            // Lưu lại ID của cổng muốn xuất hiện ở Scene tiếp theo
            targetSpawnId = destinationGateId;
            
            // Tải scene mới
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}