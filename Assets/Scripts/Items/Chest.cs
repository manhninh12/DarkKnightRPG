using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    [Header("Visuals")]
    [SerializeField] private GameObject visualCue;
    [SerializeField] private Animator animator;

    [Header("Drop Settings")]
    [SerializeField] private GameObject coinPrefab;
    
    private bool playerInRange;
    private bool isOpened;

    private void Awake() 
    {
        playerInRange = false;
        isOpened = false;
        if (visualCue != null)
        {
            visualCue.SetActive(false);
        }
    }

    private void Update() 
    {
        if (playerInRange && !isOpened) 
        {
            if (visualCue != null) visualCue.SetActive(true);

            if (InputManager.GetInstance().GetInteractPressed()) 
            {
                OpenChest();
            }
        }
        else 
        {
            if (visualCue != null) visualCue.SetActive(false);
        }
    }

    private void OpenChest()
    {
        isOpened = true;
        if (visualCue != null) visualCue.SetActive(false);

        if (animator != null)
        {
            animator.SetTrigger("OpenChest"); // Tên Trigger này phải trùng với trong Animator
        }

        StartCoroutine(DropCoinsRoutine());
    }

    private IEnumerator DropCoinsRoutine()
    {
        // Chờ 0.5s để nắp rương mở ra xong
        yield return new WaitForSeconds(0.5f);

        if (coinPrefab != null)
        {
            int dropCount = Random.Range(7, 10); // Rớt từ 7 đến 10 đồng
            for (int i = 0; i < dropCount; i++)
            {
                // Rải đều coin xung quanh
                Vector3 randomOffset = new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(0f, 0.5f), 0);
                GameObject droppedCoin = Instantiate(coinPrefab, transform.position + randomOffset, Quaternion.identity);
                
                // Thêm lực nảy tạo cảm giác văng ra
                Rigidbody2D coinRb = droppedCoin.GetComponent<Rigidbody2D>();
                if (coinRb != null)
                {
                    coinRb.AddForce(new Vector2(Random.Range(-2f, 2f), Random.Range(3f, 5f)), ForceMode2D.Impulse);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collider) 
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collider) 
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}
