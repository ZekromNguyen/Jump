using UnityEngine;

public class Movement : MonoBehaviour
{
    public float moveSpeed = 7f;
    public float maxJumpForce = 37f;
    public float chargeRate = 50f;
    public LayerMask groundLayer;

    [SerializeField] private AudioClip runningSound;
    [SerializeField] private AudioClip jumpingSound;

    private Rigidbody2D rb;
    private Animator animator;
    private AudioSource audioSource;
    private bool isGrounded;
    private bool isChargingJump;
    private bool isJumping;
    private float jumpForce;
    private int jumpDirection;
    private bool facingRight = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = gameObject.AddComponent<AudioSource>(); // Thêm AudioSource vào Player
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

        UpdateAnimation();
        PlayRunningSound();
    }

    void HandleMovement()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");

        if (!isChargingJump)
        {
            rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
            FlipCharacter(moveInput);
        }
        else if (moveInput != 0)
        {
            jumpDirection = (int)moveInput;
        }
    }

    void FlipCharacter(float moveInput)
    {
        if ((moveInput > 0 && !facingRight) || (moveInput < 0 && facingRight))
        {
            facingRight = !facingRight;
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
    }

    void HandleJump()
    {
        if (isGrounded && !isJumping)
        {
            float moveInput = Input.GetAxisRaw("Horizontal");
            if (moveInput != 0)
            {
                jumpDirection = (int)moveInput;
                FlipCharacter(jumpDirection);
            }

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

                // Nếu đã đạt lực tối đa, tự động nhảy
                //if (jumpForce >= maxJumpForce)
                //{
                //    Jump();
                //}
            }

            if (Input.GetKeyUp(KeyCode.Space) && isChargingJump)
            {
                Jump();
            }
        }
    }

    void Jump()
    {
        isChargingJump = false;
        isJumping = true;
        animator.SetBool("isJumping", true);

        FlipCharacter(jumpDirection);
        float horizontalJumpSpeed = jumpDirection * moveSpeed * 1.4f;
        float verticalJumpSpeed = jumpForce; 

        rb.linearVelocity = new Vector2(horizontalJumpSpeed, verticalJumpSpeed);

        PlayJumpSound(); 
    }

    void UpdateAnimation()
    {
        animator.SetBool("isRunning", Mathf.Abs(rb.linearVelocity.x) > 0.1f && isGrounded);
        animator.SetBool("isJumping", !isGrounded);
        animator.SetBool("isCrouching", isCrouching());
    }

    private bool isCrouching()
    {
        return isChargingJump;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (contact.normal.y > 0.5f)
            {
                isJumping = false;
                return;
            }
        }
    }

    private void PlayJumpSound()
    {
        if (jumpingSound != null)
        {
            audioSource.PlayOneShot(jumpingSound);
        }
    }

    private void PlayRunningSound()
    {
        if (runningSound != null && rb.linearVelocity.x != 0 && isGrounded)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.clip = runningSound;
                audioSource.loop = true;
                audioSource.Play();
            }
        }
        else
        {
            audioSource.Stop();
        }
    }
}