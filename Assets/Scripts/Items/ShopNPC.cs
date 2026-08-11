using UnityEngine;

public class ShopNPC : MonoBehaviour
{
    [SerializeField] private GameObject visualCue; // Chữ E hiện lên báo hiệu có thể tương tác
    
    private bool playerInRange;

    private void Start()
    {
        if (visualCue != null)
        {
            visualCue.SetActive(false);
        }
    }

    private void Update()
    {
        // Khi người chơi ở gần và bấm phím Interact (mặc định phím E)
        if (playerInRange)
        {
            if (InputManager.GetInstance().GetInteractPressed())
            {
                if (ShopUI.instance != null)
                {
                    // Nếu cửa hàng đang mở thì đóng, đang đóng thì mở (Toggle)
                    if (ShopUI.instance.shopPanel.activeSelf)
                    {
                        ShopUI.instance.CloseShop();
                    }
                    else
                    {
                        ShopUI.instance.OpenShop();
                    }
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = true;
            if (visualCue != null)
            {
                visualCue.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = false;
            if (visualCue != null)
            {
                visualCue.SetActive(false);
            }
            
            // Bắt buộc đóng cửa hàng nếu người chơi chạy ra khỏi phạm vi
            if (ShopUI.instance != null)
            {
                ShopUI.instance.CloseShop();
            }
        }
    }
}
