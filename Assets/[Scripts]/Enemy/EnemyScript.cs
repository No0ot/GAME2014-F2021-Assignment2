using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType
{
    GOBLIN,
    FLYINGEYE,
    SKELETON
}

public class EnemyScript : MonoBehaviour
{
     private Rigidbody2D rigidbody;
    public Rigidbody2D Rigidbody { get { return rigidbody; } }

    [Header("Movement")]
    public float maxSpeed;
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
    public bool isDead;
    public EnemyType type;

    [Header("Animation")]
    Animator animator;
    public Vector3 startPosition;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody =  GetComponent<Rigidbody2D>();
        animator = transform.GetChild(1).GetComponent<Animator>();
        enemyLOS = transform.GetChild(0).GetComponent<LOS>();
        health = maxHealth;
        isDead = false;
    }
    private void OnDisable()
    {
        GetComponent<Collider2D>().enabled = true;
        transform.localScale = new Vector3(1.0f, transform.localScale.y, transform.localScale.z);
    }

    private void OnEnable()
    {
        health = maxHealth;
        isDead = false;
        GetComponent<Collider2D>().enabled = true;
        startPosition = transform.position;
    }

    public void Move()
    {
        
        if (isGroundAhead)
        {

                if (target == null)
                    rigidbody.AddForce(Vector2.right * LimitSpeed(transform.localScale.x) * transform.localScale.x);
                else
                {
                    rigidbody.AddForce(Vector2.right * (LimitSpeed(transform.localScale.x) * 2) * transform.localScale.x);
                }
                rigidbody.velocity *= 0.99f;
            
        }
        else
            Flip();

        if (isWallAhead)
            Flip();

        if (!isAttacking)
            attackCollider.SetActive(false);
    }

    public void UpdateAnimator()
    {
        if (rigidbody.velocity.x != 0)
            animator.SetInteger("VelX", 1);
        else
            animator.SetInteger("VelX", 0);
    }

    public void Flip()
    {
        rigidbody.velocity *= 0.50f;
        transform.localScale = new Vector3(transform.localScale.x * -1.0f, transform.localScale.y, transform.localScale.z);
    }

    public void LookAhead()
    {
        var hit = Physics2D.Linecast(transform.position, groundCheckPoint.position, groundLayerMask);
        isGroundAhead = (hit) ? false : true;

        var wallhit = Physics2D.Linecast(transform.position, wallCheckPoint.position, wallLayerMask);
        isWallAhead = (wallhit) ? true : false;
    }

    private float LimitSpeed(float x)
    {
        float moveForce = 0;
        float tempMaxSpeed = maxSpeed;
        if (x > 0)
        {
            if (rigidbody.velocity.x < tempMaxSpeed)
                moveForce = x * runForce;
            else
                moveForce = 0;
        }
        else if (x < 0)
        {
            if (rigidbody.velocity.x > -tempMaxSpeed)
                moveForce = x * -runForce;
            else
                moveForce = 0;
        }
        return moveForce;
    }

    public bool HasLOS()
    {
        if (enemyLOS.colliderList.Count > 0)
        {
            // Case 1 enemy polygonCollider2D collides with player and player is at the top of the list
            if ((enemyLOS.collidesWith.gameObject.CompareTag("Player")) &&
                (enemyLOS.colliderList[0].gameObject.CompareTag("Player")))
            {
                target = enemyLOS.colliderList[0].gameObject;
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
                            target = enemyLOS.colliderList[0].gameObject;
                            return true;
                        }
                    }
                }
            }
        }
        target = null;
        return false;
    }

    public void Attack()
    {
        animator.SetBool("isAttacking", true);
        //attackCollider.SetActive(true);
        isAttacking = true;
    }

    public void AttackTowardsTarget()
    {
        animator.SetBool("isAttacking", true);
        attackCollider.SetActive(true);
        isAttacking = true;

        Vector3 targetdirection = new Vector3(transform.position.x - target.transform.position.x,
                                                      Vector2.down.y,
                                                      transform.position.z - target.transform.position.z);
        targetdirection.Normalize();

        rigidbody.AddForce(targetdirection * (runForce / 2) * transform.localScale.x);
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

    public void TakeDamage(float damage, Vector2 attackdirection)
    {
        health -= damage;
        Vector2 temp = new Vector2(-attackdirection.x * 10, 5);
        rigidbody.AddForce(temp, ForceMode2D.Impulse);
        animator.SetBool("TakeDamage", true);

        if(health <= 0)
        {
            attackCollider.SetActive(false);
            GetComponent<Collider2D>().enabled = false;
            isDead = true;
            animator.SetBool("isDead", true);
        }
            
    }
    
    public void ReturnToStartHeight()
    {
        if (startPosition.y + 0.5 < transform.position.y )
            rigidbody.AddForce(Vector2.down * runForce);
        else
            if(startPosition.y - 0.5 > transform.position.y )
            rigidbody.AddForce(Vector2.up * runForce);
    }
}
