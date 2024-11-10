using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Movement parameters
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float jumpForce = 10f;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] GameManager gameManager;

    // Components
    Rigidbody2D rb;
    CapsuleCollider2D col;

    // Ground check

    void Start()
    {
        // Get Rigidbody2D and CapsuleCollider2D components
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<CapsuleCollider2D>();
    }

    void Update()
    {
        // soh move se nao tiver no gameover
        if (!gameManager.IsGameOver()) {
            // Handle horizontal movement
            float moveDirection = Input.GetAxis("Horizontal");
            rb.linearVelocity = new Vector2(moveDirection * moveSpeed, rb.linearVelocity.y);

            // Handle jump
            if (Input.GetButtonDown("Jump") && IsGrounded())
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            }
        }
    }

    // Grounded check using CapsuleCollider2D bounds
    private bool IsGrounded()
    {
        // Check for ground layer within the collider bounds
        RaycastHit2D raycastHit = Physics2D.Raycast(col.bounds.center, Vector2.down, col.bounds.extents.y + 0.1f, groundLayer);
        return raycastHit.collider != null;
    }
}
