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

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
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
        if (playerTransform == null || MapManager.instance == null) return;

        // Nhờ MapManager quy đổi tọa độ 3D thành tọa độ điểm trên ảnh Bản đồ
        Vector2 targetUIPos = MapManager.instance.GetPlayerUIPosition(playerTransform);

        // Gán vị trí cho Icon Player
        if (playerIcon != null)
        {
            playerIcon.localPosition = targetUIPos;
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
