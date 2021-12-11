using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackColliderScript : MonoBehaviour
{
    float damage;
    private void Start()
    {
        //damage = transform.parent.GetComponent<EnemyScript>().attackDamage;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            Vector2 temp = collision.ClosestPoint(transform.position);
            Vector2 temp2 = new Vector2(temp.x - player.transform.position.x, temp.y - player.transform.position.y);
            temp2.Normalize();
            player.TakeDamage(transform.parent.transform.parent.GetComponent<EnemyScript>().attackDamage, temp2);
        }
        
    }
}
