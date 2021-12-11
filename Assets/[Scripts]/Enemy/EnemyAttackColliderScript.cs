using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackColliderScript : MonoBehaviour
{
    float damage;
    bool canDamage = true;
    private void Start()
    {
        //damage = transform.parent.GetComponent<EnemyScript>().attackDamage;
    }
    private void OnDisable()
    {
        canDamage = true;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (canDamage)
        {
            if (collision.CompareTag("Player"))
            {
                PlayerController player = collision.gameObject.GetComponent<PlayerController>();
                Vector2 temp = collision.ClosestPoint(transform.position);
                Vector2 temp2 = new Vector2(temp.x - player.transform.position.x, temp.y - player.transform.position.y);
                temp2.Normalize();
                player.TakeDamage(transform.parent.transform.parent.GetComponent<EnemyScript>().attackDamage, temp2);
                canDamage = false;
            }
        }
    }
}
