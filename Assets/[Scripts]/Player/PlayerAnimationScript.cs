using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationScript : MonoBehaviour
{
    public PlayerController controller;
    private Animator animator;
    Vector2 velocity;
    Vector2 input;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (input.x != 0)
            animator.SetBool("isRunning", true);
        else
            animator.SetBool("isRunning", false);

        animator.SetFloat("VelX", velocity.x);
        animator.SetFloat("VelY", velocity.y);
    }

    public void IsRunning(bool tf)
    {
        animator.SetBool("isRunning", tf);
    }

    public void IsJumping(bool tf)
    {
        animator.SetBool("isJumping", tf);
    }

    public void IsWallSliding(bool tf)
    {
        animator.SetBool("isWallSliding", tf);
    }

    public void IsGrounded(bool tf)
    {
        animator.SetBool("isGrounded", tf);
    }

    public void LightAttack()
    {
        animator.SetTrigger("LightAttack");
        //if (controller.isGrounded && !controller.isAttacking)
        //    controller.groundLightAttackCollider.SetActive(true);
        //else if(!controller.isAttacking)
        //    controller.airLightAttackCollider.SetActive(true);
    }

    public void HeavyAttack()
    {
        animator.SetTrigger("HeavyAttack");
        //if (controller.isGrounded && !controller.isAttacking)
        //    controller.groundHeavyAttackCollider.SetActive(true);
        //else if(!controller.isAttacking)
        //    controller.airHeavyAttackCollider.SetActive(true);
    }

    public void PassInVelocity(float x, float y)
    {
        velocity = new Vector2(x, y);
        
    }

    public void PassInInput(float x, float y)
    {
        input = new Vector2(x, y);
    }

    public void TakeDamage(bool tf)
    {
        animator.SetBool("TakeDamage", true);
    }

}

