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
	private float jumpForce;
	private int jumpDirection;
	private bool facingRight = true;

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

		UpdateAnimation();
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
			}

			if (Input.GetKeyUp(KeyCode.Space) && isChargingJump)
			{
				isChargingJump = false;
				isJumping = true;
				animator.SetBool("isJumping", true);

				FlipCharacter(jumpDirection);

				rb.linearVelocity = new Vector2(jumpDirection * jumpForce, jumpForce);
			}
		}
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
}