using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroController : MonoBehaviour
{
    // Tên của màn hình đầu tiên cần chuyển tới
    public string nextSceneName = "SampleScene";

    void Start()
    {
        // Dùng thời gian chờ
        Invoke("GoToNextScene", 60f);
    }

    void Update()
    {
        // Chuyển scene khi người dùng bấm phím Space hoặc bấm chuột
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            GoToNextScene();
        }
    }

    public void GoToNextScene()
    {
        SceneManager.LoadScene(nextSceneName);
    }
}
