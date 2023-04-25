using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathHelpers
{
    public static Vector2 RadianToVector2(float radian)
    {
        return new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));
    }
    public static Vector2 RadianToVector2(float radian, float length)
    {
        return RadianToVector2(radian) * length;
    }
    public static Vector2 DegreeToVector2(float degree)
    {
        return RadianToVector2(degree * Mathf.Deg2Rad);
    }
    public static Vector2 DegreeToVector2(float degree, float length)
    {
        return RadianToVector2(degree * Mathf.Deg2Rad) * length;
    }
}

public class PlayerAttackCollider : MonoBehaviour
{
    public float damage;
    public float knockbackDegrees;
    public float knockbackForce;

    private void OnTriggerEnter2D(Collider2D collision)
    {
            if (collision.CompareTag("Enemy"))
            {
                Debug.Log("hit");
                EnemyScript Enemy = collision.gameObject.GetComponent<EnemyScript>();
                Vector2 temp = collision.ClosestPoint(transform.position);
                Vector2 knockback = MathHelpers.DegreeToVector2(knockbackDegrees) * knockbackForce;
                ///knockback.Normalize();
                Enemy.TakeDamage(damage, knockback);

            }
        
    }
}
