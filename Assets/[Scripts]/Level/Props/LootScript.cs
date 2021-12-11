using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LootType
{
    COIN,
    GEM,
    HEART
}

public class LootScript : MonoBehaviour
{
    public LootType type;
    public Rigidbody2D rigidbody;

    public int score;

    private void OnEnable()
    {
        float xforce = Random.Range(-10f, 10f);
        float yforce = Random.Range(5, 10f);

        Vector2 force = new Vector2(xforce, yforce);
        rigidbody.AddForce(force, ForceMode2D.Impulse);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            switch (type)
            {
                case LootType.COIN:
                    PlayerProgressionScript temp = collision.GetComponent<PlayerProgressionScript>();
                    temp.scoreNum += score;
                    break;
                case LootType.GEM:
                    PlayerProgressionScript temp2 = collision.GetComponent<PlayerProgressionScript>();
                    temp2.scoreNum += score;
                    break;
                case LootType.HEART:
                    PlayerController temp3 = collision.GetComponent<PlayerController>();
                    temp3.health += score;
                    if (temp3.health > temp3.maxHealth)
                        temp3.health = temp3.maxHealth;
                    break;

            }
            rigidbody.gameObject.SetActive(false);
        }
    }
}
