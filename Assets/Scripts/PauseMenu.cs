using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [Header("UI References")]
    public GameObject pauseMenuPanel;   // Kéo Panel Menu giữa màn hình vào đây
    public GameObject openMenuButton;   // Kéo nút Icon góc phải trên vào đây

    private bool isPaused = false;

    void Start()
    {
        // Đảm bảo trạng thái ban đầu: Panel ẩn, nút Open hiện
        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(false);
        if (openMenuButton != null) openMenuButton.SetActive(true);
        
        Time.timeScale = 1f; // Chắc chắn rằng game không bị dừng khi vừa load xong
    }

    void Update()
    {
        // Nhấn phím Esc để bật/tắt menu
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                OpenMenu();
            }
        }
    }

    // Hàm gọi khi nhấn vào Nút Icon góc trên bên phải
    public void OpenMenu()
    {
        isPaused = true;
        Time.timeScale = 0f; // Tạm dừng game
        
        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(true);
        if (openMenuButton != null) openMenuButton.SetActive(false); // Ẩn nút icon
    }

    // Hàm gọi khi nhấn vào Nút Continue
    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f; // Tiếp tục game
        
        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(false);
        if (openMenuButton != null) openMenuButton.SetActive(true); // Hiện lại nút icon
    }

    // Hàm gọi khi nhấn vào Nút Quit
    public void QuitToMainMenu()
    {
        Time.timeScale = 1f; // Phục hồi thời gian trước khi đổi Scene
        SceneManager.LoadScene("MainMenu");
    }
}
