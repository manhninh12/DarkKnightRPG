using UnityEngine;

public class Climb : MonoBehaviour
{
    public float climbSpeed = 5f;
    private bool isClimbing = false;
    private Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb= GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Ladder"))
        {
            isClimbing = true;
            rb.gravityScale = 0f; // Disable gravity while climbing
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isClimbing = false;
            rb.gravityScale = 1f; // Re-enable gravity when not climbing
        }
    }
    private void FixedUpdate()
    {
        if (isClimbing)
        {
            float verticalInput = Input.GetAxis("Vertical");
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, verticalInput * climbSpeed);
        }
    }
}
