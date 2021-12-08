using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyScript : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            PlayerProgressionScript temp = collision.GetComponent<PlayerProgressionScript>();
            temp.keyNum++;
            Destroy(this.gameObject);
        }
    }
}
