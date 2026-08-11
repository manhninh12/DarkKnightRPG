using UnityEngine;
using UnityEngine.SceneManagement;

public class TeleportUI : MonoBehaviour
{
    public static TeleportUI instance;

    [Header("UI Elements")]
    [Tooltip("Khung to nhất chứa toàn bộ Menu Dịch Chuyển")]
    public GameObject teleportPanel;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (teleportPanel != null)
        {
            teleportPanel.SetActive(false);
        }
    }

    public void OpenUI()
    {
        if (teleportPanel != null)
        {
            teleportPanel.SetActive(true);
        }
    }

    public void CloseUI()
    {
        if (teleportPanel != null)
        {
            teleportPanel.SetActive(false);
        }
    }

    /// <summary>
    /// Hàm này để gọi khi người chơi bấm vào một nút Chọn Địa Điểm trên UI.
    /// Bạn hãy gán hàm này vào sự kiện OnClick của Button, sau đó gõ tên Scene vào ô truyền tham số.
    /// </summary>
    /// <param name="sceneName">Tên của Scene muốn bay tới</param>
    public void TeleportToScene(string sceneName)
    {
        // Đặt cờ báo hiệu rằng Player đang dùng hệ thống Teleport (để khi qua Scene kia nhân vật spawn ở cột dịch chuyển)
        TeleportPoint.justTeleported = true;
        
        // Đóng UI lại (tùy chọn, vì load scene mới cũng tự mất UI, nhưng đóng cho mượt)
        CloseUI();

        // Tải Scene mới
        SceneManager.LoadScene(sceneName);
    }
}
