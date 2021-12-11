using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerScript : MonoBehaviour
{
    public int coinMin;
    public int coinMax;
    public int gemMin;
    public int gemMax;

    private int coinNum;
    private int gemNum;

    public bool breakable;
    bool opened;

    Animator animator;

    private void Start()
    {
        coinNum = Random.Range(coinMin, coinMax + 1);
        gemNum = Random.Range(gemMin, gemMax + 1);
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

                    opened = true;
                    GenerateLoot();
                    Invoke("TurnOff", 0.4f);
                }
            }
            else
            {
                if (collision.CompareTag("Player"))
                {
                    animator.SetTrigger("Open");

                    opened = true;
                    GenerateLoot();
                }
            }
            
        }
    }

    private void GenerateLoot()
    {
        for (int i = 0; i < coinNum; i++)
        {
            GameObject temp = LootManager.Instance.GetLoot(LootType.COIN);
            temp.SetActive(true);
            temp.transform.position = transform.position;
        }
        for (int i = 0; i < gemNum; i++)
        {
            GameObject temp = LootManager.Instance.GetLoot(LootType.GEM);
            temp.SetActive(true);
            temp.transform.position = transform.position;
        }
    }

    void TurnOff()
    {
        gameObject.SetActive(false);
    }
}
