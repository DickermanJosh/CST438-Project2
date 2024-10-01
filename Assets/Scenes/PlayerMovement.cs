using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    private Rigidbody2D rb;
    private bool isGrounded;
    private bool isFacingRight = true; // Default facing right

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Horizontal movement
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(moveHorizontal * moveSpeed, rb.velocity.y);

        // Jumping
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            isGrounded = false;
        }

        // Handle flipping the player sprite
        Flip(moveHorizontal);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Foreground"))
        {
            isGrounded = true;
        }
    }

    private void Flip(float moveHorizontal)
    {
        if (moveHorizontal > 0 && !isFacingRight)
        {
            FlipPlayer();
        }
        else if (moveHorizontal < 0 && isFacingRight)
        {
            FlipPlayer();
        }
    }

    private void FlipPlayer()
    {
        isFacingRight = !isFacingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
    }
}
