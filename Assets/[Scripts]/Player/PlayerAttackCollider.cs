using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackCollider : MonoBehaviour
{
    public float damage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
            if (collision.CompareTag("Enemy"))
            {
                EnemyScript Enemy = collision.gameObject.GetComponent<EnemyScript>();
                Vector2 temp = collision.ClosestPoint(transform.position);
                Vector2 temp2 = new Vector2(temp.x - Enemy.transform.position.x, temp.y - Enemy.transform.position.y);
                temp2.Normalize();
                Enemy.TakeDamage(damage, temp2);

            }
        
    }
}
