using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Touch Input")]
    public Joystick joystick;

    [Header("Stats")]
    public float maxHealth = 100f;
    public float health;
    public float maxEnergy = 100f;
    public float energy;
    public float energyRegenRate;
    public bool isDead;

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

    [Header("Attack")]
    public float lightAttackEnergyCost;
    public float heavyAttackEnergyCost;
    public bool isAttacking;
    public bool canTakeDamage;
    public GameObject groundLightAttackCollider;
    public GameObject groundHeavyAttackCollider;
    public GameObject airLightAttackCollider;
    public GameObject airHeavyAttackCollider;
    bool canAttack;

    [Header("References")]
    private Rigidbody2D rigidbody;
    public Transform groundOrigin;
    public Transform wallOrigin;
    public GameObject sprite;

    [Header("Animation")]
    private Animator animatorController;
    public PlayerAnimationScript animator;
    private Vector3 originalLocalPosition;

    private void Start()
    {
        originalLocalPosition = animator.gameObject.transform.localPosition;
        rigidbody = GetComponent<Rigidbody2D>();
        canAttack = true;
        health = maxHealth;
        energy = maxEnergy;
        canTakeDamage = true;
    }

    void FixedUpdate()
    {
        if (!isDead)
        {
            Move();
            CheckIfGrounded();
            if (!isAttacking)
                Attack();
            FollowSprite();
            RegenerateEnergy();
        }
    }

    private void Move()
    {
        // Keyboard Input
        float x = (Input.GetAxisRaw("Horizontal") + joystick.Horizontal);
        float y = (Input.GetAxisRaw("Vertical") + joystick.Vertical);
        float jump = 0;
        if (!isAttacking)
            jump = Input.GetAxisRaw("Jump") + ((UIControls.jumpButtonDown) ? 1.0f : 0.0f);

        float mass = rigidbody.mass * rigidbody.gravityScale;

        //animator.PassInInput(x, y);
        animator.PassInVelocity(Mathf.Abs(rigidbody.velocity.x), rigidbody.velocity.y);
        if (jump != 0)
        {
            animator.IsJumping(true);
        }
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
                //x = FlipAnimation(x);

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
            FlipAnimation(-x);
        }
    }

    private float LimitSpeed(float x)
    {
        float moveForce = 0;
        float tempMaxSpeed = maxSpeed;
        if (isAttacking)
            tempMaxSpeed *= 0.25f;
        if (x > 0)
        {
            if (rigidbody.velocity.x < tempMaxSpeed)
                moveForce = x * horizontalForce;
            else
                moveForce = 0;
        }
        else if (x < 0)
        {
            if (rigidbody.velocity.x > -tempMaxSpeed)
                moveForce = x * horizontalForce;
            else
                moveForce = 0;
        }

        return moveForce;
    }

    private void Attack()
    {
        float a = (Input.GetAxisRaw("Fire1"));
        float h = (Input.GetAxisRaw("Fire2"));

        if (a != 0 && energy > lightAttackEnergyCost)
        {
            animator.LightAttack();
            isAttacking = true;
            energy -= lightAttackEnergyCost;
        }
        if (h != 0 && energy > heavyAttackEnergyCost)
        {
            animator.HeavyAttack();
            isAttacking = true;
            if (!isGrounded)
                rigidbody.gravityScale = 6;

            energy -= heavyAttackEnergyCost;
        }

        if(!isAttacking)
        {
            groundLightAttackCollider.SetActive(false);
            groundHeavyAttackCollider.SetActive(false);
            airLightAttackCollider.SetActive(false);
            airHeavyAttackCollider.SetActive(false);
        }

    }
    private float CheckWallSlideDirection()
    {
        //RaycastHit2D hitRight = Physics2D.CircleCast(groundOrigin.position, groundRadius, Vector2.right, groundRadius, wallLayerMask);
        //RaycastHit2D hitLeft = Physics2D.CircleCast(groundOrigin.position, groundRadius, Vector2.left, groundRadius, wallLayerMask);
        RaycastHit2D hitLeft = Physics2D.Linecast(wallOrigin.position,  wallOrigin.position - (Vector3.right * wallRadius), wallLayerMask);
        RaycastHit2D hitRight = Physics2D.Linecast(wallOrigin.position, wallOrigin.position - (Vector3.left *  wallRadius), wallLayerMask);
        if (hitRight)
        {
            return -1;
        }
        else if (hitLeft)
        {
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
        {
            if(!isGrounded)
                SoundManager.Instance.PlayPlayerSound(PlayerSoundStates.RUN);
            isGrounded = true;
        }
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
        //Gizmos.color = Color.green;
        //Gizmos.DrawWireSphere(groundOrigin.position, groundRadius);
        //Gizmos.color = Color.red;
        //Gizmos.DrawWireSphere(wallOrigin.position, wallRadius);

    }

    public void ResetGravity()
    {
        rigidbody.gravityScale = 3;
    }

    public void FollowSprite()
    {
        transform.position = animator.gameObject.transform.position;
        transform.position += -transform.right * originalLocalPosition.x;
        transform.position += -transform.up * originalLocalPosition.y;
        animator.gameObject.transform.localPosition = originalLocalPosition;
    }

    private void RegenerateEnergy()
    {
        if (energy < 0)
            energy = 0;
        if(energy < maxEnergy)
        {
            energy += energyRegenRate;
        }

        if (energy > maxEnergy)
            energy = maxEnergy;
    }

    public void TakeDamage(float damage, Vector2 attackdirection)
    {
        if (canTakeDamage)
        {
            health -= damage;
            canTakeDamage = false;
            Vector2 temp = new Vector2(-attackdirection.x * 10, 10);
            //Debug.Log(temp);
            rigidbody.AddForce(temp, ForceMode2D.Impulse);
            animator.TakeDamage(true);

            if(health <= 0)
            {
                animator.PlayDeath(true);
                isDead = true;
            }
        }
    }
}
