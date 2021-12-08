using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    Rigidbody2D rigidbody;

    [Header("Movement")]
    public float runForce;
    public bool isGroundAhead;
    public bool isWallAhead;
    public Transform groundCheckPoint;
    public LayerMask groundLayerMask;
    public Transform wallCheckPoint;
    public LayerMask wallLayerMask;

    [Header("Attack")]
    public GameObject attackCollider;
    public bool isAttacking;
    public float attackDamage;

    [Header("Player Detection")]
    public LOS enemyLOS;
    public GameObject target;
    public LayerMask playerLayerMask;

    [Header("Stats")]
    public float maxHealth = 100;
    public float health;

    [Header("Animation")]
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody =  GetComponent<Rigidbody2D>();
        animator = transform.GetChild(0).GetComponent<Animator>();
        enemyLOS = GetComponent<LOS>();
        maxHealth = health;
    }

    public void Move()
    {
        rigidbody.AddForce(Vector2.right * runForce * transform.localScale.x);
        rigidbody.velocity *= 0.90f;
    }

    public void UpdateAnimator()
    {
        if (rigidbody.velocity.x != 0)
            animator.SetInteger("VelX", 1);
        else
            animator.SetInteger("VelX", 0);

        if (!isAttacking)
            attackCollider.SetActive(false);
    }

    public void Flip()
    {
        transform.localScale = new Vector3(transform.localScale.x * -1.0f, transform.localScale.y, transform.localScale.z);
    }

    public void LookAhead()
    {
        var hit = Physics2D.Linecast(transform.position, groundCheckPoint.position, groundLayerMask);
        isGroundAhead = (hit) ? true : false;

        var wallhit = Physics2D.Linecast(transform.position, wallCheckPoint.position, wallLayerMask);
        isWallAhead = (wallhit) ? true : false;
    }

    public bool HasLOS()
    {
        if (enemyLOS.colliderList.Count > 0)
        {
            // Case 1 enemy polygonCollider2D collides with player and player is at the top of the list
            if ((enemyLOS.collidesWith.gameObject.CompareTag("Player")) &&
                (enemyLOS.colliderList[0].gameObject.CompareTag("Player")))
            {
                return true;
            }
            // Case 2 player is in the Collider List and we can draw ray to the player
            else
            {
                foreach (var collider in enemyLOS.colliderList)
                {
                    if (collider.gameObject.CompareTag("Player"))
                    {
                        var hit = Physics2D.Raycast(wallCheckPoint.position, Vector3.Normalize(collider.transform.position - wallCheckPoint.position), 5.0f, enemyLOS.contactFilter.layerMask);

                        if ((hit) && (hit.collider.gameObject.CompareTag("Player")))
                        {
                            return true;
                        }
                    }
                }
            }
        }

        return false;
    }

    public void Attack()
    {
        animator.SetBool("isAttacking", true);
        attackCollider.SetActive(true);
        isAttacking = true;
    }

    public bool InRangeOfPlayer()
    {
        var wallhit = Physics2D.Linecast(transform.position, wallCheckPoint.position, playerLayerMask);
        //Debug.Log(wallhit.collider.tag);
        if (wallhit)
            return true;
        else
            return false;
        
    }
}
