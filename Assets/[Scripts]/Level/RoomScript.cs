using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomScript : MonoBehaviour
{
    List<MonsterSpawner> spawners;

    private void Start()
    {
        spawners = new List<MonsterSpawner>();
        foreach (MonsterSpawner spawner in GetComponentsInChildren<MonsterSpawner>(true))
        {
            spawners.Add(spawner);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            foreach(MonsterSpawner spawner in spawners)
            {
                spawner.canSpawn = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            foreach (MonsterSpawner spawner in spawners)
            {
                spawner.canSpawn = false;
            }
        }
    }
}
