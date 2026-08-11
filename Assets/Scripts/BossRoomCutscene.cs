using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class BossRoomCutscene : MonoBehaviour
{
    [Header("Actors")]
    [SerializeField] private Transform player;
    [SerializeField] private Transform boss;

    [Header("Camera")]
    [SerializeField] private Camera cam;
    [SerializeField] private Transform camPosPlayer;
    [SerializeField] private Transform camPosBoss;
    [SerializeField] private Transform camPosWide;
    [SerializeField] private float panSpeed = 4f;
    [SerializeField] private float closeSize = 5f;
    [SerializeField] private float wideSize = 8f;
    [SerializeField] private float zoomSpeed = 3f;

    [Header("Beats")]
    [SerializeField] private float holdOnPlayer = 1f;
    [SerializeField] private float holdOnBoss = 1.5f;
    [SerializeField] private float holdOnWide = 0.6f;

    [Header("The Pull")]
    [SerializeField] private float windupHold = 0.5f;
    [SerializeField] private float pullDuration = 0.5f;
    [SerializeField] private float stopShortOf = 2f;
    [SerializeField] private float hoverHeight = 2f;
    [SerializeField] private float hoverTime = 0.8f;
    [SerializeField] private float bobAmount = 0.15f;
    [SerializeField] private float bobSpeed = 4f;

    [Header("Flash")]
    [SerializeField] private Image flashOverlay;
    [SerializeField] private float flashInSpeed = 14f;

    [Header("To Be Continued")]
    [SerializeField] private Image toBeContinued;
    [SerializeField] private float cardFadeSpeed = 1.5f;
    [SerializeField] private float cardHold = 3f;
    [SerializeField] private string sceneAfterDeath = "MainMenu";

    private Animator bossAnim;

    private void Start()
    {
        Time.timeScale = 1f;

        bossAnim = boss.GetComponent<Animator>();

        LockPlayer();
        cam.transform.position = camPosPlayer.position;
        cam.orthographicSize = closeSize;

        StartCoroutine(PlayCutscene());
    }

    private void LockPlayer()
    {
        var controller = player.GetComponent<PlayerController>();
        if (controller != null) controller.enabled = false;

        var rb = player.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Kinematic;
        }

        var anim = player.GetComponent<Animator>();
        if (anim != null) anim.SetFloat("Speed", 0f);
    }

    private IEnumerator PlayCutscene()
    {
        yield return new WaitForSeconds(holdOnPlayer);

        yield return MoveCam(camPosBoss.position, closeSize);
        yield return new WaitForSeconds(holdOnBoss);

        yield return MoveCam(camPosWide.position, wideSize);
        yield return new WaitForSeconds(holdOnWide);

        if (bossAnim != null) bossAnim.SetTrigger("AttackWindup");
        yield return new WaitForSeconds(windupHold);

        yield return PullPlayerIn();
        yield return Hover(hoverTime);

        if (bossAnim != null) bossAnim.SetTrigger("AttackStrike");
        yield return new WaitForSeconds(0.2f);

        yield return Flash();

        player.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.3f);

        // white bleeds down to black
        Color c = flashOverlay.color;
        while (c.r > 0f)
        {
            c.r -= cardFadeSpeed * Time.deltaTime;
            c.g = c.r;
            c.b = c.r;
            flashOverlay.color = c;
            yield return null;
        }
        c.r = 0f; c.g = 0f; c.b = 0f;
        flashOverlay.color = c;

        // card fades up
        Color t = toBeContinued.color;
        while (t.a < 1f)
        {
            t.a += cardFadeSpeed * Time.deltaTime;
            toBeContinued.color = t;
            yield return null;
        }

        yield return new WaitForSeconds(cardHold);

        SceneManager.LoadScene(sceneAfterDeath);
    }

    private IEnumerator PullPlayerIn()
    {
        Vector3 start = player.position;
        float dir = Mathf.Sign(boss.position.x - start.x);

        Vector3 end = new Vector3(
            boss.position.x - stopShortOf * dir,
            boss.position.y + hoverHeight,
            start.z);

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / pullDuration;
            player.position = Vector3.Lerp(start, end, Mathf.SmoothStep(0f, 1f, t));
            yield return null;
        }
        player.position = end;
    }

    private IEnumerator Hover(float duration)
    {
        Vector3 basePos = player.position;
        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            Vector3 p = basePos;
            p.y += Mathf.Sin(t * bobSpeed) * bobAmount;
            player.position = p;
            yield return null;
        }
    }

    private IEnumerator Flash()
    {
        Color c = flashOverlay.color;
        while (c.a < 1f)
        {
            c.a += flashInSpeed * Time.deltaTime;
            flashOverlay.color = c;
            yield return null;
        }
        c.a = 1f;
        flashOverlay.color = c;
    }

    private IEnumerator MoveCam(Vector3 target, float targetSize)
    {
        while (Vector3.Distance(cam.transform.position, target) > 0.02f ||
               Mathf.Abs(cam.orthographicSize - targetSize) > 0.02f)
        {
            cam.transform.position = Vector3.MoveTowards(
                cam.transform.position, target, panSpeed * Time.deltaTime);
            cam.orthographicSize = Mathf.MoveTowards(
                cam.orthographicSize, targetSize, zoomSpeed * Time.deltaTime);
            yield return null;
        }
    }
}