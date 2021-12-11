using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LootType
{
    COIN,
    GEM
}

public class LootScript : MonoBehaviour
{
    public LootType type;
    public Rigidbody2D rigidbody;

    private void OnEnable()
    {
        float xforce = Random.Range(-10f, 10f);
        float yforce = Random.Range(0, 10f);

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
            rigidbody.gameObject.SetActive(false);
        }
    }
}
