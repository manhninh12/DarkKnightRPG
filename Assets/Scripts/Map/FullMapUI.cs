using UnityEngine;
using UnityEngine.UI;

public class FullMapUI : MonoBehaviour
{
    [Tooltip("Cái Panel đen che kín màn hình chứa Map")]
    public GameObject fullMapPanel;
    
    [Tooltip("Bức ảnh to đùng chứa Bản đồ (Nằm trong ScrollRect)")]
    public RectTransform mapContent;
    
    [Tooltip("Icon cái đầu của Player")]
    public RectTransform playerIcon;

    private Transform playerTransform;

    private void Start()
    {
        // Đảm bảo map tắt lúc mới vào game
        if (fullMapPanel != null)
            fullMapPanel.SetActive(false);
    }

    private void Update()
    {
        // Bấm phím M để mở / đóng Bản đồ tổng quát
        if (Input.GetKeyDown(KeyCode.M))
        {
            fullMapPanel.SetActive(!fullMapPanel.activeSelf);
            
            if (fullMapPanel.activeSelf)
            {
                UpdateMap();
                CenterMapOnPlayer();
            }
        }

        // Nếu Map đang mở thì cập nhật liên tục vị trí của Player Icon
        if (fullMapPanel.activeSelf)
        {
            UpdateMap();
        }
    }

    private void UpdateMap()
    {
        if (playerTransform == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null) 
            {
                playerTransform = player.transform;
            }
            else 
            {
                Debug.Log("MAP DEBUG: KHÔNG THỂ VẼ BẢN ĐỒ - Không tìm thấy nhân vật (Tag: Player) trong Scene!");
                return; 
            }
        }

        if (MapManager.instance == null)
        {
            Debug.LogWarning("MAP DEBUG: KHÔNG THỂ VẼ BẢN ĐỒ - MapManager bị null! (Chưa kéo script vào GameManager hoặc bị xóa mất).");
            return;
        }

        if (MapManager.instance.currentRoom == null)
        {
            Debug.LogWarning("MAP DEBUG: KHÔNG THỂ VẼ BẢN ĐỒ - currentRoom bị null! (Scene này không có MapRoom_Data, hoặc script MapRoom chưa chạy).");
            return;
        }

        // Nhờ MapManager quy đổi tọa độ 3D thành tọa độ điểm trên ảnh Bản đồ
        Vector2 targetUIPos = MapManager.instance.GetPlayerUIPosition(playerTransform);
        Debug.Log("<color=cyan>MAP DEBUG: Hệ thống đã tính toán vị trí của Đầu nhân vật là: " + targetUIPos + "</color>");

        // Gán vị trí cho Icon Player
        if (playerIcon != null)
        {
            playerIcon.localPosition = targetUIPos;
            
            // --- TỰ ĐỘNG SỬA LỖI ---
            // 1. Ép Icon phải nổi lên trên cùng (không bị ảnh bản đồ đè)
            playerIcon.SetAsLastSibling();
            // 2. Chống lỗi teo nhỏ Icon
            playerIcon.localScale = Vector3.one;
            
            Debug.Log("<color=green>MAP DEBUG: Đã gán vị trí và đưa đầu nhân vật lên trên cùng thành công!</color>");
        }
        else
        {
            Debug.LogError("MAP DEBUG: KHÔNG THỂ VẼ BẢN ĐỒ - Ô 'Player Icon' trong script FullMapUI đang trống, bạn chưa kéo cái đầu vào!");
        }
    }

    private void CenterMapOnPlayer()
    {
        if (mapContent != null && playerIcon != null)
        {
            // Mẹo siêu đơn giản: Kéo ngược bức ảnh Map lại đúng bằng tọa độ của Player 
            // để Player nằm chính giữa màn hình khi vừa mở Map lên.
            // *Lưu ý: Bắt buộc Pivot của mapContent phải là (0.5, 0.5)
            mapContent.localPosition = -playerIcon.localPosition;
        }
    }
}
