using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorCutsceneTrigger : MonoBehaviour
{
    [SerializeField] private string sceneToLoad = "BossRoom";
    [SerializeField] private float pauseBeforeLoad = 1f;

    private bool triggered;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered) return;
        if (!other.CompareTag("Player")) return;

        triggered = true;
        StartCoroutine(PlayCutscene(other.gameObject));
    }

    private IEnumerator PlayCutscene(GameObject player)
    {
        // take control away from the player
        var controller = player.GetComponent<PlayerController>();
        if (controller != null) controller.enabled = false;

        var rb = player.GetComponent<Rigidbody2D>();
        if (rb != null) rb.linearVelocity = Vector2.zero;

        var anim = player.GetComponent<Animator>();
        if (anim != null) anim.SetFloat("Speed", 0f);

        yield return new WaitForSeconds(pauseBeforeLoad);

        SceneManager.LoadScene(sceneToLoad);
    }
}