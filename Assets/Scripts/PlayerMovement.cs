
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
    public float wallSlideTime = 3f; // tres segundos

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
    public GameObject PantallaMuerte;

    void Start()
    {
        animator = GetComponent<Animator>();
        PantallaMuerte.SetActive(false);
    }

    private void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        if(IsGrounded())
        {
            wallSlideTime = 3f;
        }

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
        FallingDeath();
        SuffocateDeath();

        if (!isWallJumping)
        {
            Flip();
        }

        if (Input.GetKeyDown(KeyCode.T) && HealthSystem.Instance.manaPoint >= HealthSystem.Instance.maxManaPoint)
        {
            animator.SetBool("IsTping", true);
            //TeleportPlayer();
            Invoke("TeleportPlayer", 0.2f);
            
        }
    }
    private void StopTP()
    {
        animator.SetBool("IsTping", false);

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
            rb.velocity = new Vector2(rb.velocity.x, -wallSlidingSpeed);
            animator.SetBool("IsWallSliding", true);
            if(wallSlideTime > 0)
            {
                wallSlideTime -= Time.deltaTime;
            }
            else
            {
                isWallSliding = false;
                animator.SetBool("IsWallSliding", false);
                if(!isFacingRight)
                {
                    Debug.Log("MOver izquierda");
                    playerTransform.position += new Vector3(0.1f, 0f, 0f);
                }
                else
                {
                    Debug.Log("Mover derecha");
                    playerTransform.position += new Vector3(-0.1f, 0f, 0f);
                }
            }
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
            animator.SetBool("IsWallJumping", true);

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
            
            animator.SetBool("IsTping",false);

    }
  
    private void FallingDeath()
    {
        if (transform.position.y < -30f)
        {
            DiePlayer();

            return;
        }
    }

    private void SuffocateDeath()
    {
        Vector2 playerPositiom = transform.position;

        Collider2D hitCollider = Physics2D.OverlapPoint(playerPositiom, groundLayer);

        if (hitCollider != null)
        {
            DiePlayer();
        }
    }

    public void ShowDeadScreen()
    {
        PantallaMuerte.SetActive(true);
    }

    public void DiePlayer()
    {
        Time.timeScale = 0.2f;
        animator.SetBool("IsDying", true);
        Invoke("ShowDeadScreen", 0.2f);
        Invoke("RestartLevel", 0.6f);

    }

    private void RestartLevel()
    {
        GameManager.Instance.RestartLevel();
        Time.timeScale = 1f;

    }

}