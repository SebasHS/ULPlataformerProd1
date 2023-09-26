/*using System.Collections;
using System.Collections.Generic;
using System.Net.WebSockets;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float runSpeed = 2f;
    [SerializeField] private float jumpSpeed = 4f;

    private float moveDirection = 0f;
    private float horizontal;
    public bool isInTheAir = true;
    private Rigidbody2D rb;
    private Animator animator;
    private CapsuleCollider2D capsuleCollider;
    private bool isFacingRight = true;

    private bool isWall;
    private bool isWallJumping;
    private float wallSliceSpeed = 2f;
    private float wallJumpingDirection;
    private float wallJumpingTime = 0.2f;
    private float wallJumpingCounter;
    private float wallJumpingDuration = 0.4f;
    private Vector2 wallJumpingPower = new Vector2(8f, 16f);

    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        playerTransform = GetComponent<Transform>();
    }

    private void OnMove(InputValue value)
    {
        moveDirection = value.Get<float>();
    }

    private void OnJump(InputValue value)
    {
        if (value.isPressed)
        {
            if (capsuleCollider.IsTouchingLayers(
                LayerMask.GetMask("Ground"))
            )
            {
                // Saltar
                animator.SetBool("IsJumping", true);
                rb.velocity += new Vector2(0f, jumpSpeed);
                isInTheAir = true;
                HealthSystem.Instance.RestoreMana(10f);
            }
        }
    }

    private void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        Run();
        FlipSprite();
        //Debug.Log(isInTheAir);
        //Debug.Log((Mathf.Abs(rb.velocity.y) < Mathf.Epsilon));
        if (isInTheAir && (Mathf.Abs(rb.velocity.y) < Mathf.Epsilon))
        {
            // Estoy en el punto mas alto del salto
            //Debug.Log("Entra");
            rb.gravityScale = 2f;
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            TeleportPlayer();
        }
    }

    private void FlipSprite()
    {
        if (Mathf.Abs(rb.velocity.x) > Mathf.Epsilon)
        {
            transform.localScale = new Vector3(
                Mathf.Sign(rb.velocity.x),
                1f,
                1f
            );

        }
    }

    private void Run()
    {
        if (moveDirection == 0f)
        {
            animator.SetBool("IsRunning", false);
        }
        else
        {
            animator.SetBool("IsRunning", true);
        }

        rb.velocity = new Vector2(
            runSpeed * moveDirection,
            rb.velocity.y
        );
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.CompareTag("Platform"))
        {
            // Finalizo el salto
            animator.SetBool("IsJumping", false);
            isInTheAir = false;
            rb.gravityScale = 1f;
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }
    private bool isWallSlide()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
    }

    private void WallSlide()
    {
        Debug.Log("----> " + isWallSlide());
        if(isWallSlide() && !isInTheAir && horizontal != 0f)
        {

            isWall = true;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSliceSpeed, float.MaxValue));
        }
        else
        {
            isWall = false;
        }
    }

     private void WallJump()
    {
        if (isWall)
        {
            isWall = false;
            wallJumpingDirection = -transform.localScale.x;
            wallJumpingCounter = wallJumpingTime;

            CancelInvoke(nameof(StopWallJumping));
        }
        else
        {
            wallJumpingCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump") && wallJumpingCounter > 0f)
        {
            isWall = true;
            rb.velocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
            wallJumpingCounter = 0f;

            if (transform.localScale.x != wallJumpingDirection)
            {
                isFacingRight = !isFacingRight;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
            }

            Invoke(nameof(StopWallJumping), wallJumpingDuration);
        }
    }

    private void StopWallJumping()
    {
        isWallJumping = false;
    }

}
*/
using System.Collections;
using System.Collections.Generic;
using System.Net.WebSockets;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Animator animator;
    private float horizontal;
    private float speed = 4f;
    private float jumpingPower = 10f;
    private bool isFacingRight = true;

    private bool isWallSliding;
    private float wallSlidingSpeed = 2f;

    private bool isWallJumping;
    private float wallJumpingDirection;
    private float wallJumpingTime = 0.2f;
    private float wallJumpingCounter;
    private float wallJumpingDuration = 0.4f;
    public Transform playerTransform;
    private Vector2 wallJumpingPower = new Vector2(8f, 16f);

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private float teleportDistance = 4f;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        if (horizontal != 0f)
        {
            //Debug.Log("Running");
            animator.SetBool("IsRunning", true);
        }
        else
        {
            //Debug.Log("Not running");
            animator.SetBool("IsRunning", false);
        }

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            //Debug.Log("Jumping");
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
            animator.SetBool("IsJumping", true);
            HealthSystem.Instance.RestoreMana(10f);
        }

        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            //Debug.Log("Not jumping");
            animator.SetBool("IsJumping", false);
            animator.SetBool("IsRunning", false);
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }

        WallSlide();
        WallJump();

        if (!isWallJumping)
        {
            Flip();
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            TeleportPlayer();
        }
    }

    private void FixedUpdate()
    {
        if (!isWallJumping)
        {
            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
        }
    }

    private bool IsGrounded()
    {
        //Debug.Log("IsGrounded");
        animator.SetBool("IsJumping", false);
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private bool IsWalled()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
    }

    private void WallSlide()
    {
        if (IsWalled() && !IsGrounded() && horizontal != 0f)
        {
            isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, -wallSlidingSpeed);//Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
            animator.SetBool("IsWallSliding", true);
        }
        else
        {
            isWallSliding = false;
            animator.SetBool("IsWallSliding", false);
        }
    }

    private void WallJump()
    {
        if (isWallSliding)
        {
            isWallJumping = false;
            animator.SetBool("IsWallJumping", false);
            animator.SetBool("IsWallSliding", true);
            wallJumpingDirection = -transform.localScale.x;
            wallJumpingCounter = wallJumpingTime;
            

            CancelInvoke(nameof(StopWallJumping));
        }
        else
        {
            wallJumpingCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump") && wallJumpingCounter > 0f)
        {
            isWallJumping = true;
            rb.velocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
            wallJumpingCounter = 0f;
            animator.SetBool("IsWallJumping",true);

            if (transform.localScale.x != wallJumpingDirection)
            {
                isFacingRight = !isFacingRight;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
            }

            Invoke(nameof(StopWallJumping), wallJumpingDuration);
        }
    }

    private void StopWallJumping()
    {
        isWallJumping = false;
        animator.SetBool("IsWallJumping", false);
    }

    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    private void TeleportPlayer()
    {
        if (HealthSystem.Instance.manaPoint >= HealthSystem.Instance.maxManaPoint)
        {
            if (isFacingRight)
            {
                playerTransform.position = playerTransform.position + Vector3.right * teleportDistance;
                HealthSystem.Instance.UseMana(HealthSystem.Instance.manaPoint);
                Debug.Log("tp derecha");
            }
            else
            {
                playerTransform.position = playerTransform.position + Vector3.left * teleportDistance;
                HealthSystem.Instance.UseMana(HealthSystem.Instance.manaPoint);
                Debug.Log("tp izquierda");
            }

        }
        else
        {
            Debug.Log("No tienes mana");
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.CompareTag("Platform"))
        {
            Debug.Log("Mori xd");
        }
    }
}