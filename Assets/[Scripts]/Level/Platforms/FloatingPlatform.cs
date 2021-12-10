using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingPlatform : MonoBehaviour
{
    Rigidbody2D rigidbody;
    Vector3 originalPosition;

    public float floatForce;

    public bool floatUp;
    public bool floatDown;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        originalPosition = transform.position;
        floatUp = false;
        floatDown = false;
    }

    private void FixedUpdate()
    {
        if (floatUp && transform.position.y < originalPosition.y)
            rigidbody.AddForce(Vector2.up * floatForce);
        //else if(floatDown)
        //    rigidbody.MovePosition(new Vector2(rigidbody.position.x, rigidbody.position.y - floatForce));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            floatUp = false;
            floatDown = true;
            rigidbody.velocity *= 0;
            collision.gameObject.transform.SetParent(this.gameObject.transform);
            //rigidbody.bodyType = RigidbodyType2D.Dynamic;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            floatUp = false;
            floatDown = true;
            rigidbody.velocity *= 0;
            collision.gameObject.transform.SetParent(this.gameObject.transform);
            //rigidbody.bodyType = RigidbodyType2D.Dynamic;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            floatDown = false;
            collision.gameObject.transform.SetParent(null);
            rigidbody.velocity *= 0;
            //rigidbody.bodyType = RigidbodyType2D.Kinematic;
            floatUp = true;
        }
    }
}
