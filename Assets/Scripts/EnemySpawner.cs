using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    [Tooltip("Kéo prefab của Slime (hoặc Enemy) vào đây")]
    public GameObject enemyPrefab;
    
    [Tooltip("Thời gian hồi sinh (tính bằng giây). 3 phút = 180 giây")]
    public float respawnTime = 180f; 

    private GameObject currentEnemy;
    private bool isRespawning = false;

    void Start()
    {
        // Sinh ra quái vật lần đầu tiên khi game bắt đầu
        SpawnEnemy();
    }

    void Update()
    {
        // Kiểm tra xem quái vật đã bị Destroy chưa (khi Destroy, currentEnemy sẽ tự động bằng null)
        if (currentEnemy == null && !isRespawning)
        {
            StartCoroutine(RespawnRoutine());
        }
    }

    private IEnumerator RespawnRoutine()
    {
        isRespawning = true;
        
        // Chờ thời gian đếm ngược (VD: 3 phút)
        yield return new WaitForSeconds(respawnTime);
        
        // Hồi sinh quái vật
        SpawnEnemy();
        
        isRespawning = false;
    }

    private void SpawnEnemy()
    {
        if (enemyPrefab != null)
        {
            // Tạo ra enemy mới tại đúng vị trí (Spawnpoint) của GameObject chứa script này
            currentEnemy = Instantiate(enemyPrefab, transform.position, transform.rotation);
        }
        else
        {
            Debug.LogWarning("Chưa gắn Enemy Prefab vào Spawner!");
        }
    }
}
