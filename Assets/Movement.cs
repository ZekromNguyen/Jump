using UnityEngine;

public class Movement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float maxJumpForce = 12f;
    public float chargeRate = 6f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private Animator animator; // Thêm Animator
    private bool isGrounded;
    private bool isChargingJump;
    private bool isJumping;
    private float jumpForce;
    private int jumpDirection; // Temporary direction per jump

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>(); // Lấy Animator từ nhân vật
    }

    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(transform.position, 0.2f, groundLayer);

        if (isGrounded && !isJumping) // Reset jump state when landing
        {
            isJumping = false;
        }

        if (!isJumping) // Can move only when not jumping
        {
            HandleMovement();
            HandleJump();
        }

        UpdateAnimation(); // Cập nhật animation mỗi frame
    }

    void HandleMovement()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");

        if (!isChargingJump) // Normal movement only if not charging jump
        {
            rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
        }
        else if (moveInput != 0) // Store jump direction when pressing left or right
        {
            jumpDirection = (int)moveInput;
        }
    }

    void HandleJump()
    {
        if (isGrounded && !isJumping) // Only allow jump when grounded
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                isChargingJump = true;
                jumpForce = 0f;
                jumpDirection = 0; // Reset jump direction every charge
                rb.linearVelocity = Vector2.zero; // Stop movement while charging
            }

            if (isChargingJump)
            {
                jumpForce += chargeRate * Time.deltaTime;
                jumpForce = Mathf.Clamp(jumpForce, 0, maxJumpForce);
            }

            if (Input.GetKeyUp(KeyCode.Space) && isChargingJump)
            {
                isChargingJump = false;
                isJumping = true; // Mark character as jumping

                // Jump straight up if no direction was pressed
                rb.linearVelocity = new Vector2(jumpDirection * jumpForce, jumpForce);
            }
        }
    }

    void UpdateAnimation()
    {
        // Cập nhật animation chạy
        animator.SetBool("isRunning", Mathf.Abs(rb.linearVelocity.x) > 0.1f && isGrounded);

        // Cập nhật animation nhảy
        animator.SetBool("isJumping", !isGrounded);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (contact.normal.y > 0.5f) // If landed on ground
            {
                isJumping = false; // Allow new jumps only when fully landed
                return;
            }
        }
    }
}