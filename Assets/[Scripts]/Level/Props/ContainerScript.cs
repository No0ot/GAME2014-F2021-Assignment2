using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerScript : MonoBehaviour
{
    public int coinMin;
    public int coinMax;
    public int gemMin;
    public int gemMax;
    public int heartMin;
    public int heartMax;

    private int coinNum;
    private int gemNum;
    private int heartNum;

    public bool breakable;
    bool opened;

    Animator animator;

    private void Start()
    {
        coinNum = Random.Range(coinMin, coinMax + 1);
        gemNum = Random.Range(gemMin, gemMax + 1);
        heartNum = Random.Range(heartMin, heartMax + 1);
        animator = GetComponent<Animator>();

        opened = false;

        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!opened)
        {
            if (breakable)
            {
                if (collision.CompareTag("AttackCollider"))
                {
                    animator.SetTrigger("Open");

                    SoundManager.Instance.PlayJarBreak();
                    opened = true;
                    LootManager.Instance.GenerateLoot(coinNum, gemNum, heartNum, transform.position);
                    Invoke("TurnOff", 0.4f);
                }
            }
            else
            {
                if (collision.CompareTag("Player"))
                {
                    animator.SetTrigger("Open");
                    SoundManager.Instance.PlayChestOpen();
                    opened = true;
                    LootManager.Instance.GenerateLoot(coinNum, gemNum, heartNum, transform.position);
                }
            }
            
        }
    }

    void TurnOff()
    {
        gameObject.SetActive(false);
    }
}
