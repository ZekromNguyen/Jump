using UnityEngine;

public class Movement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float maxJumpForce = 12f;
    public float chargeRate = 6f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private Animator animator;
    private bool isGrounded;
    private bool isChargingJump;
    private bool isJumping;
    private bool isBounding;
    private bool isCrouching; // Biến mới để kiểm tra crouching
    private float jumpForce;
    private int jumpDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(transform.position, 0.2f, groundLayer);

        if (isGrounded && !isJumping)
        {
            isJumping = false;
        }

        if (!isJumping)
        {
            HandleMovement();
            HandleJump();
        }

        HandleCrouch(); // Xử lý crouch
        UpdateAnimation();
    }

    void HandleMovement()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");

        if (!isChargingJump)
        {
            rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
        }
        else if (moveInput != 0)
        {
            jumpDirection = (int)moveInput;
        }
    }

    void HandleJump()
    {
        if (isGrounded && !isJumping)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                isChargingJump = true;
                jumpForce = 0f;
                jumpDirection = 0;
                rb.linearVelocity = Vector2.zero;
            }

            if (isChargingJump)
            {
                jumpForce += chargeRate * Time.deltaTime;
                jumpForce = Mathf.Clamp(jumpForce, 0, maxJumpForce);
            }

            if (Input.GetKeyUp(KeyCode.Space) && isChargingJump)
            {
                isChargingJump = false;
                isJumping = true; // ✅ Đánh dấu đang nhảy, không reset ngay lập tức
                animator.SetBool("isJumping", true); // ✅ Giữ trạng thái Jump

                rb.linearVelocity = new Vector2(jumpDirection * jumpForce, jumpForce);
            }
        }
    }



    void HandleCrouch()
    {
        isCrouching = Input.GetKey(KeyCode.Space); // Kiểm tra giữ Space
    }

    void UpdateAnimation()
    {
        animator.SetBool("isRunning", Mathf.Abs(rb.linearVelocity.x) > 0.1f && isGrounded && !isJumping);
        // Chỉ đặt Jump nếu thực sự không grounded
        if (!isGrounded)
        {
            animator.SetBool("isJumping", true);
        }
        else
        {
            animator.SetBool("isJumping", false);
        }
        animator.SetBool("isBounding", isBounding);
        animator.SetBool("isCrouching", isCrouching); // Cập nhật animation crouching
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (contact.normal.y > 0.5f) // Chỉ reset khi tiếp đất
            {
                isJumping = false;
                isBounding = true;
                animator.SetBool("isJumping", false); // ✅ Chỉ reset khi đã chạm đất
                Invoke("ResetBounding", 0.1f);
                return;
            }
        }
    }



    private void ResetBounding()
    {
        isBounding = false;
    }
}
