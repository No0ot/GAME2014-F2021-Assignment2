using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Touch Input")]
    public Joystick joystick;

    [Header("Base Movement")]
    public float horizontalForce;
    public float jumpForce;
    public bool isGrounded;
    [Range(0.1f, 0.9f)]
    public float airControlFactor;
    public float groundRadius;
    public LayerMask groundLayerMask;
    public float maxSpeed;

    [Header("Wall Movement")]
    public bool wallSlide;
    public bool wallJump;
    public float wallSlideSpeed;
    public Vector2 walljumpForce;
    public LayerMask wallLayerMask;
    public float wallRadius;

    [Header("References")]
    private Rigidbody2D rigidbody;
    private Animator animatorController;
    public Transform groundOrigin;
    public Transform wallOrigin;
    public GameObject sprite;
    public PlayerAnimationScript animator;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        Move();
        CheckIfGrounded();
    }

    private void Move()
    {
        // Keyboard Input
        float x = (Input.GetAxisRaw("Horizontal") + joystick.Horizontal);
        float y = (Input.GetAxisRaw("Vertical") + joystick.Vertical);
        float jump = Input.GetAxisRaw("Jump") + ((UIControls.jumpButtonDown) ? 1.0f : 0.0f);
        float mass = rigidbody.mass * rigidbody.gravityScale;

        //animator.PassInInput(x, y);
        animator.PassInVelocity(Mathf.Abs(rigidbody.velocity.x), rigidbody.velocity.y);
        if (jump != 0)
            animator.IsJumping(true);
        else
            animator.IsJumping(false);
        if(wallSlide)
            animator.IsWallSliding(true);
        else
            animator.IsWallSliding(false);

        animator.IsGrounded(isGrounded);
        if (isGrounded)
        {
            wallSlide = false;


            // Check for Flip
            if (x != 0)
            {
                x = FlipAnimation(x);
            }

            float jumpMoveForce = jump * jumpForce;



            rigidbody.AddForce(new Vector2(LimitSpeed(x), jumpMoveForce) * mass);
            rigidbody.velocity *= 0.99f; // scaling / stopping hack
        }
        else // Air Control
        {

            if (x != 0)
            {
                x = FlipAnimation(x);

                float horizontalMoveForce = LimitSpeed(x) * airControlFactor;

                rigidbody.AddForce(new Vector2(horizontalMoveForce, 0.0f) * mass);
            }


        }

        float wallJumpDirection = CheckWallSlideDirection();

        if (wallJumpDirection != 0 && !isGrounded && x != 0)
        {
            wallSlide = true;
           //FlipAnimation(-x);
        }
        else
            wallSlide = false;
        if (wallSlide)
        {
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, -wallSlideSpeed);
        }

        if (jump != 0 && wallSlide)
            wallJump = true;

        if (wallJump)
        {
            wallJump = false;
            rigidbody.AddForce(new Vector2(walljumpForce.x * wallJumpDirection, walljumpForce.y) * mass);
        }
    }

    private float LimitSpeed(float x)
    {
        float moveForce = 0;
        if (x > 0)
        {
            if (rigidbody.velocity.x < maxSpeed)
                moveForce = x * horizontalForce;
            else
                moveForce = 0;
        }
        else if (x < 0)
        {
            if (rigidbody.velocity.x > -maxSpeed)
                moveForce = x * horizontalForce;
            else
                moveForce = 0;
        }
        return moveForce;
    }
    private float CheckWallSlideDirection()
    {
        //RaycastHit2D hitRight = Physics2D.CircleCast(groundOrigin.position, groundRadius, Vector2.right, groundRadius, wallLayerMask);
        //RaycastHit2D hitLeft = Physics2D.CircleCast(groundOrigin.position, groundRadius, Vector2.left, groundRadius, wallLayerMask);
        RaycastHit2D hitLeft = Physics2D.Linecast(wallOrigin.position,  wallOrigin.position - (Vector3.right * wallRadius), wallLayerMask);
        RaycastHit2D hitRight = Physics2D.Linecast(wallOrigin.position, wallOrigin.position - (Vector3.left *  wallRadius), wallLayerMask);
        if (hitRight)
        {
            Debug.Log(-1);
            return -1;
        }
        else if (hitLeft)
        {
            Debug.Log(1);
            return 1;
        }
        else
        {
            return 0;
        }
    }

    private void CheckIfGrounded()
    {
        Vector3 leftSide = new Vector3(groundOrigin.position.x - groundRadius, groundOrigin.position.y, groundOrigin.position.z);
        Vector3 rightSide = new Vector3(groundOrigin.position.x + groundRadius, groundOrigin.position.y, groundOrigin.position.z);
        //RaycastHit2D hit = Physics2D.CircleCast(groundOrigin.position, groundRadius, Vector2.down, groundRadius, groundLayerMask);
        RaycastHit2D hitLeft = Physics2D.Linecast(leftSide, leftSide - (Vector3.up * groundRadius), groundLayerMask);
        RaycastHit2D hitRight = Physics2D.Linecast(rightSide, rightSide - (Vector3.up * groundRadius), groundLayerMask);

        if (hitLeft || hitRight)
            isGrounded = true;
        else
            isGrounded = false;
    }

    private float FlipAnimation(float x)
    {
        // depending on direction scale across the x-axis either 1 or -1
        x = (x > 0) ? 1 : -1;

        sprite.transform.localScale = new Vector3(x, 1.0f);
        return x;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(groundOrigin.position, groundRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(wallOrigin.position, wallRadius);

    }
}
