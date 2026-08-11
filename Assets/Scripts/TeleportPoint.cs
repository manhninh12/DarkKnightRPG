using UnityEngine;

public class TeleportPoint : MonoBehaviour
{
    [Header("UI & References")]
    [Tooltip("Chữ E hiện lên báo hiệu có thể tương tác")]
    [SerializeField] private GameObject visualCue; 
    
    [Tooltip("Animator nằm ở vật thể khác để chạy Animation")]
    [SerializeField] private Animator externalAnimator; 
    
    [Tooltip("Tên Trigger trong Animator để kích hoạt (Mặc định: Light)")]
    [SerializeField] private string animationTrigger = "Light";
    
    [Header("Spawn Settings")]
    [Tooltip("Vị trí nhân vật sẽ xuất hiện ngay sau vật dịch chuyển (Offset so với tâm)")]
    [SerializeField] private Vector3 spawnOffset = new Vector3(0f, 1f, 0f);

    private bool playerInRange;
    private bool isActivated = false;

    // Biến static để check xem người chơi có vừa dùng dịch chuyển đến không
    public static bool justTeleported = false;

    private void Start()
    {
        if (visualCue != null)
        {
            visualCue.SetActive(false);
        }

        // Nếu người chơi vừa dịch chuyển đến Scene này
        if (justTeleported)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                // Đặt vị trí player ngay sau/cạnh điểm dịch chuyển này
                player.transform.position = transform.position + spawnOffset;
            }
            
            // Đánh dấu là đã dịch chuyển xong để các Scene sau không bị nhầm
            justTeleported = false; 
            
            // Tự động kích hoạt điểm này (sáng lên) vì đã đi qua nó
            isActivated = true; 
            if (externalAnimator != null)
            {
                 externalAnimator.SetTrigger(animationTrigger);
            }
        }
    }

    private void Update()
    {
        // Khi người chơi ở gần và bấm phím Interact (mặc định phím E)
        if (playerInRange)
        {
            if (InputManager.GetInstance().GetInteractPressed())
            {
                if (!isActivated)
                {
                    // Lần đầu tương tác: Kích hoạt và chạy Animation
                    isActivated = true;
                    if (externalAnimator != null)
                    {
                        externalAnimator.SetTrigger(animationTrigger);
                    }
                    // Tùy chọn: có thể bật UI luôn ở lần 1, nhưng theo yêu cầu thì lần 2 mới hiện UI.
                }
                else
                {
                    // Lần tương tác thứ 2 trở đi: Mở UI Teleport
                    if (TeleportUI.instance != null)
                    {
                        if (TeleportUI.instance.teleportPanel.activeSelf)
                        {
                            TeleportUI.instance.CloseUI();
                        }
                        else
                        {
                            TeleportUI.instance.OpenUI();
                        }
                    }
                    else
                    {
                        Debug.LogWarning("Không tìm thấy TeleportUI trong Scene!");
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
            
            // Đi ra xa thì tự động đóng UI
            if (TeleportUI.instance != null)
            {
                TeleportUI.instance.CloseUI();
            }
        }
    }
}
