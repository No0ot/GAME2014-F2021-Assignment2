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
    public bool directionSwitch;
    public bool direction;
    public EnemyScript[] monsters;

    // Start is called before the first frame update
    void Start()
    {
        spawnPosition = transform;
        animator = transform.GetChild(0).GetComponent<Animator>();
        canSpawn = false;
        direction = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (canSpawn && monsters.Length < maxSpawnAmount)
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
        monster.transform.position = spawnPosition.position;
        monster.SetActive(true);
        monster.transform.parent = this.transform;
        animator.SetTrigger("Spawn");

        if (directionSwitch)
        {
            direction = !direction;
        }

        if (direction)
        {
            monster.transform.localScale = new Vector3(transform.localScale.x * -1.0f, transform.localScale.y, transform.localScale.z);
        }
    }
}
