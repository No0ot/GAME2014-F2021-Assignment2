using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    Transform spawnPosition;
    public EnemyType type;
    public float spawnTime;
    float spawnCounter;
    Animator animator;
    public bool canSpawn;

    // Start is called before the first frame update
    void Start()
    {
        spawnPosition = transform;
        animator = GetComponent<Animator>();
        canSpawn = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (canSpawn)
        {
            spawnCounter += Time.deltaTime;
            if (spawnCounter >= spawnTime)
            {
                SpawnMonster();
                spawnCounter = 0;
            }
        }
    }

    void SpawnMonster()
    {
        GameObject monster = EnemyManager.Instance.GetEnemy(type);
        monster.SetActive(true);
        monster.transform.position = spawnPosition.position;
        monster.transform.position = spawnPosition.position;
        monster.transform.parent = this.transform;
        animator.SetTrigger("Spawn");
     }
}
