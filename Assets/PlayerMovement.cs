using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Movement parameters
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float jumpForce = 10f;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] GameManager gameManager;

    bool isMobile; 

    // Components
    Rigidbody2D rb;
    CapsuleCollider2D col;

    // Ground check

    void Start()
    {
        // Get Rigidbody2D and CapsuleCollider2D components
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<CapsuleCollider2D>();
        isMobile = Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer;
    }

    void Update()
    {
        // Just can control if the game is not over
        if (!gameManager.IsGameOver()) {
            // Handle horizontal movement
            float moveDirection;
            if (isMobile && SystemInfo.supportsAccelerometer) 
            {
                moveDirection = Input.acceleration.x * 2.4F;
            } 
            else 
            {
                moveDirection = Input.GetAxis("Horizontal");
            }
            
            rb.linearVelocity = new Vector2(moveDirection * moveSpeed, rb.linearVelocity.y);

            // Handle jump
            if ((Input.GetMouseButtonDown(0) || Input.GetButtonDown("Jump")) && IsGrounded())
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
