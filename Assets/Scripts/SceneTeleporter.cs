using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTeleporter : MonoBehaviour
{
    [Tooltip("Tên của Scene mà bạn muốn chuyển đến")]
    [SerializeField] private string sceneToLoad;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Kiểm tra xem object chạm vào có phải là Player hay không
        if (collision.CompareTag("Player"))
        {
            // Tải scene mới
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}