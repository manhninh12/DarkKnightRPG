#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;
using System.IO;

public class MapGenerator : MonoBehaviour
{
    // Tạo một nút bấm trên thanh Menu của Unity
    [MenuItem("Tools/Generate Map Image")]
    public static void GenerateMapImage()
    {
        // 1. Lọc ra những Tilemap có Tag hoặc Layer là "Ground"
        TilemapRenderer[] allTilemaps = FindObjectsOfType<TilemapRenderer>();
        System.Collections.Generic.List<TilemapRenderer> groundTilemaps = new System.Collections.Generic.List<TilemapRenderer>();

        foreach (var tm in allTilemaps)
        {
            // Kiểm tra xem nó có Tag là Ground HOẶC Layer là Ground không
            if (tm.CompareTag("Ground") || LayerMask.LayerToName(tm.gameObject.layer) == "Ground")
            {
                groundTilemaps.Add(tm);
            }
        }

        if (groundTilemaps.Count == 0)
        {
            Debug.LogWarning("Không tìm thấy Tilemap nào có Tag hoặc Layer là 'Ground' trong Scene này!");
            return;
        }

        // 2. Tính toán khung bao trọn (Bounds) của những địa hình Ground
        Bounds bounds = groundTilemaps[0].bounds;
        for (int i = 1; i < groundTilemaps.Count; i++)
        {
            bounds.Encapsulate(groundTilemaps[i].bounds);
        }

        // Mở rộng viền thêm một chút cho đẹp
        bounds.Expand(2f);

        // --- BÍ QUYẾT TÀNG HÌNH: Tạm thời tắt hết mọi thứ không phải Ground ---
        Renderer[] allRenderers = FindObjectsOfType<Renderer>();
        bool[] originalStates = new bool[allRenderers.Length];
        
        for (int i = 0; i < allRenderers.Length; i++)
        {
            originalStates[i] = allRenderers[i].enabled;
            // Tắt hiển thị nếu nó KHÔNG nằm trong danh sách groundTilemaps
            if (!groundTilemaps.Contains(allRenderers[i] as TilemapRenderer))
            {
                allRenderers[i].enabled = false;
            }
        }
        // --------------------------------------------------------------------

        // 3. Tạo một Camera phụ để làm phó nháy
        GameObject camObj = new GameObject("TempMapCamera");
        Camera mapCam = camObj.AddComponent<Camera>();
        
        // Đặt góc nhìn 2D (Orthographic)
        mapCam.orthographic = true;
        mapCam.orthographicSize = bounds.extents.y;
        
        // Tính tỉ lệ màn hình của khung địa hình
        float targetAspect = bounds.size.x / bounds.size.y;
        
        // Căn chỉnh Camera vào chính giữa bản đồ
        camObj.transform.position = new Vector3(bounds.center.x, bounds.center.y, -10f);
        
        // Đổi màu nền Camera thành trong suốt (Transparent)
        mapCam.clearFlags = CameraClearFlags.SolidColor;
        mapCam.backgroundColor = new Color(0, 0, 0, 0); 

        // 4. Thiết lập độ phân giải ảnh (Ảnh chất lượng cao)
        int width = 2048;
        int height = Mathf.RoundToInt(width / targetAspect);

        // Tạo khung vẽ (RenderTexture)
        RenderTexture rt = new RenderTexture(width, height, 24);
        mapCam.targetTexture = rt;
        
        Texture2D screenShot = new Texture2D(width, height, TextureFormat.RGBA32, false);
        
        // Bấm máy chụp!
        mapCam.Render();
        
        // Lưu ảnh vào biến screenShot
        RenderTexture.active = rt;
        screenShot.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        screenShot.Apply();
        
        // 5. Dọn dẹp hiện trường (Xóa camera nháp)
        mapCam.targetTexture = null;
        RenderTexture.active = null; 
        DestroyImmediate(rt);
        DestroyImmediate(camObj);

        // --- Phục hồi lại trạng thái hiển thị ban đầu ---
        for (int i = 0; i < allRenderers.Length; i++)
        {
            allRenderers[i].enabled = originalStates[i];
        }
        // ------------------------------------------------

        // 6. Lưu thành file .PNG
        byte[] bytes = screenShot.EncodeToPNG();
        
        // Tạo thư mục MapImages nếu chưa có
        string folderPath = Application.dataPath + "/MapImages";
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        // Lấy tên Scene hiện tại làm tên ảnh
        string sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        if (string.IsNullOrEmpty(sceneName)) sceneName = "UnsavedScene";
        
        string filename = folderPath + "/" + sceneName + "_Map.png";
        File.WriteAllBytes(filename, bytes);
        
        Debug.Log($"<color=green>Đã chụp ảnh Bản đồ thành công!</color> Ảnh được lưu tại: Assets/MapImages/{sceneName}_Map.png");
        
        // Ép Unity tải lại thư mục để hiển thị file ảnh mới
        AssetDatabase.Refresh();
    }
}
#endif
