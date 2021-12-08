using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    Animator animator;
    Collider2D collider2D;

    private void Start()
    {
        animator = GetComponent<Animator>();
        collider2D = GetComponent<Collider2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerProgressionScript temp = collision.gameObject.GetComponent<PlayerProgressionScript>();
            if(temp.keyNum > 0)
            {
                collider2D.enabled = false;
                animator.SetBool("Unlocked", true);
                temp.keyNum -= 1;
            }   
        }
    }
}
