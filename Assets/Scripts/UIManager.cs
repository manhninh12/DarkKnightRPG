using UnityEngine;
using TMPro; // Sử dụng TextMeshPro
public class UIManager : MonoBehaviour
{
    [Header("References")]
    public PlayerController playerController;
    public GameObject playerMenuPanel; // Panel chứa Menu mới thiết kế
    [Header("UI Elements")]
    public string playerName = "FROG"; 
    public TextMeshProUGUI nameUI;
    public TextMeshProUGUI healthUI;
    public TextMeshProUGUI speedUI;
    public TextMeshProUGUI jumpUI;
    public TextMeshProUGUI coinHUDText;
    public TextMeshProUGUI coinMenuText;
    void Start()
    {
        // Đảm bảo Menu luôn ẩn khi vừa bấm Play game
        if (playerMenuPanel != null)
        {
            playerMenuPanel.SetActive(false);
        }
        
        if (playerController != null)
        {
            playerController.OnCoinChanged += UpdateCoinHUD;
            UpdateCoinHUD(playerController.currentCoins);
        }
    }

    private void OnDestroy()
    {
        if (playerController != null)
        {
            playerController.OnCoinChanged -= UpdateCoinHUD;
        }
    }

    private void UpdateCoinHUD(int coins)
    {
        if (coinHUDText != null)
        {
            coinHUDText.text = coins.ToString();
        }
    }

    void Update()
    {
        // Nhấn phím C để bật/tắt Menu
        if (Input.GetKeyDown(KeyCode.C))
        {
            ToggleMenu();
        }
        // Nếu Menu đang bật thì mới cần cập nhật thông số để tiết kiệm hiệu năng
        if (playerMenuPanel.activeSelf && playerController != null)
        {
            UpdateMenuUI();
        }
    }
    public void ToggleMenu()
    {
        playerMenuPanel.SetActive(!playerMenuPanel.activeSelf);
    }
    void UpdateMenuUI()
    {
        if (nameUI != null) nameUI.text = playerName;
        if (healthUI != null) healthUI.text = playerController.currentHealth + "/" + playerController.maxHealth;
        if (speedUI != null) speedUI.text = playerController.moveSpeed.ToString(); 
        if (jumpUI != null) jumpUI.text = playerController.amountOfJumps.ToString();
        if (coinMenuText != null) coinMenuText.text = playerController.currentCoins.ToString();
    }
}
