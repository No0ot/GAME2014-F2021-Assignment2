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
    public int maxSpawnAmount;
    public bool canSpawn;
    public EnemyScript[] monsters;

    // Start is called before the first frame update
    void Start()
    {
        spawnPosition = transform;
        animator = transform.GetChild(0).GetComponent<Animator>();
        canSpawn = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (canSpawn && monsters.Length - 1 <= maxSpawnAmount)
        {
            spawnCounter += Time.deltaTime;
            if (spawnCounter >= spawnTime)
            {
                SpawnMonster();
                spawnCounter = 0;
            }
        }
        monsters = GetComponentsInChildren<EnemyScript>();

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
